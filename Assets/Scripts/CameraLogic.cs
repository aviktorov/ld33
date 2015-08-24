using UnityEngine;

public class CameraLogic : MonoBehaviour {
	
	private Animation cachedAnimation;
	private bool paused;
	
	private void Awake() {
		cachedAnimation = GetComponent<Animation>();
		paused = false;
	}
	
	private void Update() {
		if (!paused) return;
		if (Input.GetKeyDown(KeyCode.W)) Next();
		if (Input.GetKeyDown(KeyCode.S)) Prev();
	}
	
	private void Next() {
		foreach (AnimationState state in cachedAnimation) {
			state.speed = 1.0f;
		}
		paused = false;
	}
	
	private void Prev() {
		foreach (AnimationState state in cachedAnimation) {
			state.speed = -1.0f;
		}
		paused = false;
	}
	
	public void Pause () {
		foreach (AnimationState state in cachedAnimation) {
			state.speed = 0.0f;
		}
		paused = true;
	}
}
