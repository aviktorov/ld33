using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoSingleton<GameLogic> {
	public InterstageFadeUI interstageFadeUI;

	public Timer timer;
	public GameObject suspectButton;
	public string theme;

	private GameDBCSV db;
	private Item mimic;

	public void Awake() {
		db = GlobalSettings.db;

		GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
		Item[] items = new Item[itemObjects.Length];
		for (int i = 0; i < items.Length; ++i) {
			items[i] = itemObjects[i].GetComponent<Item>();
		}

		int mimicNumber = 0;
		bool chose = false;
		while (!chose) {
			chose = true;
			mimicNumber = Random.Range(0, items.Length);
			for (int i = 1; i <= 8; i++) {
				if (items[mimicNumber] == null || items[mimicNumber].type == ("Note" + i)) {
					chose = false;
					break;
				}
			}
		}

		theme = db.themes[Random.Range(0, db.themes.Count)];
		int currentNumber = 0;
		foreach (var item in items) {
			if (item == null) continue;
			item.type = db.types[item.index];

			// Set mimic
			if (mimicNumber == currentNumber) {
				mimic = item;
				item.mimic = true;
			}

			// Set name and price
			List<NameData> names = (from name in db.names where name.type == item.type select name).ToList();
			int numNames = Random.Range(0, names.Count);
			item.label = names[numNames].name;
			item.price = names[numNames].price;

			// Set descriptions
			List<DescriptionData> descriptions = new List<DescriptionData>();
			foreach (var d in db.descriptions) {
				if (item.type == d.type && (mimicNumber == currentNumber || !d.mimic) && (d.theme == "" || d.theme == theme)) {
					descriptions.Add(d);
				}
			}

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

			int descriptionsNumber = 0;
			item.description = "";
			for (int i = 0; i < chosenDescriptions.Count; i++) {
				if (chosenDescriptions[i].probability >= Random.Range(0.0f, 1.0f)) {
					descriptionsNumber++;
					item.description += "• " + chosenDescriptions[i].text + "\n";
				}
			}
			if (descriptionsNumber == 0)
				item.description += "• " + chosenDescriptions[Random.Range(0, chosenDescriptions.Count)].text + "\n";

			currentNumber++;
		}
	}

	public void Update() {
		if (Player.instance.selectedItem != null)
			suspectButton.SetActive(true);
		else 
			suspectButton.SetActive(false);
			
		if (Input.GetKeyDown(KeyCode.Space) && mimic != null) {
			Debug.Log(mimic.gameObject.name);
			mimic.SelectedDebug();
		}
	}

	public void EndGame() {
		timer.gameObject.SetActive(false);
		suspectButton.SetActive(false);

		GlobalSettings.win = Player.instance.selectedItem == mimic;

		interstageFadeUI.gameObject.SetActive(true);
		interstageFadeUI.FadeToLevel("End");
	}
}
