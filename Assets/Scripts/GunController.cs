using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	CameraController cameraController;
	SpriteRenderer spriteRenderer;
	public GameObject bullet;
	public GameObject muzzleFlash;
	GameObject owner;
	PlayerController playerController;
	//stuff that can change
	public int maxAmmo = 3;
	public int ammo;
	float maxInaccuracy = 0.05f;
	float recoilForce = 0.3f;
	float shootForce = 20f;
	bool isHeld = true;
	Vector3 velocity = Vector3.zero;
	private bool upButton, leftButton, downButton, rightButton;

	public void Setup (GameObject owner, string fireButton, Color color) {
		this.owner = owner;
		this.fireButton = fireButton;
		GetComponent<SpriteRenderer>().color = color;
		cameraController = Camera.main.GetComponent<CameraController>();

		muzzleFlash = Instantiate(muzzleFlash, transform.position + new Vector3 (0.26f,0.0333f,0), Quaternion.identity) as GameObject;
		muzzleFlash.transform.parent = transform;
		muzzleFlash.SetActive(false);
	}
	
	public void Update () {
		if (Input.GetButtonDown(fireButton) && isHeld) Shoot ();

		if (isHeld) {
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Vector3(GetDirection()), 0.5f);
			GetComponent<SpriteRenderer>().flipY = GetDirection().x < 1;
			Vector3 destination = new Vector3(owner.transform.position.x + playerController.m_lastDirection * 0.25f,owner.transform.position.y, 0);
			transform.position = Vector3.Lerp(transform.position, destination, 0.75f);
		} else {
			velocity += new Vector3(0,-0.015f,0);
			transform.position += velocity;
		}

		if (ammo != maxAmmo && cameraController.IsOnScreen(owner.transform.position)) Reload();
	}

	void Drop() {
		isHeld = false;
		velocity = playerController.m_currentVelocity;
	}

	public void Reload() {
		ammo = maxAmmo;
	}

	public void Shoot() {
		if (ammo > 0) {
			GameObject newBullet = Instantiate(bullet, owner.transform.position, Quaternion.identity) as GameObject;
			//fire in that direction + players velocity
			newBullet.GetComponent<BulletController>().velocity = shootVector * shootForce + playerController.m_currentVelocity;
			StartCoroutine("ShootFx");
			ammo--;
		} else {
			PlaySound(3); //click
		}
	}

	public IEnumerator ShootFx () {
		// RECOIL
		if (playerController.m_onGround && shootVector.y > 0) { //shooting on the ground
			playerController.AddForce (new Vector3(-shootVector.x * recoilForce + Random.Range(-recoilForce, recoilForce), -shootVector.y * recoilForce, 0));
		} else {
			playerController.AddForce (-shootVector * recoilForce);
		}

		//screenshake
		cameraController.StartScreenShake(0.1f,2);
		//make sound
		PlaySound(1); //TODO move to gune
		//muzzle flash on
		muzzleFlash.SetActive(true);
		//recoil out
		Vector3 recoilDistance = -shootVector * recoilForce * 0.5f;
		transform.position += recoilDistance;
		yield return null; yield return null;
		//muzzle flash off
		muzzleFlash.SetActive(false);
		yield return null; yield return null;
		//recoil back
		transform.position -= recoilDistance;
	}

	Vector3 GetDirection() {
		string xAxis = playerController.m_horizontalAxis;
		string yAxis = playerController.m_verticalAxis;

		upButton = Input.GetAxisRaw(yAxis) > 0;
		leftButton = Input.GetAxisRaw(xAxis) < 0;
		downButton = Input.GetAxisRaw(yAxis) < 0;
		rightButton = Input.GetAxisRaw(xAxis) > 0;

		if (upButton && leftButton) return new Vector3(-1,1,0);
		if (upButton && rightButton) return new Vector3(1,1,0);
		if (downButton && leftButton) return new Vector3(-1,-1,0);
		if (downButton && rightButton) return new Vector3(1,-1,0);
		if (upButton) return Vector3.up;
		if (downButton) return Vector3.down;
		if (leftButton) return Vector3.left;
		if (rightButton) return Vector3.right;
	}

	void PlaySound(int n) {
		
	}
}
