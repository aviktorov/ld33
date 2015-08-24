using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public TextAnimation[] texts;
	public InterstageFade interstageFade;
	public InterstageFadeUI interstageFadeUI;

	private int index = 0;
	private bool done = false;

	private void Start() {
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
			}
		}

		if (Input.anyKeyDown) {
			interstageFade.gameObject.SetActive(true);
			interstageFadeUI.gameObject.SetActive(true);
			interstageFade.FadeToLevel("Main");
			interstageFadeUI.FadeToLevel("Main");
		}
	}
}
