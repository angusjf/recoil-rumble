using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	
	Rigidbody2D rb;
	public Vector3 direction;
	float speed = 15f;
	float hitForce = 2f;
	public GameObject owner;

	public string solid;
	Vector3 v, a;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		v = direction * speed + new Vector3(0, owner.GetComponent<PlayerController> ().m_currentVelocity.y, 0);
		a = Vector3.zero;

		Destroy(gameObject,2f);

		float r = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg; 
		transform.rotation = Quaternion.Euler(new Vector3(0,0,r));;
	}
	
	void FixedUpdate () {
		rb.velocity = v;
		v += a;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == solid) {
			Destroy(gameObject, 0f); //delete bullet
			speed = 0f; // ?
		} else if (other.gameObject != owner && other.gameObject.tag.Contains("Player")) {
			// only do this if its not already dead
			if (other.gameObject.GetComponent<PlayerController>().m_isAlive) {
				// do damage / fx
				other.gameObject.GetComponent<PlayerController>().Hit();

				// increase score / fx
				owner.GetComponent<PlayerController>().m_score ++;
				owner.GetComponent<PlayerHudDisplayer>().StartCoroutine("BounceScore");
			}

			// bullet hit back
			if (other.GetComponent<PlayerController>().m_onGround && direction.y < 0) {
				direction.y = -direction.y;
			}
			other.GetComponent<PlayerController>().AddForce(direction * hitForce);

			// hit sound / Explosion? HACK - TODO move to bullet 
			owner.GetComponent<PlayerController>().PlaySound(2); // can cause error null ref

			//delete bullet
			Destroy(gameObject, 0f);
		}
	}
}
