using UnityEngine;
using System.Collections;

public class PlayerHudDisplayer : MonoBehaviour {
	PlayerController pc;
	CameraController cam;

	public GameObject scoreCounterPrefab, arrowPrefab;
	public Sprite[] starSprites = new Sprite[2];
		
	GameObject[] scoreCounters;
	GameObject arrow;

	WaitForSeconds ShortWait = new WaitForSeconds(0.2f);
	WaitForSeconds LongWait = new WaitForSeconds(4);

	void Start () {
		pc = GetComponent<PlayerController>();
		cam = Camera.main.GetComponent<CameraController>();

		//score	
		scoreCounters = new GameObject[GameManagerScript.winScore];
		for (int i = 0; i < scoreCounters.Length; i ++) {
			scoreCounters[i] = Instantiate(scoreCounterPrefab) as GameObject;
			scoreCounters[i].transform.position = transform.position + new Vector3(i * 0.21875f - (scoreCounters.Length - 1) * 0.21875f / 2, 1.5f*0.37495f, 0);
			scoreCounters[i].transform.parent = transform;
			scoreCounters[i].GetComponent<SpriteRenderer>().enabled = false;
			scoreCounters[i].GetComponent<SpriteRenderer>().color = pc.m_playerColor;
		}

		//arrow
		arrow = Instantiate(arrowPrefab) as GameObject;
		arrow.GetComponent<SpriteRenderer>().color = pc.m_playerColor;
		pc.destroyEvent += DestroyElements;
		pc.getPointEvent += BounceScore;
	}
	
	void Update () {
		UpdateCounterVisiblity();
		UpdateArrowPosition();
	}

	void UpdateCounterVisiblity () {
		if (pc.score > scoreCounters.Length) return;

		//enable counters that need to be
		for (int i = 0; i < pc.score; i ++) {
			scoreCounters[i].GetComponent<SpriteRenderer>().sprite = starSprites[1];
		}
		//disable others
		for (int i = scoreCounters.Length - 1; i >= pc.score; i --) {
			scoreCounters[i].GetComponent<SpriteRenderer>().sprite = starSprites[0];
		}

	}

	void UpdateArrowPosition () {
		Vector3 pos = transform.position;
		Vector3 rot = Vector3.zero;
			
		if (!cam.IsOnScreen(transform.position)) {

			if (pos.x > cam.GetCameraBounds().max.x) {
				pos.x = cam.GetCameraBounds().max.x - 0.5f;
				rot.z = 0;
			}
			if (pos.y > cam.GetCameraBounds().max.y) {
				pos.y = cam.GetCameraBounds().max.y - 0.5f;
				rot.z = 90;
			}
			if (pos.x < cam.GetCameraBounds().min.x) {//- cam.GetComponent<CameraController> ().bounds.x * 2) {
				pos.x = cam.GetCameraBounds().min.x + 0.5f;// - cam.GetComponent<CameraController> ().bounds.x * 2;
				rot.z = 180;
			}
			if (pos.y < cam.GetCameraBounds().min.y) {
				pos.y = cam.GetCameraBounds().min.y + 0.5f;
				rot.z = 270;
			}

			arrow.GetComponent<SpriteRenderer>().enabled = true;
			arrow.transform.position = pos;
			arrow.transform.rotation = Quaternion.Euler(rot);

		} else {
			arrow.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	IEnumerator SetScoreVisibilty (bool visible) {
		for (int i = 0; i < scoreCounters.Length; i ++) {
			scoreCounters[i].GetComponent<SpriteRenderer>().enabled = visible;
		}

		if (visible) {
			yield return LongWait;
			StartCoroutine(SetScoreVisibilty(false));
		}
	}

	void BounceScore () {
		StartCoroutine("BounceScoreCoroutine");
	}

	IEnumerator BounceScoreCoroutine () {
		StartCoroutine(SetScoreVisibilty(true));
		yield return ShortWait;
		for (int i = 0; i < 10; i ++) {
			scoreCounters[pc.score - 1].transform.position += (i < 5 ? 0.05f : -0.05f) * Vector3.up;
			yield return null;
		}
	}

	public void DestroyElements () {
		Destroy (arrow);
	}
}
