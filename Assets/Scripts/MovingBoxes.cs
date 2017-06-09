using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoxes : MonoBehaviour {

	public GameObject secondBox;
	Rigidbody rb;
	Vector3 oldPos;
	Vector3 newPos;
	Vector3 oldSecPos;
	float xMov;
	float yMov;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		oldPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		newPos = transform.position;
		xMov = oldPos.x - newPos.x;
		yMov = oldPos.y - newPos.y;
		if (rb.velocity.x != 0 || rb.velocity.y != 0) {
			oldSecPos = secondBox.transform.position;
			secondBox.GetComponent<Rigidbody> ().MovePosition (new Vector3 (oldSecPos.x - xMov, oldSecPos.y - yMov, oldSecPos.z));
		}
		oldPos = transform.position;
			
	}
}
