using UnityEngine;
using System.Collections;
using System;

public class BulletController : MonoBehaviour {
	
	public Vector3 velocity;
	public string solid;
	const float hitForce = 0.07f;
	bool used = false;
	public Action hitPlayerEvent;

	void Start () {
		GetComponent<Rigidbody2D>().velocity = velocity;
		transform.rotation = Quaternion.Euler(new Vector3(0,0,Mathf.Atan2(velocity.y,velocity.x) * Mathf.Rad2Deg + 90f));;
		Destroy(gameObject,2f);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (!used /* &&other.gameObject != owner*/ && other.gameObject.tag != "Bullet") {
			HitObject();
			if (other.gameObject.tag.Contains("Player")) {
				HitPlayer(other.gameObject);
			}
		}
	}

	void HitObject() {
		used = true;
		GetComponent<Rigidbody2D>().velocity = Vector3.zero; //stop bullet
		GetComponent<ParticleSystem>().Play();
		GetComponent<SpriteRenderer>().enabled = false;
		//TODO explode
	}

	void HitPlayer(GameObject player) {
		// only do this if its not already dead
		if (player.GetComponent<PlayerController>().m_isAlive) {
			if (hitPlayerEvent != null) hitPlayerEvent();
			player.gameObject.GetComponent<PlayerController>().Hit();
		}

		// bullet hit back
		if (player.GetComponent<PlayerController>().m_onGround && velocity.y < 0) {
			velocity.y = -velocity.y;
		}
		player.GetComponent<PlayerController>().AddForce(velocity * hitForce);

		// hit sound / Explosion? HACK - TODO move to bullet 
	}
}