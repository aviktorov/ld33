using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class InterstageFadeUI : MonoBehaviour {

	public AnimationCurve inTransition = AnimationCurve.EaseInOut(0f, 0f, 0.5f, 1f);
	public AnimationCurve outTransition = AnimationCurve.EaseInOut(0f, 1f, 0.5f, 0f);

	public void FadeToLevel(string level) {
		GetComponentInChildren<Image>().enabled = true;
		StartCoroutine(DoFade(level));
	}

	private IEnumerator DoFade(string level) {
		DontDestroyOnLoad(gameObject);
		yield return StartCoroutine(DoAnimate(inTransition));

		Application.LoadLevel(level);
		yield return StartCoroutine(DoAnimate(outTransition));

		Destroy(gameObject);
	}

	private IEnumerator DoAnimate(AnimationCurve curve) {
		var image = GetComponentInChildren<Image>();
		var duration = curve[curve.length - 1].time;

		for (var t = 0f; t <= duration; t += Mathf.Max(0.001f, Time.deltaTime)) {
			image.color = image.color.WithA(curve.Evaluate(t));
			yield return null;
		}
	}
}
