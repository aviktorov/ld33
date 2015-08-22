using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameLogic : MonoBehaviour {
	[Header("Statistics")]
	public GameObject statisticsPanel;
	public Text statisticsText;
	public string trueMimicColor;
	public string falseMimicColor;

	public void EndGame() {
		statisticsPanel.SetActive(true);

		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		statisticsText.text = "<b>Statistics:</b>\n";
		foreach (var itemObject in items) {
			Item item = itemObject.GetComponent<Item>();
			if (item != null) {
				if (item.selected) {
					statisticsText.text += "<color=\"" + (item.isMimic ? trueMimicColor : falseMimicColor) +"\">" + item.label + "</color>\n";
				}
			}
		}
	}
}
