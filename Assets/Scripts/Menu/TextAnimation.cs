using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextAnimation : MonoBehaviour {
	[HideInInspector]
	public bool done = false;

	[HideInInspector]
	public bool end = false;

	public float indentDelay = 0.4f;
	public float delay = 0.02f;

	public float flickTime = 0.5f;
	private bool showSquare = true;

	private Text text;
	private string finalText;
	private string currentText = "";
	private int index = 0;

	private float timer = 0;
	private float indentTimer = 0;

	private bool show = false;

	private void Awake () {
		text = GetComponent<Text>();
		finalText = text.text;
		text.text = "";
	}
	
	private void Update () {
		if (timer >= 0.0f)
			timer -= Time.deltaTime;

		if (indentTimer >= 0.0f)
			indentTimer -= Time.deltaTime;

		if (show && indentTimer > 0.0f) {
			if (timer < 0.0f) {
				if (showSquare) {
					text.text = currentText;
					showSquare = false;
					timer = flickTime;
				}
				else {
					text.text = currentText + '■';
					showSquare = true;
					timer = flickTime;
				}
			}
		}

		if (show && indentTimer < 0.0f) {
			if (!done) {
				if (timer < 0.0f) {
					if (finalText[index] == '\n')
						indentTimer = indentDelay;
					currentText += finalText[index];
					text.text = currentText + '■';
					timer = delay;
					
					index++;
					if (index == finalText.Length) {
						text.text = currentText;
						done = true;
					}
				}
			}
			else if(end) {
				if (timer < 0.0f) {
					if (showSquare) {
						text.text = currentText;
						showSquare = false;
						timer = flickTime;
					}
					else {
						text.text = currentText + '■';
						showSquare = true;
						timer = flickTime;
					}
				}
			}
		}
	}

	public void Show() {
		show = true;
		indentTimer = indentDelay;
	}
}
