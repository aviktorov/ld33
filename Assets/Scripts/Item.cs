using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	public int index = 0;
	public bool selected = false;

	public string type = "TV";
	public bool mimic = false;
	public string label = "LCD \"Horizont\"";
	public string price = "100₽";
	public string description = "• Wow! This is brand new TV! And they added remote control to it.";

	[HideInInspector]
	public GameObject outline;
	
	[HideInInspector]
	public Renderer outlineRenderer;

	private bool selectedDebug = false;

	private void Awake() {
		outlineRenderer.sharedMaterial = OutlineSettings.instance.highlightedMaterial;
		outlineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
	}

	public void Select() {
		selected = true;
		if (!selectedDebug)
			outlineRenderer.sharedMaterial = OutlineSettings.instance.selectedMaterial;
	}

	public void Unselect() {
		selected = false;
		if (!selectedDebug)
			outlineRenderer.sharedMaterial = OutlineSettings.instance.highlightedMaterial;
	}

	public void SelectedDebug() {
		if (selectedDebug) {
			selectedDebug = false;
			if (selected) {
				outlineRenderer.sharedMaterial = OutlineSettings.instance.selectedMaterial;
			}
			else {
				outline.SetActive(false);
				outlineRenderer.sharedMaterial = OutlineSettings.instance.highlightedMaterial;
			}
		}
		else {
			outline.SetActive(true);
			selectedDebug = true;
			outlineRenderer.sharedMaterial = OutlineSettings.instance.selectedDebugMaterial;
		}
	}

	public void Show() {
		outline.SetActive(true);
	}

	public void Hide() {
		if (!selected && !selectedDebug)
			outline.SetActive(false);
	}
}
