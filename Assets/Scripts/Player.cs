using UnityEngine;
using System.Collections;

public class Player : MonoSingleton<Player> {
	public Transform panel;
	
	public Material outlineMaterial;

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
			}

			panel.gameObject.SetActive(true);

			Vector3 itemPosition = cachedCamera.WorldToScreenPoint(hitItem.transform.position);

			float offset = 0.0f;
			if (itemPosition.x < cachedCamera.pixelWidth / 2)
				offset = -150.0f;
			else
				offset = 150.0f;

			panel.position = VectorExt.WithZ(itemPosition, 0.0f) + Vector3.left * offset;

			if (panel.position.x > cachedCamera.pixelWidth - 175) 
				panel.position = VectorExt.WithX(panel.position, panel.position.x - (cachedCamera.pixelWidth - panel.position.x + 50));
			if (panel.position.x < 175) 
				panel.position = VectorExt.WithX(panel.position, panel.position.x + (50 + panel.position.x));

			if (panel.position.y > cachedCamera.pixelHeight - 150) 
				panel.position = VectorExt.WithY(panel.position, panel.position.y - (cachedCamera.pixelHeight - panel.position.y + 100));
			if (panel.position.y < 150) 
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
