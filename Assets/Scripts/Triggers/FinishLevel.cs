using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag ("Player"))
			EventManager.TriggerEvent ("OnLevelFinish","level1");
	}

}
