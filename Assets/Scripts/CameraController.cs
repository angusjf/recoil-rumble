using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Camera camera;
	GameManagerScript gameManagerScript;
	Vector3 shake;

	void Start () {
		camera = GetComponent<Camera>();
		gameManagerScript = GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ();
	}

	void Update () {

		if (GameManagerScript.gameRunning) {
			LookAt(Vector3.Lerp (
				transform.position,
				GetAveragePosition(gameManagerScript.GetPlayer(1).transform.position, gameManagerScript.GetPlayer(2).transform.position) + shake,
				0.4f
			));
		}

		Debug.DrawLine(new Vector3(GetCameraBounds().extents.x,GetCameraBounds().extents.y,0), new Vector3(-GetCameraBounds().extents.x,GetCameraBounds().extents.y,0), Color.green);
		Debug.DrawLine(new Vector3(-GetCameraBounds().extents.x,GetCameraBounds().extents.y,0), new Vector3(-GetCameraBounds().extents.x,-GetCameraBounds().extents.y,0), Color.green);
		Debug.DrawLine(new Vector3(-GetCameraBounds().extents.x,-GetCameraBounds().extents.y,0), new Vector3(GetCameraBounds().extents.x,-GetCameraBounds().extents.y,0), Color.green);
		Debug.DrawLine(new Vector3(GetCameraBounds().extents.x,-GetCameraBounds().extents.y,0), new Vector3(GetCameraBounds().extents.x,GetCameraBounds().extents.y,0), Color.green);
	}

	void LookAt(Vector3 pos) {
		pos.z = transform.position.z;
		transform.position = pos;
	}

	Vector3 GetAveragePosition(params Vector3[] positions) {
		for (int i = 0; i < positions.Length; i++) {
			positions[i] = ClampToScreen(positions[i]);
		}
		Vector3 sumPositions = Vector3.zero;
		for (int i = 0; i < positions.Length; i++) {
			sumPositions += positions[i];
		}
		return sumPositions /= positions.Length;
	}

	public bool IsOnScreen(Vector3 pos) {
		return Mathf.Abs(pos.x) > GetCameraBounds().extents.x || Mathf.Abs(pos.y) > GetCameraBounds().extents.y;
	}

	Vector3 ClampToScreen(Vector3 pos) {
		if (Mathf.Abs(pos.x) > GetCameraBounds().extents.x) {
			pos.x = GetCameraBounds().extents.x * Mathf.Sign(pos.x);
		}
		if (Mathf.Abs(pos.y) > GetCameraBounds().extents.y) {
			pos.y = GetCameraBounds().extents.y * Mathf.Sign(pos.y);
		}
		return pos;
	}

	public void StartScreenShake (float a, int f) {
		StartCoroutine(ShakeScreen(a, f));
	}

	public Bounds GetCameraBounds() {
		return new Bounds(transform.position, new Vector3(camera.orthographicSize * camera.aspect * 2, camera.orthographicSize * 2, 0));
	}

	IEnumerator ShakeScreen (float a, int f) {
		for (int i = 0; i < f; i ++) {
			shake = new Vector3(Random.insideUnitSphere.x * a,Random.insideUnitSphere.y * a, 0);
			yield return null;
		}
		shake = Vector3.zero;
	}
}
