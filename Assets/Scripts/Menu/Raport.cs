using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum RaportType {
	StartRaport,
	EndRaport
}

[RequireComponent(typeof(Text))]
public class Raport : MonoBehaviour {
	public RaportType raportType = RaportType.StartRaport;

	private Text text;

	private void Awake() {
		GameDBCSV db = GlobalSettings.db;

		text = GetComponent<Text>();

		if (raportType == RaportType.StartRaport) {
			text.text = db.raports[0];
		}
		else {
			if (GlobalSettings.win)
				text.text = db.raports[1];
			else
				text.text = db.raports[2];
		}
	}
}
