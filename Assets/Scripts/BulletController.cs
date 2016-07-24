using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	
	Rigidbody2D rb;
	public Vector3 velocity;
	float hitForce = 0.07f;
	public GameObject owner;

	public string solid;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = velocity;

		//rotation
		Vector3 direction = Vector3.Normalize(velocity);
		float r = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg; 
		transform.rotation = Quaternion.Euler(new Vector3(0,0,r));;

		Destroy(gameObject,2f);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == solid) {				// HIT WALL 

			Destroy(gameObject, 0f); //delete bullet
			rb.velocity = Vector3.zero; // stop bullet
			// explode TODO

		} else if (other.gameObject != owner && other.gameObject.tag.Contains("Player")) {		//HIT PERSON
			// only do this if its not already dead
			if (other.gameObject.GetComponent<PlayerController>().m_isAlive) {
				// do damage / fx
				other.gameObject.GetComponent<PlayerController>().Hit();

				// increase score / fx
				owner.GetComponent<PlayerController>().m_score ++;
				owner.GetComponent<PlayerHudDisplayer>().StartCoroutine("BounceScore");
			}

			// bullet hit back
			if (other.GetComponent<PlayerController>().m_onGround && velocity.y < 0) {
				velocity.y = -velocity.y;
			}
			other.GetComponent<PlayerController>().AddForce(velocity * hitForce);

			// hit sound / Explosion? HACK - TODO move to bullet 
			owner.GetComponent<PlayerController>().PlaySound(2); // can cause error null ref

			//delete bullet
			Destroy(gameObject, 0f);
		}
	}
}
