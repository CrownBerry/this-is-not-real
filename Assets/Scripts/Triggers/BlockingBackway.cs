using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingBackway : MonoBehaviour {

	public GameObject block;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("Player")) {
			block.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}

}
