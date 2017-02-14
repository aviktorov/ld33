using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TextAnimation : MonoBehaviour {
	[HideInInspector]
	public bool done = false;

	[HideInInspector]
	public bool end = false;

	public AudioSource sound;

	public float pauseDelay = 0.9f;
	public float typeDelay = 0.05f;
	public float flickDelay = 0.3f;

	private bool showSquare = true;

	private Text text;
	private string finalText;
	private string currentText = "";
	private int index = 0;

	private float timer = 0;
	private float pauseTimer = 0;

	private bool show = false;

	private void Start () {
		text = GetComponent<Text>();
		finalText = text.text;
		text.text = "";
	}
	
	private void Update () {
		if (timer >= 0.0f)
			timer -= Time.deltaTime;

		if (pauseTimer >= 0.0f)
			pauseTimer -= Time.deltaTime;

		if (show && pauseTimer >= 0.0f) {
			if (timer < 0.0f) {
				if (showSquare) {
					text.text = currentText;
					showSquare = false;
					timer = flickDelay;
				}
				else {
					text.text = currentText + '■';
					showSquare = true;
					timer = flickDelay;
				}
			}
		}

		if (show && pauseTimer < 0.0f) {
			if (!done) {
				if (timer < 0.0f) {
					if (finalText[index] == '\n') {
						showSquare = false;
						pauseTimer = pauseDelay;
						timer = flickDelay;
					}
					if (!sound.isPlaying) {
						sound.Play();
					}
					currentText += finalText[index];
					text.text = currentText + '■' + "<color=#000000>" + finalText.Substring(index + 1) +  "</color>";
					timer = typeDelay;
					
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
						timer = flickDelay;
					}
					else {
						text.text = currentText + '■';
						showSquare = true;
						timer = flickDelay;
					}
				}
			}
		}
	}

	public void Show() {
		show = true;
		pauseTimer = pauseDelay;
	}

	public void ShowFull() {
		done = true;
		pauseTimer = -1.0f;
		timer = -1.0f;
		currentText = string.Copy(finalText);
		text.text = string.Copy(finalText);
	}
}
