using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	PlayerController pc;

	const string horizontalAxis = "Horizontal1", verticalAxis = "Fire1"; // controls
	const float moveForce = 0.05f;	//how much you can move on the x

	void Awake () {
		pc = GetComponent<PlayerController>();
	}
	
	void Update () {
		X();
		Y();
	}

	void X () {
		float x = Input.GetAxisRaw(horizontalAxis);

		if (x != 0) {
			pc.m_currentAcceleration.x = Mathf.Sin(x) * moveForce;
		}
	}

	void Y () {
		bool y = Input.GetButtonDown(verticalAxis);

		if (y && pc.m_onGround)
			pc.AddForce(Vector3.up * 0.4f);
	}
}
