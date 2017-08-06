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
	    try
	    {
		    EventManager.TriggerEvent("OnPlayerMoveSwitch", false);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnCameraCanMove");
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
        var lineGoal = localInLab ? 0f : 1f;
	    try
	    {
		    EventManager.TriggerEvent("OnCameraSwitch", lineGoal);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnViewportGoal", lineGoal);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnShadowSlide", localInLab);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnPlayerSwitch", forced, localInLab);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnGirlsVisible", true, true);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    if (localInLab)
			    EventManager.TriggerEvent("OnModelTransfer", 0f, 100f);
		    else
			    EventManager.TriggerEvent("OnModelTransfer", -100f, 0f);
		    print("Step one transgression");
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    while (!isFinishedTransfer)
	    {
		    yield return null;
	    }
	    print("Step two go");
	    isFinishedTransfer = false;
	    try
	    {
		    EventManager.TriggerEvent("OnModelTransfer", 0f, 0f);

	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnGirlsVisible", localInLab, !localInLab);

	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnCameraCanMove");
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnLightSwitch", localInLab);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
	    try
	    {
		    EventManager.TriggerEvent("OnPlayerMoveSwitch", true);
	    }
	    catch (Exception e)
	    {
		    print(e);
		    throw;
	    }
    }
}
