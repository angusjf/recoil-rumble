using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public GameObject bullet;
	GameObject gameManager;
	GameObject muzzleFlash;
	public GameObject gun;
	public GameObject owner;

	int ammo = 3;
	float maxInaccuracy = 0f;
	float recoilForce = 0.3f;

	Vector3 shootVector;

	public enum EDir {
		N, NE, E, SE, S, SW, W, NW
	};

	public EDir shootDir;

	ScreenShake screenShake;

	void Start () {
		gameManager = GameObject.Find("Game Manager");
		screenShake = GameObject.FindWithTag("MainCamera").GetComponent<ScreenShake>();
	}
	
	void Update () {

		if (owner.GetComponent<PlayerController>().m_onGround) {
			Reload();
		}

		if (Input.GetButtonDown(owner.GetComponent<PlayerController>().m_fireButton) && ammo > 0 && owner.GetComponent<PlayerController>().m_hasControl) {
			Shoot ();
		}
		
		SetShootDir();
		SetRotation();
	}


	void Shoot() {
		//make a bullet
		GameObject newBullet = Instantiate(bullet,transform.position,Quaternion.identity) as GameObject;
		//fire in that direction
		newBullet.GetComponent<BulletController>().direction = shootVector;

		newBullet.GetComponent<BulletController>().direction.
		//set who it belongs to
		newBullet.GetComponent<BulletController>().owner = owner;

		//take away ammo
		ammo --;
		//effects
		StartCoroutine("ShootFx");
	}

	IEnumerator ShootFx () {
		//recoil
		gameObject.GetComponent<PlayerController>().AddForce (-shootVector * recoilForce);
		//screenshake
		screenShake.StartScreenShake(0.1f,2);
		//make sound
		gameObject.GetComponent<PlayerController>().PlaySound(2);
		//muzzle flash TODO
		yield return null;
	}

	public void Reload () {
		ammo = 300000;//TODO
	}

	void SetShootDir() {

		string xAxis = owner.GetComponent<PlayerController>().m_horizontalAxis;
		string yAxis = owner.GetComponent<PlayerController>().m_verticalAxis;

		bool upButton = Input.GetAxisRaw(yAxis) > 0,
			leftButton = Input.GetAxisRaw(xAxis) < 0,
			downButton = Input.GetAxisRaw(yAxis) < 0,
			rightButton = Input.GetAxisRaw(xAxis) > 0;

		if (upButton)
			shootDir = EDir.N;
		if (downButton)
			shootDir = EDir.S;
		if (leftButton)
			shootDir = EDir.W;
		if (rightButton)
			shootDir = EDir.E;

		if (upButton && leftButton)
			shootDir = EDir.NW;
		if (upButton && rightButton)
			shootDir = EDir.NE;
		if (downButton && leftButton)
			shootDir = EDir.SW;
		if (downButton && rightButton)
			shootDir = EDir.SE;

		switch (shootDir) {
			case EDir.N:
				shootVector = Vector3.up;
				break;
			case EDir.NE:
				shootVector = new Vector3(1,1,0);
				break;
			case EDir.E:
				shootVector = Vector3.right;
				break;
			case EDir.SE:
				shootVector = new Vector3(1,-1,0);
				break;
			case EDir.S:
				shootVector = Vector3.down;
				break;
			case EDir.SW:
				shootVector = new Vector3(-1,-1,0);
				break;
			case EDir.W:
				shootVector = Vector3.left;
				break;
			case EDir.NW:
				shootVector = new Vector3(-1,1,0);
				break;
			default:
				shootDir = EDir.N;
				SetRotation();
				break;
		}
	}

	void SetRotation() {
		Vector3 angle = new Vector3(0,0,0);

		switch (shootDir) {
			case EDir.N:
				angle.z = 90;
				break;
			case EDir.NE:
				angle.z = 45;
				break;
			case EDir.E:
				angle.z = 0;
				break;
			case EDir.SE:
				angle.z = 315;
				break;
			case EDir.S:
				angle.z = 270;
				break;
			case EDir.SW:
				angle.z = 225;
				break;
			case EDir.W:
				angle.z = 180;
				break;
			case EDir.NW:
				angle.z = 135;	
				break;
			default:
				shootDir = EDir.N;
				SetRotation();
				break;
		}

		gun.transform.localRotation = Quaternion.Euler(angle);

		gun.GetComponent<SpriteRenderer>().flipY = (angle.z < 270 && angle.z > 90);

		gun.transform.localPosition = new Vector3 (Mathf.Abs(gun.transform.localPosition.x) * owner.GetComponent<PlayerController>().m_lastDirection, gun.transform.localPosition.y, gun.transform.localPosition.z);

	}

}
