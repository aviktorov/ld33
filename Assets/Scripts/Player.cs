using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoSingleton<Player> {
	[Header("Panel")]
	public AudioSource selectedSound;
	public AudioSource showInfoSound;

	[Header("Panel")]
	public Transform panel;
	public Text labelText;
	public Text priceText;
	public Text descriptionText;
	public float offset = 150.0f;
	
	public Item selectedItem;

	public UnityStandardAssets.ImageEffects.Fisheye fisheye;

	private Camera mainCamera;
	private Item highlightedItem;

	private Vector2 intensity = Vector2.zero;

	private void Start() {
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
	}

	private void Update () {
		RaycastHit hitItem;
		RaycastHit hitWall;

		Vector2 screenSize = new Vector2(mainCamera.pixelWidth, mainCamera.pixelHeight);

		// Fisheye for mousePosition
		float oneOverBaseSize = 80.0f / 512.0f;
		float ar = (screenSize.x * 1.0f) / (screenSize.y * 1.0f);

		intensity.x = -fisheye.strengthX * ar * oneOverBaseSize;
		intensity.y = -fisheye.strengthY * oneOverBaseSize;
		Vector2 mousePosition = Input.mousePosition;
		Vector2 mousePositionNormalized = new Vector2(mousePosition.x / screenSize.x, mousePosition.y / screenSize.y);
		Vector2 coords = 2.0f * (mousePositionNormalized - 0.5f * Vector2.one);
		
		Vector2 realCoordOffs = new Vector2((1.0f - coords.y * coords.y) * intensity.y * coords.x, (1.0f - coords.x * coords.x) * intensity.x * coords.y);

		Vector2 fisheyeMousePosition = mousePositionNormalized + realCoordOffs;
		fisheyeMousePosition.x *= screenSize.x;
		fisheyeMousePosition.y *= screenSize.y;

		Ray ray = mainCamera.ScreenPointToRay(fisheyeMousePosition);
		bool wallCollided = Physics.Raycast(ray, out hitWall, Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));

		if (Physics.Raycast(ray, out hitItem, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item")) && (!wallCollided || hitItem.distance < hitWall.distance)) {
			Item item = hitItem.transform.GetComponent<Item>();
			if (item != null) {
				if (highlightedItem != null)
					highlightedItem.Hide();

				if (item != highlightedItem && !showInfoSound.isPlaying) 
					showInfoSound.Play();

				highlightedItem = item;
				highlightedItem.Show();

				if (Input.GetMouseButtonDown(0)) {
					if (selectedItem != null) {
						selectedItem.Unselect();
						selectedItem.Hide();
					}

					if (selectedItem != highlightedItem) {
						selectedItem = highlightedItem;
						selectedItem.Select();
						if (!selectedSound.isPlaying)
							selectedSound.Play();
					}
					else {
						selectedItem = null;
					}
				}

				labelText.text = "<b>" + highlightedItem.label + "</b>";
				priceText.text = "<b>Price:</b> " + highlightedItem.price + " ₽";
				descriptionText.text = "<b>Descritption:</b>\n" + highlightedItem.description;
				panel.gameObject.SetActive(true);
			}

			// Place a panel not on the edge and with offset.
			Vector3 itemPosition = mainCamera.WorldToScreenPoint(hitItem.transform.position);

			float dirOffset = 0.0f;
			if (itemPosition.x < screenSize.x / 2)
				dirOffset = -offset;
			else
				dirOffset = offset;

			panel.position = VectorExt.WithZ(itemPosition, 0.0f) + Vector3.left * dirOffset;

			if (panel.position.x > screenSize.x - offset) 
				panel.position = VectorExt.WithX(panel.position, panel.position.x - (screenSize.x - panel.position.x + 50));
			if (panel.position.x < offset) 
				panel.position = VectorExt.WithX(panel.position, panel.position.x + (50 + panel.position.x));

			if (panel.position.y > screenSize.y - offset) 
				panel.position = VectorExt.WithY(panel.position, panel.position.y - (screenSize.y - panel.position.y + 100));
			if (panel.position.y < offset) 
				panel.position = VectorExt.WithY(panel.position, panel.position.y + (100 + panel.position.y));
		}
		else {
			panel.gameObject.SetActive(false);
			if (highlightedItem != null) {
				highlightedItem.Hide();
				highlightedItem = null;
			}
		}
	}
}
