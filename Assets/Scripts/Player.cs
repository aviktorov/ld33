using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoSingleton<Player> {
	[Header("Panel")]
	public Transform panel;
	public Text labelText;
	public Text priceText;
	public Text descriptionText;
	public float offset = 150.0f;

	private Camera cachedCamera;
	private Item currentItem;

	private void Start() {
		cachedCamera = Camera.main;
	}

	private void Update () {
		RaycastHit hitItem;
		RaycastHit hitWall;
		Ray ray = cachedCamera.ScreenPointToRay(Input.mousePosition);
		bool wallCollided = Physics.Raycast(ray, out hitWall, Mathf.Infinity, 1 << LayerMask.NameToLayer("Wall"));
		if (Physics.Raycast(ray, out hitItem, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item")) && (!wallCollided || hitItem.distance < hitWall.distance)) {
			Item item = hitItem.transform.GetComponent<Item>();
			if (item != null) {
				if (currentItem != null)
					currentItem.Hide();
				item.Show();
				currentItem = item;

				if (Input.GetMouseButtonDown(0)) {
					currentItem.Seleted();
				}

				labelText.text = "<b>" + item.label + "</b>";
				priceText.text = "<b>Price:</b> " + item.price + " ₽";
				descriptionText.text = "<b>Descritption:</b>\n" + item.description;
			}

			panel.gameObject.SetActive(true);

			Vector3 itemPosition = cachedCamera.WorldToScreenPoint(hitItem.transform.position);

			float dirOffset = 0.0f;
			if (itemPosition.x < cachedCamera.pixelWidth / 2)
				dirOffset = -offset;
			else
				dirOffset = offset;

			panel.position = VectorExt.WithZ(itemPosition, 0.0f) + Vector3.left * dirOffset;

			if (panel.position.x > cachedCamera.pixelWidth - offset) 
				panel.position = VectorExt.WithX(panel.position, panel.position.x - (cachedCamera.pixelWidth - panel.position.x + 50));
			if (panel.position.x < offset) 
				panel.position = VectorExt.WithX(panel.position, panel.position.x + (50 + panel.position.x));

			if (panel.position.y > cachedCamera.pixelHeight - offset) 
				panel.position = VectorExt.WithY(panel.position, panel.position.y - (cachedCamera.pixelHeight - panel.position.y + 100));
			if (panel.position.y < offset) 
				panel.position = VectorExt.WithY(panel.position, panel.position.y + (100 + panel.position.y));
		}
		else {
			panel.gameObject.SetActive(false);
			if (currentItem != null) {
				currentItem.Hide();
				currentItem = null;
			}
		}
	}
}
