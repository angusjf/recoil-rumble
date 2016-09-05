using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class Custom2dPhysics : MonoBehaviour {
	BoxCollider2D boxCollider;

	const int
		numberOfRays = 5,
		solid = 256;
	const float
		maxVelocity = 1.5f,
		gravity = -0.015f,
		friction = 0.5f,
		airResistance = 0.2f;
	private Vector3
		currentVelocity,
		currentAcceleration;
	public bool canMove;

	void Awake () {
		boxCollider = GetComponent<BoxCollider2D>();
	}

	void FixedUpdate () {
		if (canMove)
			Move();
	}

	private void Move () {
		float dragAmount = 0.01f;//IsOnGround() ? friction : airResistance;

		/*if (currentVelocity.x > 0) {
			if (currentAcceleration.x - dragAmount > 0) {
				currentAcceleration.x -= dragAmount;
			} else {

			}
		} else if (currentVelocity.x < 0) {
			if (currentAcceleration.x + dragAmount < 0) {
				currentAcceleration.x += dragAmount;
			}
		}*/

		currentVelocity.x *= 0.9f;
		currentAcceleration *= 0.0f;

		//gravity on y
		if (!IsOnGround())
			currentAcceleration.y = gravity;
		else {
			currentVelocity.y = 0;
			currentAcceleration.y = 0;
		}

		//not more than maxspeed
		if (Mathf.Abs(currentVelocity.x) > maxVelocity) currentVelocity.x = Mathf.Sign(currentVelocity.x) * maxVelocity;
		if (Mathf.Abs(currentVelocity.y) > maxVelocity) currentVelocity.y = Mathf.Sign(currentVelocity.y) * maxVelocity;

		currentVelocity += currentAcceleration;
		Vector3 displacement = Vector3.zero;
		// x collisions
		if (Mathf.Abs(currentVelocity.x) > Mathf.Abs(DistanceToSolid(true))) {
			displacement.x = DistanceToSolid(true);
			currentVelocity.x = 0;
			currentAcceleration.x = 0;
		} else {
			displacement.x = currentVelocity.x;
		}
		// y collisions
		if ((currentVelocity.y > 0 && currentVelocity.y > DistanceToSolid(false)) || (currentVelocity.y < 0 && currentVelocity.y < DistanceToSolid(false))) {
			displacement.y = DistanceToSolid(false);
			currentVelocity.y = 0;
			currentAcceleration.y = 0;
		} else {
			displacement.y = currentVelocity.y;
		}

		transform.position += displacement;
	}

	private float DistanceToSolid (bool horizontal) {
		/* takes in a bool (vertical / horizontal).
		 * and return a value of how far away the nearest solid is.
		 * positive = down; negative = up.
		 * returns wired numbers for errors (positive or negative).
		 */
		float[] distances = new float[numberOfRays];
		float a = 0;
		// declare rayPosY and set it to the bottom of the collider
		float rayPosX = (-boxCollider.bounds.extents.x /*EXPERIMENTAL*/ - boxCollider.offset.x) * 0.95f; //TODO use MAX not extents
		float rayPosY = -boxCollider.bounds.extents.y * 0.95f;
		int currentRay = 0;
		Vector3 direction = Vector3.one;

		// DIRECTION
		if (horizontal) {
			if (currentVelocity.x > 0)
				direction = Vector3.right;
			else if (currentVelocity.x < 0)
				direction = Vector3.left;
			else
				return direction.x * 110000;
		} else {
			if (currentVelocity.y > 0)
				direction = Vector3.up;
			else if (currentVelocity.y <= 0)
				direction = Vector3.down;
			else
				return direction.y * 110000;
		}

		// OFFSET
		if (horizontal) {if (currentVelocity.x > 0) a = -boxCollider.size.x;}
		else {if (currentVelocity.y > 0) a = -boxCollider.size.y;}

		//loop though every ray then stop
		while (currentRay < numberOfRays) {
			RaycastHit2D r = horizontal ? Physics2D.Raycast(transform.position + new Vector3 (0,rayPosY,0), direction,100f,solid) : Physics2D.Raycast(transform.position + new Vector3 (rayPosX,0,0), direction,100f,solid);

			if (r.collider != null && r.collider.gameObject != gameObject) {
				distances[currentRay] = horizontal ? direction.x * -r.collider.bounds.extents.x + r.collider.transform.position.x - transform.position.x + boxCollider.bounds.extents.x + a : direction.y * -r.collider.bounds.extents.y + r.collider.transform.position.y - transform.position.y + boxCollider.bounds.extents.y + a;
			} else {
				distances[currentRay] = horizontal ? direction.x * 140000 : direction.y * 140000;
			}

			currentRay ++;
			rayPosY += boxCollider.bounds.size.y / numberOfRays;
			rayPosX += boxCollider.bounds.size.x / numberOfRays;
		}

		if (horizontal) {
			float shortestDistance = 170000f * direction.x;

			for (int i = 0; i < numberOfRays - 1; i ++) {
				if (Mathf.Abs(distances[i]) < Mathf.Abs(shortestDistance)) {
					shortestDistance = distances[i];
				}
			}

			return shortestDistance;
		}

		if (!horizontal) {
			float shortestDistance = 170000f * direction.y;
			for (int i = 0; i < numberOfRays - 1; i ++) {
				if (Mathf.Abs(distances[i]) < Mathf.Abs(shortestDistance)) {
					shortestDistance = distances[i];
				}
			}
			return shortestDistance;
		}

		return horizontal? direction.x * 140000 : direction.y * 140000;
	}

	public bool IsOnGround () {
		return Mathf.Abs(DistanceToSolid(false)) < 0.01f;
	}

	public void SetPosition (Vector3 pos) {
		transform.position = pos;
	}

	public void AddVelocity (Vector3 vel) {
		currentVelocity += vel;
	}

	public Vector3 GetVelocity() {
		return currentVelocity;
	}

	public void AddAcceleration (Vector3 acc) {
		currentAcceleration += acc;
	}

	public void Stop() {
		currentVelocity = Vector3.zero;
		currentAcceleration = Vector3.zero;
	}
}
