using UnityEngine;
using System.Collections;

public class SuperRayDraw : MonoBehaviour {

	Vector3 lastPos;

	void Start () {
		lastPos = transform.position;
//		InvokeRepeating("DrawRay", 0.1f,0.1f);
	}
	
	void DrawRay () {
		Debug.DrawLine(lastPos, transform.position, Color.red,1f);
		lastPos = transform.position;
	}
}
