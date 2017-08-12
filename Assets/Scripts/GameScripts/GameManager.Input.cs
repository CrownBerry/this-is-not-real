using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class GameManager : MonoBehaviour
{

	void InputChecking() {
		if (Input.GetButtonDown ("Teleport")) {
			if (canSwitch && ((stress < 100 && !inLab) || inLab) ) {
				var transgressionRoutine = Transgression(false);
				StartCoroutine(transgressionRoutine);
				inLab = !inLab;
			}
		} 

		if (Input.GetButtonDown ("Sprint") && stamina > 6.0f) {
			EventManager.TriggerEvent ("OnChangeSpeed", 18f, 12f);
		}
		if (Input.GetButtonUp ("Sprint") || stamina < 6.0f) {
			EventManager.TriggerEvent ("OnChangeSpeed", 6f, 8f);
		}
		if (Input.GetButton ("Sprint") && stamina > 6.0f && (Math.Abs(Input.GetAxis("Horizontal")) > float.Epsilon )) {
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

    private IEnumerator Transgression(bool forced)
    {
	    var localInLab = inLab;
	    EventManager.TriggerEvent("OnPlayerMoveSwitch", false);
        EventManager.TriggerEvent("OnCameraCanMove");
        var lineGoal = localInLab ? 0f : 1f;
        EventManager.TriggerEvent("OnCameraSwitch", lineGoal);
        EventManager.TriggerEvent("OnViewportGoal", lineGoal);
	    EventManager.TriggerEvent("OnShadowSlide", localInLab);
	    EventManager.TriggerEvent("OnPlayerSwitch", forced, localInLab);
	    EventManager.TriggerEvent("OnGirlsVisible", true, true);
	    if (localInLab)
	    	EventManager.TriggerEvent("OnModelTransfer", 0f, 100f);
	    else
		    EventManager.TriggerEvent("OnModelTransfer", -100f, 0f);
	    print("Step one transgression");
	    while (!isFinishedTransfer)
	    {
		    yield return null;
	    }
	    print("Step two go");
	    isFinishedTransfer = false;
		EventManager.TriggerEvent("OnModelTransfer", 0f, 0f);
	    EventManager.TriggerEvent("OnGirlsVisible", localInLab, !localInLab);
        EventManager.TriggerEvent("OnCameraCanMove");
	    EventManager.TriggerEvent("OnLightSwitch", localInLab);
	    EventManager.TriggerEvent("OnPlayerMoveSwitch", true);
    }
}
