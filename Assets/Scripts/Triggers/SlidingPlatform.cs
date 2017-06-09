using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPlatform : MonoBehaviour {

	Vector3 min;
	Vector3 max;
	Vector3 moveDelta;
	Vector3 next;
	float speed;
	Rigidbody player;
	GameObject pad;

	void Awake () {
		speed = 4.0f;
		min = new Vector3 (transform.position.x - 20, transform.position.y, transform.position.z);
		max = new Vector3 (transform.position.x + 4, transform.position.y, transform.position.z);
		next = max;
		pad = transform.parent.gameObject;
	}

	void Update () {
		SetMovement ();
		SetDirection ();
	}

	void LateUpdate()
	{
		if (player) 
		{
			Vector3 playerBody = player.position;
			player.transform.position = new Vector3 (playerBody.x, playerBody.y) + moveDelta;
		}
	}

	void SetMovement() {
		Vector3 desiredPosition = Vector3.MoveTowards(pad.transform.localPosition, next, speed * Time.deltaTime);
		moveDelta = new Vector3 (desiredPosition.x, desiredPosition.y, 0f) - pad.transform.localPosition;
		pad.transform.localPosition = desiredPosition;
	}

	void SetDirection() {
		if (Vector2.Distance(pad.transform.localPosition, next) <= 0.1)
		{
			if (next == max)
			{
				next = min;
			}
			else
			{
				next = max;
			}
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("Player")) {
			player = col.gameObject.GetComponent<Rigidbody>();
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.CompareTag ("Player")) {
			player = null;
		}
	}
}
