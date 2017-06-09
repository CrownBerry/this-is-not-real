using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharController : MonoBehaviour {

	void CheckBox() {
		RaycastHit hit;
		bool hitted;
		if (isRight)
			hitted = Physics.Linecast (new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z), new Vector3 (transform.position.x + 1.0f, transform.position.y + 1.5f, transform.position.z), out hit);
		else
			hitted = Physics.Linecast (new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z), new Vector3 (transform.position.x - 1.0f, transform.position.y + 1.5f, transform.position.z), out hit);	
		if (hitted) {
			if (hit.collider.CompareTag ("Box") && Input.GetButtonDown("Use"))
				GrabBox (hit.collider.gameObject);
		}
	}

	void CheckPoint() {
		Vector3 Start;
		Vector3 Goal;
		Vector3 Point;
		Vector3 Direction;
		RaycastHit hit;

		Start = transform.position;
		Start.y += 0.1f;
		Goal = Start;
		if (GameManager.instance.inLab)
			Goal.y -= 100.0f;
		else
			Goal.y += 100.0f;
		Direction = Goal - Start;
		Direction.Normalize ();
		Point = Start;
		Iter = 0;

		while (Point != Goal) {
			if (Physics.Linecast (Point, Goal, out hit)) {
				Iter++;
				Point = hit.point + (Direction / 100.0f);
			} else
				Point = Goal;
		}
		while (Point != Start) {
			if (Physics.Linecast (Point, Start, out hit)) {
				Iter++;
				Point = hit.point + (-Direction / 100.0f);
			} else
				Point = Start;
		}
		if (Iter % 2 == 0)
			isInside = true;
		else
			isInside = false;
	}

	public Vector3 FindPoint() {
		Vector3 point; 
		Vector3 goal;
		Vector3 result;
		float shift;
		float dir;
		bool stop;
		RaycastHit hit;

		shift = 1.0f;
		dir = 1;
		stop = false;
		result = new Vector3 (0, 0, 0);
		point = new Vector3 (transform.position.x,transform.position.y - 100.0f, transform.position.z);
		goal = point;
		goal.x += shift;

		while (!stop) {
			if (Physics.Linecast (point, goal, out hit)) {
				result = hit.point;
				stop = true;
			} else {
				dir = -dir;
				shift++;
				point = goal;
				goal.x += shift * dir;
			}
		}
		if (dir > 0)
			result.x -= 1f;
		else
			result.x += 1f;
		return result;
	}

}
