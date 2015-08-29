using UnityEngine;

public class CameraLogic : MonoBehaviour {
	
	public float speed = 1.0f;
	public float smoothness = 4.0f;
	
	private Animation cachedAnimation;

	private int dir = 0;
	
	private void Awake() {
		cachedAnimation = GetComponent<Animation>();
		foreach (AnimationState state in cachedAnimation) {
			state.speed = 1.0f;
		}
	}

	private void Start() {
		foreach (AnimationState state in cachedAnimation) {
			state.speed = 1.0f;
		}
	}
	
	private void Update() {
		foreach (AnimationState state in cachedAnimation) {
			state.speed = Mathf.Lerp(state.speed, dir * speed, smoothness * Time.deltaTime);
		}
		
		if (Input.GetKey(KeyCode.W)) dir = 1;
		else if (Input.GetKey(KeyCode.S)) dir = -1;
		else dir = 0;
	}
}
