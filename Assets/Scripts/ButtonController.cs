using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	public virtual void Push (GameObject pusher) {
		pusher.GetComponent<PlayerController>().Hit();
	}

}
