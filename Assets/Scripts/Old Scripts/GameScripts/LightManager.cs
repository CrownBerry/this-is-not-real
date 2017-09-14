using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

	private Light mainLight;

	void Awake () {
		mainLight = GameObject.Find ("Main light").GetComponent<Light> ();
		Debug.Log (mainLight.range);
	}

	void OnEnable() {
		EventManager.StartListening ("OnLightSwitch",SwitchLighting);
	}

	void OnDisable() {
		EventManager.StopListening ("OnLightSwitch",SwitchLighting);
	}

	void OnDestroy() {
		EventManager.StopListening ("OnLightSwitch",SwitchLighting);
	}
	
	void SwitchLighting (params object[] list)
	{
		var where = (bool) list[0];
		mainLight.gameObject.SetActive(where);
	}

}
