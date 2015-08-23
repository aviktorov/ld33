using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameLogic : MonoSingleton<GameLogic> {
	[Header("Statistics")]
	public GameObject statisticsPanel;
	public Text statisticsText;
	public GameObject timerObject;
	public Color trueMimicColor = Color.green;
	public Color falseMimicColor = Color.red;
	public Color notSelectedMimicColor = Color.blue;
	public Timer timer;

	[Header("Game flow")]
	public GameObject endButton;
	public GameObject restartButton;

	[Header("Logic")]
	public GameDB db;

	public void EndGame() {
		timer.isStop = true;
		timerObject.SetActive(false);
		endButton.SetActive(false);
		restartButton.SetActive(true);

		statisticsPanel.SetActive(true);

		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		statisticsText.text = "<b>Statistics (" + timer.GetTime() + "):</b>\n";
		foreach (var itemObject in items) {
			Item item = itemObject.GetComponent<Item>();
			if (item != null) {
				if (item.selected) {
					statisticsText.text += "<color=\"#" + (item.mimic ? ColorExt.ToHexString(trueMimicColor) : ColorExt.ToHexString(falseMimicColor)) +"\">";
					statisticsText.text += item.label + " " + (item.mimic ? "(True)" : "(False)") + "</color>\n";
				}
				else if (item.mimic) 
					statisticsText.text += "<color=\"#" + ColorExt.ToHexString(notSelectedMimicColor) +"\">" + item.label + " (NotSelected) </color>\n";
			}
		}
	}

	public void Restart() {
		Application.LoadLevel(0);
	}
}
