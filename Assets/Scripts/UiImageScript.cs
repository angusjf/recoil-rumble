using UnityEngine;
using System.Collections;

public class UiImageScript : MonoBehaviour {

	public bool moving;

	void Start () {
		if (moving) StartCoroutine(Move());
	}

	IEnumerator Move () {
		float amount = 0.2f;
		float t = 0f;
		float dt = 0.08f;
		Vector3 initialPosition = this.transform.position;
		for (;;) {
			this.transform.position = initialPosition + new Vector3(Mathf.Cos(t) * amount, Mathf.Sin(t) * amount, 0);
			t += dt;
			yield return null;
		}
	}
}
