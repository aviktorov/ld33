using UnityEngine;

public class CameraLogic : MonoBehaviour {
	
	public float smoothness = 4.0f;
	
	private Animation cachedAnimation;
	private bool paused;
	private float targetSpeed;
	
	private void Awake() {
		cachedAnimation = GetComponent<Animation>();
		paused = false;
		targetSpeed = 1.0f;
	}
	
	private void Update() {
		foreach (AnimationState state in cachedAnimation) {
			state.speed = Mathf.Lerp(state.speed, targetSpeed, smoothness * Time.deltaTime);
		}
		
		if (!paused) return;
		
		if (Input.GetKeyDown(KeyCode.W)) Next();
		if (Input.GetKeyDown(KeyCode.S)) Prev();
	}
	
	private void Next() {
		paused = false;
		targetSpeed = 1.0f;
	}
	
	private void Prev() {
		paused = false;
		targetSpeed = -1.0f;
	}
	
	public void Pause () {
		paused = true;
		targetSpeed = 0.0f;
	}
}
