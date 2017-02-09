using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoSingleton<GameLogic> {
	public InterstageFadeUI interstageFadeUI;

	public Clock clock;
	public GameObject suspectButton;
	public string theme;

	private GameDBCSV db;
	private Item mimic;

	public void Awake() {
		db = GlobalSettings.instance.db;

		GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
			List<Item> items = new List<Item>();
		for (int i = 0; i < itemObjects.Length; ++i) {
			Item item = itemObjects[i].GetComponent<Item>();
			if (item != null) {
				items.Add(item);
				item.type = db.types[item.index];
			}
		}

		// Set mimic
		bool rightChoise = false;
		while (!rightChoise) {
			int mimicNumber = Random.Range(0, items.Count);
			rightChoise = true;
			for (int i = 1; i <= 7; i++) {
				if (items[mimicNumber].type == ("Note" + i)) {
					rightChoise = false;
					break;
				}
			}
			if (rightChoise) {
				mimic = items[mimicNumber];
				items[mimicNumber].mimic = true;
			}
		}

		theme = db.themes[Random.Range(0, db.themes.Count)];
		foreach (var item in items) {
			// Set name and price
			List<NameData> names = (from name in db.names where name.type == item.type select name).ToList();
			int numNames = Random.Range(0, names.Count);
			item.label = names[numNames].name;
			item.price = names[numNames].price;

			// Set descriptions
			List<DescriptionData> descriptions = new List<DescriptionData>();
			foreach (var d in db.descriptions) {
				if (item.type != d.type) continue;
				if (d.mimic && item != mimic) continue;
				if (d.theme != "" && d.theme != theme) continue;

				descriptions.Add(d);
			}

			if (descriptions.Count == 0) continue;

			List<DescriptionData> chosenDescriptions = new List<DescriptionData>();
			int groupsNumber = descriptions.Max(d => d.group);
			for (int i = 0; i <= groupsNumber; i++) {
				List<DescriptionData> descriptionsInGroup = new List<DescriptionData>();
				foreach (var d in descriptions)
					if (d.group == i)
						descriptionsInGroup.Add(d);
				if (descriptionsInGroup.Count > 0) {
					chosenDescriptions.Add(descriptionsInGroup[Random.Range(0, descriptionsInGroup.Count)]);
				}
			}

			item.description = "";
			for (int i = 0; i < chosenDescriptions.Count; i++) {
				if (chosenDescriptions[i].probability >= Random.Range(0.0f, 1.0f)) {
					item.description += "• " + chosenDescriptions[i].text + "\n";
				}
			}
			if (string.IsNullOrEmpty(item.description))
				item.description += "• " + chosenDescriptions[Random.Range(0, chosenDescriptions.Count)].text + "\n";
		}
	}

	public void Update() {
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space) && mimic != null) {
			Debug.Log(mimic.gameObject.name);
			mimic.SelectedDebug();
		}
		#endif
	}

	public void EndGame() {
		clock.gameObject.SetActive(false);
		suspectButton.SetActive(false);

		GlobalSettings.instance.win = Player.instance.selectedItem == mimic;

		interstageFadeUI.gameObject.SetActive(true);
		interstageFadeUI.FadeToLevel("End");
	}
}
