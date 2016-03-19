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
		v = direction * speed;
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
			other.gameObject.GetComponent<PlayerController>().Hit(); // do damage
			other.GetComponent<PlayerController>().AddForce(direction * hitForce); // bullet hit back
//			owner.GetComponent<PlayerController>().m_lives ++; // increase combo
			owner.GetComponent<PlayerController>().PlaySound(3); // hit sound? TODO move to bullet
			Destroy(gameObject, 0f); //delete bullet
		}
	}
}
