using UnityEngine;
using System.Collections;

public class SpikeButton : ButtonController {

	public override void Push (GameObject pusher) {
		base.Push(pusher);
		pusher.GetComponent<PlayerController>().Hit();
	}

}
