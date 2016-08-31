using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Camera cam;
	GameManagerScript gameManagerScript;
	Vector3 shake;
	float lerpAmount = 1f;
	float p = 0f;

	void Start () {
		cam = GetComponent<Camera>();
		gameManagerScript = GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ();
	}

	void Update () {
		if (GameManagerScript.gameRunning) {
			LookAt(Vector3.Lerp (transform.position, GetAveragePosition(gameManagerScript.GetPlayers()) * p + shake, lerpAmount));
		}
	}

	void LookAt(Vector3 pos) {
		pos.z = transform.position.z;
		transform.position = pos;
	}

	Vector3 GetAveragePosition(GameObject[] gameObjects) {
		Vector3[] positions = new Vector3[gameObjects.Length];
		for (int i = 0; i < positions.Length; i++) {
			positions[i] = ClampToScreen(gameObjects[i].transform.position);
		}
		Vector3 sumPositions = Vector3.zero;
		for (int i = 0; i < positions.Length; i++) {
			sumPositions += positions[i];
		}
		return sumPositions /= positions.Length;
	}

	public bool IsOnScreen(Vector3 pos) {
		return pos.x < GetCameraBounds().max.x && pos.y < GetCameraBounds().max.y && pos.x > GetCameraBounds().min.x && pos.y > GetCameraBounds().min.y;
	}

	Vector3 ClampToScreen(Vector3 pos) {
		float insetWidth = 3;
		if (pos.x > GetCameraBounds().max.x - insetWidth)
			pos.x = GetCameraBounds().max.x - insetWidth;
		if (pos.y > GetCameraBounds().max.y - insetWidth)
			pos.y = GetCameraBounds().max.y - insetWidth;
		if (pos.x < GetCameraBounds().min.x + insetWidth)
			pos.x = GetCameraBounds().min.x + insetWidth;
		if (pos.y < GetCameraBounds().min.y + insetWidth)
			pos.y = GetCameraBounds().min.y + insetWidth;
		return pos;
	}

	public Bounds GetCameraBounds() {
		return new Bounds(transform.position, new Vector3(cam.orthographicSize * cam.aspect * 2, cam.orthographicSize * 2, 0));
	}

	public void StartScreenShake (float a, int f) {
		StartCoroutine(ShakeScreen(a, f));
	}

	IEnumerator ShakeScreen (float a, int f) {
		for (int i = 0; i < f; i ++) {
			shake = new Vector3(Random.insideUnitSphere.x * a,Random.insideUnitSphere.y * a, 0);
			yield return null;
		}
		shake = Vector3.zero;
	}
}
