using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Player : MonoSingleton<Player> {
	[Header("Sounds")]
	public AudioSource selectedSound;
	public AudioSource showInfoSound;

	[Header("Panel")]
	public Transform panel;
	public Text labelText;
	public Text priceText;
	public Text descriptionText;
	public float offset = 150.0f;
	public float offsetWidth = 260.0f;
	public float offsetHeight = 410.0f; 

	[Header("Select")]
	public UnityStandardAssets.ImageEffects.Fisheye fisheye;
	public GameObject suspectButton;

	[HideInInspector]
	public Item selectedItem;

	private Camera mainCamera;
	private Item highlightedItem;

	private Vector2 intensity = Vector2.zero;
	private bool onSuspect = false;

	private GameDBCSV db;

	private void Awake() {
		db = GlobalSettings.instance.db;
	}

	private void Start() {
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		suspectButton.SetActive(false);
	}

	private void Update () {
		Vector2 screenSize = new Vector2(mainCamera.pixelWidth, mainCamera.pixelHeight);
		
		Ray ray = mainCamera.ScreenPointToRay(GetMousePosition(screenSize));
		
		RaycastHit hitItem;
		RaycastHit hitWall;

		bool itemCollided = Physics.Raycast(ray, out hitItem, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item"));
		bool wallCollided = Physics.Raycast(ray, out hitWall, Mathf.Infinity, 1 << LayerMask.NameToLayer("Default"));

		Item item = null;
		if (itemCollided) item = hitItem.transform.GetComponent<Item>();

		if (itemCollided && (!wallCollided || hitItem.distance < hitWall.distance) && item != null) {
			// Highlight
			if (highlightedItem != null)
				highlightedItem.Hide();

			if (item != highlightedItem && !showInfoSound.isPlaying) 
				showInfoSound.Play();

			highlightedItem = item;
			highlightedItem.Show();

			// Select
			if (!onSuspect && Input.GetMouseButtonDown(0)) {
				if (selectedItem != null) {
					selectedItem.Unselect();
					selectedItem.Hide();

					if (!selectedSound.isPlaying)
						selectedSound.Play();
				}

				if (selectedItem != highlightedItem) {
					selectedItem = highlightedItem;
					selectedItem.Select();
					suspectButton.SetActive(true);

					if (!selectedSound.isPlaying)
						selectedSound.Play();
				}
				else {
					selectedItem = null;
					suspectButton.SetActive(false);
					onSuspect = false;
				}
			}

			// Update panel
			labelText.text = "<b>" + highlightedItem.label + "</b>";
			priceText.text = "<b>" + db.GetTranslation("Price") + ":</b> " + highlightedItem.price + " ₽";
			descriptionText.text = "<b>" + db.GetTranslation("Descritption") + ":</b>\n" + highlightedItem.description;
			panel.gameObject.SetActive(true);

			// Place a panel not on the edge and with offset.
			Vector3 itemPosition = mainCamera.WorldToScreenPoint(hitItem.transform.position);

			float dirOffset = (itemPosition.x > screenSize.x / 2) ? +offset : -offset;
			Vector3 panelPosition = VectorExt.WithZ(itemPosition, 0.0f) + Vector3.left * dirOffset;

			if (panelPosition.x > screenSize.x - offsetWidth) 
				panelPosition = VectorExt.WithX(panelPosition, screenSize.x - offsetWidth);
			if (panelPosition.x < offsetWidth) 
				panelPosition = VectorExt.WithX(panelPosition, offsetWidth);

			if (panelPosition.y > screenSize.y - offsetHeight) 
				panelPosition = VectorExt.WithY(panelPosition, screenSize.y - offsetHeight);
			if (panelPosition.y < offsetHeight) 
				panelPosition = VectorExt.WithY(panelPosition, offsetHeight);

			panel.position = panelPosition;
		}
		else {
			panel.gameObject.SetActive(false);
			if (highlightedItem != null) {
				highlightedItem.Hide();
				highlightedItem = null;
			}
		}
	}

	private Vector2 GetMousePosition(Vector2 screenSize) {
		// Applying the inverse fisheye
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

		return fisheyeMousePosition;
	}

	public void OnSuspect(bool onSuspect) {
		this.onSuspect = onSuspect;
	}
}
