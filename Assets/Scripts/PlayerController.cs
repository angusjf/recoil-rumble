using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int m_playerNumber;
	private bool m_analogControls = false;

	public int m_lastDirection = 1;
	public int m_combo = 0;
	int m_numberOfRays = 5;
	public bool m_onGround = false;
	public bool m_hasControl = true;
	private bool m_canMove = true;

	//componants
	BoxCollider2D m_boxCollider;
	AudioSource m_audioSource;
	ParticleSystem m_particleSystem;
	SpriteRenderer m_spriteRenderer;
	GameObject mainCamera;
	public int m_solid = 256;
	public AudioClip[] m_soundEffects;

	public Vector3 m_startingPosition, m_respawnPosition; //position vectors

	Vector3 m_currentVelocity, m_terminalVelocity, m_currentAcceleration; //movement vectors
	float dragAmount;

	float moveForce = 0.05f; //how much you can move on the x
	float gravity = -0.015f;// -0.015f; // how much you accelerate down
	float friction = 0.5f; // ground resistance
	float airResistance = 0.2f; // air resistance;

	public string m_horizontalAxis, m_verticalAxis, m_fireButton; // controls

	void Awake () {
		mainCamera = GameObject.FindWithTag("MainCamera");
		m_boxCollider = GetComponent<BoxCollider2D>();
		m_audioSource = GetComponent<AudioSource>();
		m_particleSystem = GetComponent<ParticleSystem>();
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		m_currentVelocity = Vector3.zero;
		m_terminalVelocity = new Vector3(1000,-1000,0);
		m_currentAcceleration = Vector3.zero;
		tag = "Player " + m_playerNumber;
		m_horizontalAxis = "Horizontal" + m_playerNumber;
		m_verticalAxis = "Vertical" + m_playerNumber;
		m_fireButton = "Fire" + m_playerNumber;
		m_spriteRenderer.color = m_playerNumber == 1 ? new Color(0.97f,0.27f,0.30f) : new Color(0.40f,0.84f,0.31f);
		m_particleSystem.startColor = GetComponent<SpriteRenderer>().color;
		m_respawnPosition = m_playerNumber == 1 ? new Vector3 (-5,4,0) : new Vector3 (5.5f,4,0);
	}

	void Start () {
		transform.position = m_startingPosition;
	}
	
	void FixedUpdate () {
		//see if you are on the ground 
		m_onGround = IsOnGround();
		//which direction to fire / move in
		if (Mathf.Abs(Input.GetAxisRaw(m_horizontalAxis)) > 0) m_lastDirection = (int)Mathf.Sign(Input.GetAxisRaw(m_horizontalAxis));
		//move
		if (m_canMove) { Move(); }
		//set scale
		m_spriteRenderer.flipX = (m_lastDirection == -1);
	}

	void Move () {
		// controls => movement
		if (m_hasControl) {
			if (m_analogControls)
				m_currentAcceleration.x = Input.GetAxisRaw(m_horizontalAxis);
			else
				m_currentAcceleration.x = Input.GetAxisRaw(m_horizontalAxis) == 0 ? m_currentAcceleration.x = 0 : Input.GetAxisRaw(m_horizontalAxis) > 0 ? 1 : -1;
			m_currentAcceleration.x *= moveForce;
		} else {
			m_currentAcceleration.x = 0;
		}

		//drag on x
		if (m_onGround && m_hasControl) { // friction from 'feet in the ground'
			dragAmount = friction;
		} else {
			dragAmount = airResistance; // blasting away!
		}

		//apply drag on x TODO
		m_currentAcceleration.x += Mathf.Sign(m_currentVelocity.x) * -Mathf.Abs(m_currentVelocity.x) * dragAmount;
		
		//gravity
		if (!m_onGround) {
			m_currentAcceleration.y = gravity;
		} else {
			m_currentVelocity.y = 0;
			m_currentAcceleration.y = 0;
		}

		//apply drag on y
//		m_currentAcceleration.y += Mathf.Sign(m_currentVelocity.y) * -Mathf.Abs(m_currentVelocity.x);

		//turn maths to pictures
		SetPosition ();
	}

	public void AddForce (Vector3 direction) {
		m_currentVelocity += direction;
	}

	bool IsOnGround () {
		bool r = Mathf.Abs(DistanceToSolid(false)) < 0.01f && (bool)Physics2D.Raycast(transform.position,Vector3.down,m_boxCollider.size.y, m_solid); // TODO iffy
		if (r && !m_onGround) {
			PlaySound(1);
		}

		return r;
	}

	float DistanceToSolid (bool horizontal) {
		float[] distances = new float[m_numberOfRays];
		float a = 0;

		// declare rayPosY and set it to the bottom of the collider
		float rayPosX = -m_boxCollider.bounds.extents.x * 0.95f;
		float rayPosY = -m_boxCollider.bounds.extents.y * 0.95f;

		int currentRay = 0;
		Vector3 direction = Vector3.one;

		// DIRECTION
		if (horizontal) {
			if (m_currentVelocity.x > 0)
				direction = Vector3.right;
			else if (m_currentVelocity.x < 0)
				direction = Vector3.left;
			else
				return direction.x * 110000;
		} else {
			if (m_currentVelocity.y > 0)
				direction = Vector3.up;
			else if (m_currentVelocity.y <= 0)
				direction = Vector3.down;
			else
				return direction.y * 110000;
		}

		// OFFSET
		if (horizontal) {if (m_currentVelocity.x > 0) a = -m_boxCollider.size.x;}
		else {if (m_currentVelocity.y > 0) a = -m_boxCollider.size.y;}

		//loop though every ray then stop
		while (currentRay < m_numberOfRays) {
			RaycastHit2D r = horizontal ? Physics2D.Raycast(transform.position + new Vector3 (0,rayPosY,0), direction,100f,m_solid) : Physics2D.Raycast(transform.position + new Vector3 (rayPosX,0,0), direction,100f,m_solid);

			if (r.collider != null) {
				distances[currentRay] = horizontal ? direction.x * -r.collider.bounds.extents.x + r.collider.transform.position.x - transform.position.x + m_boxCollider.bounds.extents.x + a : direction.y * -r.collider.bounds.extents.y + r.collider.transform.position.y - transform.position.y + m_boxCollider.bounds.extents.y + a;
			} else {
				distances[currentRay] = horizontal ? direction.x * 140000 : direction.y * 140000;
			}

			currentRay ++;
			rayPosY += m_boxCollider.bounds.size.y / m_numberOfRays;
			rayPosX += m_boxCollider.bounds.size.x / m_numberOfRays;
		}

		if (horizontal) {
			float shortestDistance = 170000f * direction.x;

			for (int i = 0; i < m_numberOfRays - 1; i ++) {
				if (Mathf.Abs(distances[i]) < Mathf.Abs(shortestDistance))
					shortestDistance = distances[i];
			}

			return shortestDistance;
		}

		if (!horizontal) {
			float shortestDistance = 170000f * direction.y;

			for (int i = 0; i < m_numberOfRays - 1; i ++) {
				if (Mathf.Abs(distances[i]) < Mathf.Abs(shortestDistance))
					shortestDistance = distances[i];
			}

			return shortestDistance;
		}

		return horizontal? direction.x * 140000 : direction.y * 140000;
	}

	void SetPosition () {
		// v += a
		m_currentVelocity += m_currentAcceleration;
		// v = s [without collisions]
		Vector3 displacement = new Vector3();

		// x collisions
		if (Mathf.Abs(m_currentVelocity.x) > Mathf.Abs(DistanceToSolid(true))) {
			displacement.x = DistanceToSolid(true);
		} else {
			displacement.x = m_currentVelocity.x; //remove block
		}
		// y collisions
		if ((m_currentVelocity.y > 0 && m_currentVelocity.y > DistanceToSolid(false)) || (m_currentVelocity.y < 0 && m_currentVelocity.y < DistanceToSolid(false))) {
			displacement.y = DistanceToSolid(false);
			m_currentVelocity.y = 0; // TODO experimental
		} else {
			displacement.y = m_currentVelocity.y; //remove block
		}

		// move it
		transform.position += displacement;
	}

	public void Respawn () {
		m_hasControl = true;
		m_combo = 0;
		m_currentVelocity = Vector3.zero;
		TeleportTo(m_respawnPosition);

	}

	public void Hit () {
		StartCoroutine("HitEffects");
	}

	IEnumerator HitEffects () {
		m_hasControl = false; // no control
		yield return null; // wait 1 - move a bit
		yield return null; // wait 2 - move a bit
		m_canMove = false; // freeze
		mainCamera.GetComponent<ScreenShake>().StartScreenShake(0.5f,3); // shake
		yield return new WaitForSeconds(0.2f); // await a bit
		m_canMove = true; // unfreeze
		m_particleSystem.Play(); // pretty
		m_hasControl = false; // return control TODO
//		Respawn(); EXPERIEMTY DISABLED
	}

	public void PlaySound (int id) {
		m_audioSource.clip = m_soundEffects[id];
		m_audioSource.Play();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.GetComponent<ButtonController>() != null) {
			other.GetComponent<ButtonController>().Push(gameObject);
		}
	}

	public void TeleportTo(Vector3 pos) {
		transform.position = pos;
	}
}
