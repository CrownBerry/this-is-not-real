using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharController : MonoBehaviour {
    private void CheckBox() {
		RaycastHit hit;
		bool hitted;
		if (isRight)
			hitted = Physics.Linecast (new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z), new Vector3 (transform.position.x + 1.0f, transform.position.y + 1.5f, transform.position.z), out hit);
		else
			hitted = Physics.Linecast (new Vector3 (transform.position.x, transform.position.y + 1.5f, transform.position.z), new Vector3 (transform.position.x - 1.0f, transform.position.y + 1.5f, transform.position.z), out hit);
	    if (!hitted) return;
	    if (hit.collider.CompareTag ("Box") && Input.GetButtonDown("Use"))
	        GrabBox (hit.collider.gameObject);
	}

    private void CheckPoint() {
	    RaycastHit hit;

		var start = transform.position;
		start.y += 0.1f;
		var goal = start;
		if (GameManager.instance.inLab)
			goal.y -= 100.0f;
		else
			goal.y += 100.0f;
		var direction = goal - start;
		direction.Normalize ();
		var point = start;
		iter = 0;

		while (point != goal) {
			if (Physics.Linecast (point, goal, out hit)) {
				iter++;
				point = hit.point + (direction / 100.0f);
			} else
				point = goal;
		}
		while (point != start) {
			if (Physics.Linecast (point, start, out hit)) {
				iter++;
				point = hit.point + (-direction / 100.0f);
			} else
				point = start;
		}
		isInside = iter % 2 == 0;
	}

	public Vector3 FindPoint() {
	    var shift = 1.0f;
		float dir = 1;
		var stop = false;
		var result = new Vector3 (0, 0, 0);
		var point = new Vector3 (transform.position.x,transform.position.y - 100.0f, transform.position.z);
		var goal = point;
		goal.x += shift;

		while (!stop)
		{
		    RaycastHit hit;
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
