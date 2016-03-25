using UnityEngine;
using System.Collections;

public class BlockController : ButtonController {

	public override void Push (GameObject pusher)
	{
//		Invoke ("Disintegrate", 1f);
		base.Push (pusher);
	}

	void Disintegrate () {
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		Invoke ("Reintegrate", 0.4f);
	}

	void Reintegrate () {
		GetComponent<BoxCollider2D>().enabled = true;
		GetComponent<SpriteRenderer>().enabled = true;
	}
}
