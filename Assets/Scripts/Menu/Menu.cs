using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {
	public bool anyKeyToContinue = true;
	public TextAnimation[] texts;
	public InterstageFadeUI interstageFadeUI;

	private int index = 0;
	private bool done = false;

	private void Start() {
		if (texts.Length == 0)
			done = true;
		else
			texts[index].Show();
	}

	private void Update() {
		if (!done && texts[index].done) {
			if(index + 1 < texts.Length) {
				index++;
				texts[index].Show();
			}
			else {
				texts[index].end = true;
				done = true;
			}
		}

		if (!done && Input.anyKeyDown) {
			texts[index].ShowFull();
			if(index + 1 < texts.Length) {
				index++;
				texts[index].Show();
			}
			else {
				texts[index].end = true;
				done = true;
				return;
			}
		}

		if (done && anyKeyToContinue && Input.anyKeyDown) {
			interstageFadeUI.FadeToLevel("Main");
		}
	}

	public void SetLanguage(string language) {
		if (language == "Russian") GlobalSettings.instance.language = Language.Russian;
		else GlobalSettings.instance.language = Language.English;
		interstageFadeUI.FadeToLevel("Start");
	}
}
