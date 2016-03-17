using UnityEngine;
using System.Collections;

public class TeleportButton : ButtonController {

	public Vector3 destination;

	public override void Push (GameObject pusher) {
//		base.Push(pusher);
		pusher.GetComponent<PlayerController>().TeleportTo(destination);
	}

}
