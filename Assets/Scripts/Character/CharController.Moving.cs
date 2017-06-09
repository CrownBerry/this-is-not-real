using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharController: MonoBehaviour {

	void Move(float direction) {
		if (myVelocity != 0 && direction == 0) {
			if (myVelocity > 0) {
				if (myVelocity < 1)
					rb.velocity = new Vector3(0f,rb.velocity.y,0f);
				else
					rb.AddForce (new Vector3 (-speed * 5f, 0f, 0f));
			} else {
				if (myVelocity > -1)
					rb.velocity = new Vector3(0f,rb.velocity.y,0f);
				else
					rb.AddForce (new Vector3 (speed * 5f, 0f, 0f));
			}
		}
		if ((myVelocity > maxSpeed && direction > 0) || (myVelocity < -maxSpeed && direction < 0)) {
			if (direction > 0)
				rb.velocity = new Vector3 (maxSpeed, rb.velocity.y, 0f);
			else
				rb.velocity = new Vector3 (-maxSpeed, rb.velocity.y, 0f);
			return;
		}
		rb.AddForce (new Vector3 (direction * speed * 12, 0f, 0f));
	}

	void Jump() {
		//rb.AddForce (new Vector3 (0f, jumpSpeed * 20, 0f), ForceMode.Acceleration);
		rb.velocity = new Vector3 (rb.velocity.x, jumpSpeed, 0);
	}

	void RealGravity(bool checking) {
		//if (checking)
		rb.AddForce (Physics.gravity * 2, ForceMode.Acceleration);
	}

	void RotateGirl(float direction) {
		if (direction < 0 && isRight) {
			transform.Rotate(0,180,0);
			isRight = !isRight;
		} else if (direction > 0 && !isRight) {
			transform.Rotate(0,180,0);
			isRight = !isRight;
		}

	}

}
