using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	Camera mainCamera;
	Vector3 oldPos;
	float shakeAmount;
	int shakeFrames;


	void Start () {
		//for screenshake
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		oldPos = mainCamera.transform.position;
	}

	public void StartScreenShake (float a, int f) {
		shakeAmount = a;
		shakeFrames = f;
		//oldPos = mainCamera.transform.position;
		StartCoroutine("ShakeScreen");
	}

	void StopScreenShake () {
		mainCamera.transform.position = oldPos;
	}

	IEnumerator ShakeScreen () {
		for (int i = 0; i < shakeFrames; i ++) {
			mainCamera.transform.position = oldPos + new Vector3(Random.insideUnitSphere.x,Random.insideUnitSphere.y,0f) * shakeAmount;
			yield return null;
		}
		StopScreenShake();
	}
}