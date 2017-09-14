using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsScript : MonoBehaviour {

	public string itemName;

	public void GiveItem () {
		if (itemName == "boots") {
			EventManager.TriggerEvent ("OnChangeJump", 16f);
			Debug.Log ("Get boots!");
		} else if (itemName == "vrhelmet") {
			GameManager.instance.canSwitch = true;
		} else if (itemName == "glove") {
			EventManager.TriggerEvent("OnPickupGlove");
		} else if (itemName == "keyPuzTwo") {
			GameManager.instance.keyPuzzle2 = true;
		}
		Destroy (gameObject);
	
	}

}
