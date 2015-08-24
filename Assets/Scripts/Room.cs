using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	public Transform[] cameras;

	public float speed = 50.0f;
	public float speedRotation = 10.0f;

	private Transform mainCamera;
	private int current = 0;
	private Vector3 targetPosition;
	private Quaternion targetRotation;

	private Vector3[] positions;
	private Quaternion[] rotations;

	private void Awake() {
		positions = new Vector3[cameras.Length];
		rotations = new Quaternion[cameras.Length];
		
		mainCamera = GameObject.FindWithTag("MainCamera").transform;

		for (int i = 0; i < cameras.Length; ++i) {
			positions[i] = cameras[i].position;
			rotations[i] = cameras[i].rotation;
			Destroy(cameras[i].gameObject);
		}

		targetPosition = positions[current];
		targetRotation = rotations[current];
		mainCamera.position = targetPosition;
		mainCamera.rotation = targetRotation;
	}

	private void Update () {
		if (Input.GetKeyDown(KeyCode.W)) {
			current = current + 1 < positions.Length ? current + 1 : current;
			targetPosition = positions[current];
			targetRotation = rotations[current];
		}

		if (Input.GetKeyDown(KeyCode.S)) {
			current = current > 0 ? current - 1 : current;
			targetPosition = positions[current];
			targetRotation = rotations[current];
		}
		
		if (Vector3.Distance(mainCamera.position, targetPosition) > 0.0000001f) {
			mainCamera.position = Vector3.MoveTowards(mainCamera.position, targetPosition, speed * Time.deltaTime);
		}

		if (Quaternion.Angle(mainCamera.rotation, targetRotation) > 0.0000001f) {
			mainCamera.rotation = Quaternion.RotateTowards(mainCamera.rotation, targetRotation, speedRotation * Time.deltaTime);
		}
	}
}
