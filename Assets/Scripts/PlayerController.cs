using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	#region varaibles - Componants
	CameraController cameraController;
	BoxCollider2D m_boxCollider;
	AudioSource m_audioSource;
	ParticleSystem m_particleSystem;
	SpriteRenderer m_spriteRenderer;
	#endregion

	#region varaibles - Assets
	public AudioClip[] m_soundEffects; // land, shoot, explosion
	public Sprite[] redSprites, blueSprites; //alive, dead, flying
	Sprite[] m_sprites; //alive, dead, flying
	public GameObject m_gunPrefab;
	#endregion

	#region varaibles - Current state (rules)
	public int m_lastDirection, m_score;
	public bool m_onGround, m_hasControl, m_isAlive, m_canMove;
	#endregion

	#region varaibles - Current state (physics)
	float m_dragAmount;
	float m_moveForce = 0.05f; //how much you can move on the x
	float m_gravity = -0.015f;// -0.015f; // how much you accelerate down
	float m_friction = 0.5f; // ground resistance
	float m_airResistance = 0.2f; // air resistance;
	public Vector3 m_currentVelocity, m_currentAcceleration; //movement vectors
	#endregion

	#region varaibles - constants
	const int m_numberOfRays = 5;
	const int m_solid = 256;
	const float maxVelocity = 0.5f;
	#endregion

	#region varaibles - Based on player number / settings
	public int m_playerNumber;
	public Color m_playerColor;
	private bool m_analogControls = false;
	public string m_horizontalAxis, m_verticalAxis, m_fireButton; // controls
	public GameObject m_playerGun;
	#endregion

	#region setup methods
	void Awake () {
		//Componant / GameObject references
		cameraController = Camera.main.GetComponent<CameraController>();
		m_boxCollider = GetComponent<BoxCollider2D>();
		m_audioSource = GetComponent<AudioSource>();
		m_particleSystem = GetComponent<ParticleSystem>();
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		FindObjectOfType<GameManagerScript>().startGameEvent += Spawn;
	}

	public void Spawn () {
		//state
		m_lastDirection = 1; m_score = 0; m_onGround = false; m_hasControl = true; m_isAlive = true; m_canMove = true;
		m_currentVelocity = Vector3.zero; m_currentAcceleration = Vector3.zero;
		//Player Setup (rules)
		tag = m_playerNumber == 1 ? "Player1" : "Player2";
		m_horizontalAxis = m_playerNumber == 1 ? "Horizontal1" : "Horizontal2";
		m_verticalAxis = m_playerNumber == 1 ? "Vertical1" : "Vertical2";
		m_fireButton = m_playerNumber == 1 ? "Fire1" : "Fire2";
		m_sprites = m_playerNumber == 1 ? redSprites : blueSprites;
		SetSprite(0);
		m_playerColor = m_playerNumber == 1 ? Color.red : Color.blue;
		m_particleSystem.startColor = m_playerColor;
		//Player Setup (physics)
		TeleportTo (GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ().GetNextRespawnPos ());
		//Gun Setup
		m_playerGun = Instantiate(m_gunPrefab) as GameObject;
		m_playerGun.GetComponent<GunController>().owner = gameObject;
	}
	#endregion

	#region MonoBehaviour methods
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
	#endregion

	#region Movement methods
	void Move () {
		// controls => movement
		if (m_hasControl) {
			if (m_analogControls)
				m_currentAcceleration.x = Input.GetAxisRaw(m_horizontalAxis);
			else
				m_currentAcceleration.x = Input.GetAxisRaw(m_horizontalAxis) == 0 ? m_currentAcceleration.x = 0 : Input.GetAxisRaw(m_horizontalAxis) > 0 ? 1 : -1;
			m_currentAcceleration.x *= m_moveForce;
		} else {
			m_currentAcceleration.x = 0;
		}

		//drag on x
		if (m_onGround) { // friction from 'feet in the ground'
			m_dragAmount = m_friction;
		} else { // in the sky
			m_dragAmount = m_airResistance;
		}

		//apply drag on x only if in control TODO?
		m_currentAcceleration.x = m_hasControl ? m_currentAcceleration.x + Mathf.Sign(m_currentVelocity.x) * -Mathf.Abs(m_currentVelocity.x) * m_dragAmount : 0; // TODO
		
		//gravity
		if (!m_onGround) {
			m_currentAcceleration.y = m_gravity;
		} else {
			m_currentVelocity.y = 0;
			m_currentAcceleration.y = 0;
		}

		//not more than maxspeed
		if (Mathf.Abs(m_currentVelocity.x) > maxVelocity) m_currentVelocity.x = Mathf.Sign(m_currentVelocity.x) * maxVelocity;
		if (Mathf.Abs(m_currentVelocity.y) > maxVelocity) m_currentVelocity.y = Mathf.Sign(m_currentVelocity.y) * maxVelocity;

		//turn maths to pictures
		SetPosition ();
	}

	public void AddForce (Vector3 direction) {
		m_currentVelocity += direction;
	}

	bool IsOnGround () {
		bool r = Mathf.Abs(DistanceToSolid(false)) < 0.01f && (bool)Physics2D.Raycast(transform.position,Vector3.down,m_boxCollider.size.y, m_solid); // HACK but works...
		if (r && !m_onGround) {
			PlaySound(0);
		}
		return r;
	}

	float DistanceToSolid (bool horizontal) {
		/* takes in a bool (vertical / horizontal).
		 * and return a value of how far away the nearest solid is.
		 * positive = down; negative = down.
		 * returns wired numbers for errors (positive or negative).
		 */
		float[] distances = new float[m_numberOfRays];
		float a = 0;
		// declare rayPosY and set it to the bottom of the collider
		float rayPosX = (-m_boxCollider.bounds.extents.x /*EXPERIMENTAL*/ - m_boxCollider.offset.x) * 0.95f; //TODO use MAX not extents
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
				if (Mathf.Abs(distances[i]) < Mathf.Abs(shortestDistance)) {
					shortestDistance = distances[i];
				}
			}

			return shortestDistance;
		}

		if (!horizontal) {
			float shortestDistance = 170000f * direction.y;
			for (int i = 0; i < m_numberOfRays - 1; i ++) {
				if (Mathf.Abs(distances[i]) < Mathf.Abs(shortestDistance)) {
					shortestDistance = distances[i];
				}
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
			m_currentVelocity.y = 0; // HACK experimental
		} else {
			displacement.y = m_currentVelocity.y; //remove block
		}

		// move it
		transform.position += displacement;
	}
	#endregion

	#region other
	public void Respawn () {
		if (GameManagerScript.gameRunning) {
			m_hasControl = true;
			m_isAlive = true;
			m_canMove = true;
			//if (m_score > 0) m_score --; TODO should this exist
			m_currentVelocity = Vector3.zero;
			TeleportTo (GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ().GetNextRespawnPos ());
			//TODO meybe respawn effects?
			SetSprite(0);
			//reload gun
			m_playerGun.GetComponent<GunController>().Reload();
		}
	}

	public void Hit () {
		StartCoroutine("HitEffects");
	}

	IEnumerator HitEffects () {
		m_hasControl = false; // no control
		m_isAlive = false;
		SetSprite(1);

		yield return null; // wait 1 - move a bit

		m_canMove = false; // freeze
		cameraController.StartScreenShake(1.4f,3); // shake

		yield return new WaitForSeconds(0.12f); // wait a bit

		m_canMove = true; // unfreeze
		//m_hasControl = true; // return control maybe? TODO
		m_particleSystem.Play(); // pretty

		yield return new WaitForSeconds(0.33f);

		SetSprite(2);
		Invoke("Respawn", 1f);
	}

	public void PlaySound (int id) {
		m_audioSource.PlayOneShot(m_soundEffects[id]);
	}

	public void SetSprite (int id) {
		m_spriteRenderer.sprite = m_sprites[id];
	}

	public void TeleportTo (Vector3 pos) {
		transform.position = pos;
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (!m_isAlive && other.gameObject.tag == "Solid") {
			m_currentVelocity.x = 0;
		}
	}

	public void SafeDestroy () {
		GetComponent<PlayerHudDisplayer>().DestroyElements();
		Destroy(m_playerGun);
		Destroy(gameObject);
	}
	#endregion
}
