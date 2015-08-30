using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextTranslation : MonoBehaviour {
	public int index;

	private void Awake() {
		GetComponent<Text>().text = GlobalSettings.instance.db.GetTranslation(index);
	}
}
