using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour {

	void InputChecking() {
		if (Input.GetButtonDown ("Teleport")) {
			if (canSwitch && ((stress < 100 && !inLab) || inLab) ) {
				EventManager.TriggerEvent ("OnPlayerSwitch", false);
				EventManager.TriggerEvent ("OnLightSwitch");
			}
		} 

		if (Input.GetButtonDown ("Sprint") && stamina > 6.0f) {
			EventManager.TriggerEvent ("OnChangeSpeed", 18f, 12f);
		}
		if (Input.GetButtonUp ("Sprint") || stamina < 6.0f) {
			EventManager.TriggerEvent ("OnChangeSpeed", 6f, 8f);
		}
		if (Input.GetButton ("Sprint") && stamina > 6.0f && (Input.GetAxis("Horizontal") != 0 )) {
			stamina -= 40f * Time.deltaTime;
		}
		if (stamina < 100 && !Input.GetButton ("Sprint"))
			stamina += 25f * Time.deltaTime;
		else if (stamina > 100)
			stamina = 100;
		if (Input.GetButtonDown ("Exit")) {
			isPause = !isPause;
		}
	}
}
