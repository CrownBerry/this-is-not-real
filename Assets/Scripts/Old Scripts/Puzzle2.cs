using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2 : MonoBehaviour {

	public GameObject door;
	bool isFinish;
	Vector3 openPos;

	IEnumerator coroutine;

	// Use this for initialization
	void Awake () {
		openPos = door.transform.position;
		openPos.y -= 7.0f;
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.CompareTag ("Player") && Input.GetButtonDown ("Use")) {
			if (!GameManager.instance.keyPuzzle2)
				return;
			isFinish = false;
			coroutine = MovingDoor (openPos);
			StartCoroutine (coroutine);
		}
	}

	IEnumerator MovingDoor (Vector3 target) {
		while (!isFinish) {
			door.transform.position = Vector3.MoveTowards (door.transform.position, target, Time.deltaTime * 2.5f);
			if (door.transform.position != target)
				yield return null;
			else
				isFinish = true;
		}
	}

}
