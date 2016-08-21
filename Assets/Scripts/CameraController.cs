using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameManagerScript gameManagerScript;
	Vector3 playerPosition;
	Vector3 player1Pos, player2Pos;
	
	Vector3 offset;
	public Vector2 bounds;

	Vector3 oldPos;
	float shakeAmount;
	int shakeFrames;

	public Shader shader;

	void Start () {
		offset = new Vector3(0,0,transform.position.z);
		gameManagerScript = GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ();
		playerPosition = transform.position;
		bounds.y = GetComponent<Camera> ().orthographicSize;
		bounds.x = bounds.y * GetComponent<Camera> ().aspect;
	}

	void Update () {
		if (GameManagerScript.gameRunning) {
			player1Pos = gameManagerScript.GetPlayer (1).transform.position;
			player2Pos = gameManagerScript.GetPlayer (2).transform.position;

			if (player1Pos.x > bounds.x)
				player1Pos.x = bounds.x;
			
			if (player1Pos.y > bounds.y)
				player1Pos.y = bounds.y;
			
			if (player1Pos.x < -bounds.x)
				player1Pos.x = -bounds.x;
			
			if (player1Pos.y < -bounds.y)
				player1Pos.y = -bounds.y;
			
			if (player2Pos.x > bounds.x)
				player2Pos.x = bounds.x;
			
			if (player2Pos.y > bounds.y)
				player2Pos.y = bounds.y;
			
			if (player2Pos.x < -bounds.x)
				player2Pos.x = -bounds.x;
			
			if (player2Pos.y < -bounds.y)
				player2Pos.y = -bounds.y;
			
			playerPosition = (player1Pos + player2Pos) / 6;
			playerPosition.z = 0;

			playerPosition = Vector3.Lerp (transform.position, playerPosition, 0.4f);

			transform.position = playerPosition + offset;
		}
	}

	public void ResetPosition() {
		transform.position = new Vector3(0,0,-10);
	}

	#region screenshake
	public void StartScreenShake (float a, int f) {
		shakeAmount = a;
		shakeFrames = f;
		StartCoroutine("ShakeScreen");
	}

	void StopScreenShake () {
		offset = new Vector3(0,0,transform.position.z);
	}

	IEnumerator ShakeScreen () {
		for (int i = 0; i < shakeFrames; i ++) {
			offset = new Vector3(Random.insideUnitSphere.x * shakeAmount,Random.insideUnitSphere.y * shakeAmount,transform.position.z);
			yield return null;
		}
		StopScreenShake();
	}
	#endregion
}
