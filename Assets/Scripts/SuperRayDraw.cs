using UnityEngine;
using System.Collections;

public class SuperRayDraw : MonoBehaviour {

	Vector3 lastPos;

	void Start () {
		lastPos = transform.position;
		InvokeRepeating("DrawRay", 0.03f,0.03f);
	}
	
	void DrawRay () {
		Debug.DrawLine(lastPos, transform.position, Color.red,1f);
		lastPos = transform.position;
	}
}
