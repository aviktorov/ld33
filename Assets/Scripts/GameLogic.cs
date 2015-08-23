using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoSingleton<GameLogic> {
	public GameDB db;
	public Timer timer;
	public GameObject suspectButton;

	private Item mimic;

	public void Awake() {
		GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
		Item[] items = new Item[itemObjects.Length];
		for (int i = 0; i < items.Length; ++i)
			items[i] = itemObjects[i].GetComponent<Item>();

		int mimicNumber = Random.Range(0, items.Length);
		int currentNumber = 0;
		foreach (var item in items) {
			// Set mimic
			if (mimicNumber == currentNumber) {
				mimic = item;
				item.mimic = true;
			}

			// Set name
			List<NameData> names = (from name in db.names where name.type == item.type select name).ToList();
			item.label = names[Random.Range(0, names.Count)].name;

			// Set descriptions
			List<DescriptionData> descriptions = (from d in db.descriptions where (d.type == item.type && (mimicNumber == currentNumber || d.mimic != "1")) select d).ToList();
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

			currentNumber++;
		}
	}

	public void Update() {
		if (Input.GetKeyDown(KeyCode.Space))
			mimic.SelectedDebug();
	}

	public void EndGame() {
		timer.gameObject.SetActive(false);
		suspectButton.SetActive(false);

		if (Player.instance.selectedItem == mimic) 
			Debug.Log("Win!");
		else
			Debug.Log("Lose!");
	}
}
