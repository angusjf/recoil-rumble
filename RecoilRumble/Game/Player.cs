using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RecoilRumble.Game
{
	public class Player : PhysicsGameObject
	{
		private Keys xNegativeButton, xPositiveButton, jumpButton, shootButton;
		private AnimationController animationController;

		public int score = 0;
		public bool hasControl, isAlive;
		private const float moveForce = 0.02f;

		public Player (Vector2 pos, Keys xNegativeButton, Keys xPositiveButton, Keys jumpButton, Keys shootButton, PlayerSpriteSet spriteSet) : base (pos, spriteSet.idleTexture)
		{
			this.xNegativeButton = xNegativeButton;
			this.xPositiveButton = xPositiveButton;
			this.jumpButton = jumpButton;
			this.shootButton = shootButton;
			this.animationController = new AnimationController(sprite, spriteSet);
		}

		public override void Update ()
		{
			if (Engine.Instance.InputManager.GetKey(xPositiveButton)) {
				AddAcceleration (Vector2.UnitX * moveForce);
			}

			if (Engine.Instance.InputManager.GetKey (xNegativeButton)) {
				AddAcceleration (-Vector2.UnitX * moveForce);
			}

			if (Engine.Instance.InputManager.GetKey (jumpButton) && IsOnGround ()) {
				AddVelocity (Vector2.UnitY * -0.25f);
			}
			
			if (Engine.Instance.InputManager.GetKey (xNegativeButton)) {
				animationController.FacingLeft = true;
			} else if (Engine.Instance.InputManager.GetKey (xPositiveButton)) {
				animationController.FacingLeft = false;
			}

			base.Update ();
		}

		/*	public AudioClip dieSound; // land, shoot, explosion
			public GameObject gunPrefab;

			public Action destroyEvent;
			public Action hitEvent;
			public Action getPointEvent;
			public Action respawnEvent;
	*/

		/*
		public void Setup (string tag, string horizontalAxis, string verticalAxis, string jumpButton, string fireButton, Sprite [] sprites, Color playerColor)
		{
			//Componant / GameObject references
			movement = GetComponent<Custom2dPhysics> ();
			movement.canMove = true;
			cameraController = Camera.main.GetComponent<CameraController> ();
			audioSource = GetComponent<AudioSource> ();
			particleSystem = GetComponent<ParticleSystem> ();
			destroyEvent += Destroy;
			//setup
			this.tag = tag;
			this.horizontalAxis = horizontalAxis;
			this.verticalAxis = verticalAxis;
			this.jumpButton = jumpButton;
			this.sprites = sprites;
			this.playerColor = playerColor;
			particleSystem.startColor = playerColor;
			(Instantiate (gunPrefab) as GameObject).GetComponent<GunController> ().Setup (gameObject, fireButton, playerColor);//Gun Setup 
			score = 0;
			Respawn ();
		}
		*/

		/*
		public void Respawn ()
		{
			// TODO SetSprite(0);
			hasControl = true;
			isAlive = true;
			canMove = true;
			movement.Stop ();
			movement.SetPosition (GameObject.FindWithTag ("GameController").GetComponent<GameManagerScript> ().GetNextRespawnPos ());
			if (respawnEvent != null) respawnEvent ();
			//TODO maybe respawn effects?
		}

		public void Hit ()
		{
			if (hitEvent != null) hitEvent ();
			StartCoroutine ("HitEffects");
		}

		IEnumerator HitEffects ()
		{
			hasControl = false; // no control
			isAlive = false;
			// TODO SetSprite(1);

			yield return null; // wait 1 - move a bit

			canMove = false; // freeze
			cameraController.StartScreenShake (1.4f, 3); // shake

			yield return new WaitForSeconds (0.12f); // wait a bit

			canMove = true; // unfreeze
			particleSystem.Play (); // pretty

			yield return new WaitForSeconds (0.33f);

			// TODO SetSprite(2);
			Invoke ("Respawn", 1f);
		}

		public void OnHitOtherPlayer ()
		{
			if (getPointEvent != null) getPointEvent ();
			score++;
			PlaySound (dieSound);
		}

		public void PlaySound (AudioClip clip)
		{
			audioSource.PlayOneShot (clip);
		}

		public void Destroy ()
		{
			Destroy (gameObject);
		}
		*/
	}
}
