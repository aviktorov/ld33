using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameLogic : MonoBehaviour {
	[Header("Statistics")]
	public GameObject statisticsPanel;
	public Text statisticsText;
	public Color trueMimicColor = Color.green;
	public Color falseMimicColor = Color.red;
	public Color notSelectedMimicColor = Color.blue;
	public Timer timer;

	[Header("Game flow")]
	public GameObject endButton;
	public GameObject restartButton;

	public void EndGame() {
		timer.isStop = true;
		endButton.SetActive(false);
		restartButton.SetActive(true);

		statisticsPanel.SetActive(true);

		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		statisticsText.text = "<b>Statistics (" + timer.GetTime() + "):</b>\n";
		foreach (var itemObject in items) {
			Item item = itemObject.GetComponent<Item>();
			if (item != null) {
				if (item.selected) {
					statisticsText.text += "<color=\"#" + (item.isMimic ? ColorExt.ToHexString(trueMimicColor) : ColorExt.ToHexString(falseMimicColor)) +"\">";
					statisticsText.text += item.label + " " + (item.isMimic ? "(True)" : "(False)") + "</color>\n";
				}
				else if (item.isMimic) 
					statisticsText.text += "<color=\"#" + ColorExt.ToHexString(notSelectedMimicColor) +"\">" + item.label + " (NotSelected) </color>\n";
			}
		}
	}

	public void Restart() {
		Application.LoadLevel(0);
	}
}
