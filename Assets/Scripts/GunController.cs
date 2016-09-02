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
	bool isHeld = false;
	Vector3 velocity;
	Vector3 direction = Vector3.right;
	private bool upButton, leftButton, downButton, rightButton;
	string fireButton;

	public void Setup (GameObject owner, string fireButton, Color color) {
		this.owner = owner;
		this.fireButton = fireButton;
		GetComponent<SpriteRenderer>().color = color;
		isHeld = true;
		cameraController = Camera.main.GetComponent<CameraController>();
		playerController = owner.GetComponent<PlayerController>();
		playerController.destroyEvent += Destroy;
		playerController.hitEvent += Drop;
		playerController.respawnEvent += PickUp;

		muzzleFlash = Instantiate(muzzleFlash, transform.position + new Vector3 (0.26f,0.0333f,0), Quaternion.identity) as GameObject;
		muzzleFlash.transform.parent = transform;
		muzzleFlash.SetActive(false);
	}
	
	public void Update () {
		if (Input.GetButtonDown(fireButton) && isHeld) Shoot ();

		if (isHeld) {
			direction = GetDirection();
			transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0,0,Mathf.Atan(direction.y / direction.x) * Mathf.Rad2Deg)), 0.5f);
			GetComponent<SpriteRenderer>().flipX = direction.x == -1;
			Vector3 destination = new Vector3(owner.transform.position.x + playerController.m_lastDirection * 0.25f,owner.transform.position.y, 0);
			transform.position = Vector3.Lerp(transform.position, destination, 0.75f);
		} else {
			velocity += new Vector3(0,-0.015f,0);
			transform.position += velocity;
		}

		if (ammo != maxAmmo && isHeld && cameraController.IsOnScreen(owner.transform.position)) Reload();
	}

	void Drop() {
		velocity = Vector3.one*0.25f;
		isHeld = false;
	}

	void PickUp () {
		isHeld = true;
	}

	public void Reload() {
		ammo = maxAmmo;
	}

	public void Shoot() {
		if (ammo > 0) {
			GameObject newBullet = Instantiate(bullet, owner.transform.position + direction * 0.5f, Quaternion.identity) as GameObject;
			//fire in that direction + players velocity
			newBullet.GetComponent<BulletController>().velocity = direction * shootForce + playerController.m_currentVelocity;
			newBullet.GetComponent<BulletController>().hitPlayerEvent += owner.GetComponent<PlayerController>().OnHitOtherPlayer;
			StartCoroutine("ShootFx");
			ammo--;
		} else {
			PlaySound(3); //click
		}
	}

	public IEnumerator ShootFx () {
		// RECOIL
		if (playerController.m_onGround && direction.y > 0) { //shooting on the ground
			playerController.AddForce (new Vector3(-direction.x * recoilForce + Random.Range(-recoilForce, recoilForce), -direction.y * recoilForce, 0));
		} else {
			playerController.AddForce (-direction * recoilForce);
		}
		//screenshake
		cameraController.StartScreenShake(0.1f,2);
		//make sound
		PlaySound(1); //TODO move to gune
		//muzzle flash on
		muzzleFlash.SetActive(true);
		//recoil out
		Vector3 recoilDistance = -direction * recoilForce * 0.5f;
		transform.position += recoilDistance;
		yield return null; yield return null;
		//muzzle flash off
		muzzleFlash.SetActive(false);
		yield return null; yield return null;
		//recoil back
		transform.position -= recoilDistance;
		yield return null;
	}

	Vector3 GetDirection () {
		string xAxis = "playerController.m_horizontalAxis";
		string yAxis = "playerController.m_verticalAxis";

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
		return direction;
	}

	void PlaySound(int n) {
		owner.GetComponent<PlayerController>().PlaySound(n);
	}

	void Destroy () {
		Destroy(gameObject);
	}
}
