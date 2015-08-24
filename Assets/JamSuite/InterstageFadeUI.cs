using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class InterstageFadeUI : MonoBehaviour {

	public AnimationCurve inTransition = AnimationCurve.EaseInOut(0f, 0f, 0.5f, 1f);
	public AnimationCurve outTransition = AnimationCurve.EaseInOut(0f, 1f, 0.5f, 0f);

	public void FadeToLevel(string level) {
		StartCoroutine(DoFadeOut(level));
	}

	private void Start() {
		GetComponent<Image>().enabled = true;
		StartCoroutine(DoFadeIn());
	}

	private IEnumerator DoFadeIn() {
		yield return StartCoroutine(DoAnimate(outTransition));

		gameObject.SetActive(false);
	}

	private IEnumerator DoFadeOut(string level) {
		yield return StartCoroutine(DoAnimate(inTransition));

		Application.LoadLevel(level);
	}

	private IEnumerator DoAnimate(AnimationCurve curve) {
		var image = GetComponent<Image>();
		var duration = curve[curve.length - 1].time;

		for (var t = 0f; t <= duration; t += Mathf.Max(0.001f, Time.deltaTime)) {
			image.color = image.color.WithA(curve.Evaluate(t));
			yield return null;
		}
	}
}
