using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour {

	float speed = 3f;
	Vector3 startPos;
	Vector3 endPos;
	Vector3 next;
	GameObject cage;

	void Awake() {
		cage = transform.GetChild (0).gameObject;
		endPos = cage.transform.position;
		next = endPos;
		endPos.x += 3;
	}

	void Update() {
		Move ();
	}

	void Move() {
		Vector3 desiredPosition = Vector3.MoveTowards(cage.transform.position, next, speed * Time.deltaTime);
		cage.transform.position = desiredPosition;
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.CompareTag ("Player"))
			next = endPos;
	}

}
