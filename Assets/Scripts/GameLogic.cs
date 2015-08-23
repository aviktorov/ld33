using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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

	public void Awake() {
		GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
		Item[] items = new Item[itemObjects.Length];
		for (int i = 0; i < items.Length; ++i)
			items[i] = itemObjects[i].GetComponent<Item>();

		foreach (var item in items) {
			// Set name
			List<NameData> names = (from name in db.names where name.type == item.type select name).ToList();
			item.label = names[Random.Range(0, names.Count)].name;

			// Set descriptions
			List<DescriptionData> descriptions = (from d in db.descriptions where d.type == item.type select d).ToList();
			List<DescriptionData> chosenDescriptions = new List<DescriptionData>();
			int groupsNumber = 0;
			int.TryParse(descriptions.Max(d => d.group), out groupsNumber);
			for (int i = 0; i <= groupsNumber; i++) {
				List<DescriptionData> descriptionsInGroup = new List<DescriptionData>();
				foreach (var d in descriptions) {
					int id = 0;
					int.TryParse(d.group, out id);
					if (i == id) 
						descriptionsInGroup.Add(d);
				}
				if (descriptionsInGroup.Count > 0)
					chosenDescriptions.Add(descriptionsInGroup[Random.Range(0, descriptionsInGroup.Count)]);
			}

			int descriptionsNumber = 0;
			item.description = "";
			for (int i = 0; i < chosenDescriptions.Count; i++) {
				float probability = 0.0f;
				float.TryParse(chosenDescriptions[i].probability, out probability);
				if (probability >= Random.Range(0.0f, 1.0f)) {
					descriptionsNumber++;
					item.description += "• " + chosenDescriptions[i].text + "\n";
				}
			}
			if (descriptionsNumber == 0)
				item.description += "• " + chosenDescriptions[Random.Range(0, chosenDescriptions.Count)].text + "\n";

			// Set price
			item.price = descriptions[0].price;
		}
	}

	public void EndGame() {
		timer.isStop = true;
		timerObject.SetActive(false);
		endButton.SetActive(false);
		restartButton.SetActive(true);

		statisticsPanel.SetActive(true);

		GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
		statisticsText.text = "<b>Statistics (" + timer.GetTime() + "):</b>\n";
		foreach (var itemObject in itemObjects) {
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
