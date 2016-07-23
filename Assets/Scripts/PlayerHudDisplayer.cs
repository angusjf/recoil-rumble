using UnityEngine;
using System.Collections;

public class PlayerHudDisplayer : MonoBehaviour {
	#region variables
	PlayerController pc;
	GunController gc;
	GameObject cam;

	public GameObject scoreCounterPrefab;
	public GameObject ammoCounterPrefab;
	public GameObject arrowPrefab;
		
	GameObject[] scoreCounters;
	GameObject[] ammoCounters;
	GameObject arrow;

	public Sprite[] starSprites = new Sprite[2];
	public Sprite[] ammoSprites = new Sprite[2];
	#endregion

	#region MonoBehaviour
	void Start () {
		pc = GetComponent<PlayerController>();
		gc = pc.m_playerGun.GetComponent<GunController>();
		cam = GameObject.FindWithTag("MainCamera");

		//score	
		scoreCounters = new GameObject[GameManagerScript.winScore];
		for (int i = 0; i < scoreCounters.Length; i ++) {
			scoreCounters[i] = Instantiate(scoreCounterPrefab) as GameObject;
			scoreCounters[i].transform.position = transform.position + new Vector3(i * 0.21875f - (scoreCounters.Length - 1) * 0.21875f / 2, 0.37495f, 0);
			scoreCounters[i].transform.parent = transform;
			scoreCounters[i].GetComponent<SpriteRenderer>().enabled = false;
		}

		//ammo
		ammoCounters = new GameObject[gc.maxAmmo];
		for (int i = 0; i < ammoCounters.Length; i ++) {
			ammoCounters[i] = Instantiate(ammoCounterPrefab) as GameObject;
			ammoCounters[i].transform.position = transform.position + new Vector3(-0.35f, i * 0.21875f - 0.21875f * (ammoCounters.Length - 1f) / 2, 0);
			ammoCounters[i].transform.parent = transform;
			ammoCounters[i].GetComponent<SpriteRenderer>().enabled = false;
		}

		//arrow
		arrow = Instantiate(arrowPrefab) as GameObject;
		arrow.GetComponent<SpriteRenderer>().color = pc.m_playerColor;
	}
	
	void Update () {
		UpdateCounterVisiblity();
		UpdateAmmoCountVisiblity();
		UpdateArrowPosition();
	
		for (int i = 0; i < ammoCounters.Length; i++) {
			ammoCounters[i].transform.localPosition = new Vector3 (
				-Mathf.Abs(ammoCounters[i].transform.localPosition.x) * Mathf.Sign(pc.m_lastDirection),
				ammoCounters[i].transform.localPosition.y,
				0
			);
		}
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

	void UpdateAmmoCountVisiblity () {
		if (gc.ammo > ammoCounters.Length) return;

		//enable counters that need to be
		for (int i = 0; i < gc.ammo; i ++) {
			ammoCounters[i].GetComponent<SpriteRenderer>().sprite = ammoSprites[1];
		}
		//disable others
		for (int i = ammoCounters.Length - 1; i >= gc.ammo; i --) {
			ammoCounters[i].GetComponent<SpriteRenderer>().sprite = ammoSprites[0];
		}
	}

	void UpdateArrowPosition () {
		Vector2 cameraMaxPos = new Vector2 (
			cam.transform.position.x + cam.GetComponent<CameraController> ().bounds.x,
			cam.transform.position.y + cam.GetComponent<CameraController> ().bounds.y
		);

		Vector3 pos = transform.position;
		Vector3 rot = Vector3.zero;
			
		//isOnScreen = !(Mathf.Abs(pos.x) > Mathf.Abs(cameraMaxPos.x) + 1f || Mathf.Abs(pos.y) > Mathf.Abs(cameraMaxPos.y) + 1f);

		if (!OnScreen()) {
			// if off screen by one unit

			if (pos.x > cameraMaxPos.x) {
				pos.x = cameraMaxPos.x - 0.5f;
				rot.z = 0;
			}
			if (pos.x < cameraMaxPos.x - cam.GetComponent<CameraController> ().bounds.x * 2) {
				pos.x = cameraMaxPos.x + 0.5f - cam.GetComponent<CameraController> ().bounds.x * 2;
				rot.z = 180;
			}
			if (pos.y > cameraMaxPos.y)	{
				pos.y = cameraMaxPos.y - 0.5f;
				rot.z = 90;
			}
			if (pos.y < cameraMaxPos.y - cam.GetComponent<CameraController> ().bounds.y * 2) {
				pos.y = cameraMaxPos.y + 0.5f - cam.GetComponent<CameraController> ().bounds.y * 2;
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

	public IEnumerator SetAmmoVisiblity (bool visible, float timeVisible) {
		for (int i = 0; i < ammoCounters.Length; i ++) {
			ammoCounters[i].GetComponent<SpriteRenderer>().enabled = visible;
		}

		if (visible) {
			yield return new WaitForSeconds (timeVisible);
			StartCoroutine(SetAmmoVisiblity(false,0f));
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

	public void ShowAmmo () {
		StartCoroutine(SetAmmoVisiblity(true, 1f));
	}

	public void DestroyElements () {
		Destroy (arrow);
	}

	public bool OnScreen() {
		Vector2 cameraMaxPos = new Vector2 (
			cam.transform.position.x + cam.GetComponent<CameraController> ().bounds.x,
			cam.transform.position.y + cam.GetComponent<CameraController> ().bounds.y
		);

		Vector3 pos = transform.position;

		return !(Mathf.Abs(pos.x) > Mathf.Abs(cameraMaxPos.x) + 1f || Mathf.Abs(pos.y) > Mathf.Abs(cameraMaxPos.y) + 1f);
	}
	#endregion
}
