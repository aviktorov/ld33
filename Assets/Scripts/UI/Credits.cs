using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class Credits : MonoBehaviour {
	public float pauseDelay1 = 0.6f;
	public float pauseDelay2 = 0.6f;
	public float delay = 0.02f;
	
	public float flickTime = 0.5f;
	
	public string[] lines;

	private bool showSquare = true;
	private bool type = true;
	private bool done = false;

	private Text text;
	private string currentText = "";
	private int index = 0;
	private int linesIndex = 0;

	private float timer;
	private float pauseTimer;

	private void Start() {
		text = GetComponent<Text>();
		text.text = "";
		pauseTimer = pauseDelay1;
	}

	private void Update() {
		if (timer >= 0.0f)
			timer -= Time.deltaTime;

		if (pauseTimer >= 0.0f)
			pauseTimer -= Time.deltaTime;

		if (!done && pauseTimer > 0.0f) {
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

		if (!done && pauseTimer < 0.0f) {
			if (timer < 0.0f) {
				if (type) {
					currentText += lines[linesIndex][index];
					text.text = currentText + '■';
					timer = delay;

					index++;
					if (index == lines[linesIndex].Length) {
						linesIndex++;
						index = 0;
						type = false;
						pauseTimer = pauseDelay1;
						showSquare = false;
						timer = flickTime;
					}
				}
				else {
					currentText = currentText.Remove(currentText.Length - 1);
					text.text = currentText + '■';
					timer = delay;

					if (currentText.Length == 0) {
						type = true;
						pauseTimer = pauseDelay2;
						showSquare = false;
						timer = flickTime;
						if (linesIndex == lines.Length) {
							text.text = "";
							done = true;
						}
					}
				}
			}
		}
	}
}
