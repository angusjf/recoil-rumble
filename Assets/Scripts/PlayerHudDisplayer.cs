using UnityEngine;
using System.Collections;

public class PlayerHudDisplayer : MonoBehaviour {
	#region variables
	PlayerController pc;
	CameraController cam;

	public GameObject scoreCounterPrefab;
	public GameObject arrowPrefab;
		
	GameObject[] scoreCounters;
	GameObject arrow;

	public Sprite[] starSprites = new Sprite[2];
	#endregion

	#region MonoBehaviour
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
	}
	
	void Update () {
		UpdateCounterVisiblity();
		UpdateArrowPosition();
	}
	#endregion

	#region others
	void UpdateCounterVisiblity () {
		if (pc.m_score > scoreCounters.Length) return;

		//enable counters that need to be
		for (int i = 0; i < pc.m_score; i ++) {
			scoreCounters[i].GetComponent<SpriteRenderer>().sprite = starSprites[1];
		}
		//disable others
		for (int i = scoreCounters.Length - 1; i >= pc.m_score; i --) {
			scoreCounters[i].GetComponent<SpriteRenderer>().sprite = starSprites[0];
		}

	}

	void UpdateArrowPosition () {
		Vector3 pos = transform.position;
		Vector3 rot = Vector3.zero;
			
		if (!cam.IsOnScreen(transform.position)) {
			// if off screen by one unit

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

	public IEnumerator SetScoreVisibilty (bool visible, float timeVisible) {
		for (int i = 0; i < scoreCounters.Length; i ++) {
			scoreCounters[i].GetComponent<SpriteRenderer>().enabled = visible;
		}

		if (visible) {
			yield return new WaitForSeconds (timeVisible);
			StartCoroutine(SetScoreVisibilty(false,0f));
		}
	}

	public IEnumerator BounceScore () {
		StartCoroutine(SetScoreVisibilty(true, 1f));
		yield return new WaitForSeconds (0.2f);
		for (int i = 0; i < 10; i ++) {
			scoreCounters[pc.m_score - 1].transform.position += (i < 5 ? 0.05f : -0.05f) * Vector3.up;
			yield return null;
		}
	}

	public void DestroyElements () {
		Destroy (arrow);
	}
	#endregion
}
