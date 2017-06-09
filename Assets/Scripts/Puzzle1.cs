using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1 : MonoBehaviour {

	public GameObject door;
	public GameObject btn;
	Vector3 closePos;
	Vector3 openPos;
	Vector3 newPos;

	Vector3 btnPrs;
	Vector3 btnNotPrs;

	IEnumerator coroutine;

	bool isFinish;
	bool stepOne;
	bool Moving;

	void Awake() {
		isFinish = false;
		closePos = door.transform.position;
		openPos = closePos;
		openPos.y += 7.0f;
		btnNotPrs = btn.transform.position;
		btnPrs = btnNotPrs;
		btnPrs.z += 0.2f;
	}

	void Update() {
		if (Moving)
			btn.transform.position = btnPrs;
		else
			btn.transform.position = btnNotPrs;
		
	}

	void OnTriggerStay (Collider col) {
		if (col.gameObject.CompareTag ("Player") && Input.GetButtonDown ("Use")) {
			Debug.Log ("Button pressed");
			isFinish = false;
			stepOne = false;
			Moving = true;
			coroutine = MoveingDoor (openPos, closePos);
			StartCoroutine (coroutine);
		}
	}

	IEnumerator MoveingDoor(Vector3 target, Vector3 back) {
		while (!isFinish) {
			door.transform.position = Vector3.MoveTowards (door.transform.position, target, Time.deltaTime * 2.5f);
			if (door.transform.position != target)
				yield return null;
			else {
				Debug.Log ("Step one");
				stepOne = true;
				target = back;
			}
			if (door.transform.position == back && stepOne) {
				isFinish = true;
				Moving = false;
				Debug.Log ("Finish");
			}
		}
	}
}
