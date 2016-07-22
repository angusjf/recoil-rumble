using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameManagerScript gameManagerScript;
	Vector3 playerPosition;
	Vector3 player1Pos, player2Pos;
	
	Vector3 offset = Vector3.zero;
	public Vector2 bounds;

	Vector3 oldPos;
	float shakeAmount;
	int shakeFrames;

	public Shader shader;

	void Start () {
		gameManagerScript = GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ();
		playerPosition = transform.position;
		bounds.y = GetComponent<Camera> ().orthographicSize;
		bounds.x = bounds.y * GetComponent<Camera> ().aspect;

		//S H A D E R   S T U F F
		GetComponent<Camera>().RenderWithShader(shader, "Hidden/ReplaceCameraColors");
	}

	void Update () {
		if (GameManagerScript.gameStarted) {
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
			playerPosition.z = transform.position.z;

			playerPosition = Vector3.Lerp (transform.position, playerPosition, 0.4f);

			transform.position = playerPosition + offset;
		}
	}

	#region screenshake
	public void StartScreenShake (float a, int f) {
		shakeAmount = a;
		shakeFrames = f;
		StartCoroutine("ShakeScreen");
	}

	void StopScreenShake () {
		offset = Vector3.zero;
	}

	IEnumerator ShakeScreen () {
		for (int i = 0; i < shakeFrames; i ++) {
			offset = new Vector3(Random.insideUnitSphere.x,Random.insideUnitSphere.y,0f) * shakeAmount;
			yield return null;
		}
		StopScreenShake();
	}
	#endregion
}
