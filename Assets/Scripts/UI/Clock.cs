using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Clock : MonoBehaviour {
	public Text timerText;
	public bool isStop = false;

	private void Update () {
		if (!isStop) {
			int minutes = (int) Time.time / 60;
			int seconds = (int) Time.time % 60;
			timerText.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
		}
	}

	public string GetTime() {
		int minutes = (int) Time.time / 60;
		int seconds = (int) Time.time % 60;
		return (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
	}
}
