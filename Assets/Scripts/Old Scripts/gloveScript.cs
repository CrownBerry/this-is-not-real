using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gloveScript : MonoBehaviour {

	void OnEnable() {
		EventManager.StartListening ("OnPickupGlove",SetNotKinematic);
	}

	void OnDisable() {
		EventManager.StopListening ("OnPickupGlove", SetNotKinematic);
	}

	void OnDestroy() {
		EventManager.StopListening ("OnPickupGlove", SetNotKinematic);
	}

	void SetNotKinematic(params object[] list) {
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
	}
}
