using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TranslationText : MonoBehaviour {
	public int index;

	private void Awake() {
		GetComponent<Text>().text = GlobalSettings.instance.db.GetTranslation(index);
	}
}
