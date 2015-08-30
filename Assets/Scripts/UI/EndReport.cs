using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class EndReport : MonoBehaviour {
	private void Awake() {
		GameDBCSV db = GlobalSettings.instance.db;
		Text text = GetComponent<Text>();

		if (GlobalSettings.instance.win)
			text.text = db.GetTranslation("Good Report");
		else
			text.text = db.GetTranslation("Bad Report");
	}
}
