using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform target;
	float shift;
	bool right;
	Vector3 tp;
	public bool canMove;
    public float vertShift;
    public string camName;

	void Awake() {
		right = true;
		canMove = true;
		shift = 5;
	}

	void OnEnable() {
		EventManager.StartListening ("OnCameraSwitch", Shifts);
		EventManager.StartListening ("OnCameraCanMove", CanMoveSwitching);
	}

	void OnDisable() {
		EventManager.StopListening ("OnCameraSwitch", Shifts);
		EventManager.StopListening ("OnCameraCanMove", CanMoveSwitching);
	}
	void Update () {
        //COSTYL
        if (GameManager.instance.inLab )
	        vertShift = camName == "second" ? 0 : -100;
        else
	        vertShift = camName == "second" ? 100 : 0;
        //END COSTYL

		if (Input.GetAxis ("Horizontal") > 0 && !right) {
			shift = 4;
			right = !right;
		} else if (Input.GetAxis ("Horizontal") < 0 && right) {
			shift = -4;
			right = !right;
		}
			
		if (canMove) 
			transform.position = Vector3.Lerp (transform.position, 
				target.position + new Vector3 (shift, 4.8f + vertShift, -10f), Time.deltaTime * 4f);
	}

	void Switch(params object[] list) {
		float xDiv = (float)list [0];
		if (GameManager.instance.inLab) {
			tp = new Vector3 (transform.position.x + xDiv, transform.position.y - 100, -7f);
		} else {
			tp = new Vector3 (transform.position.x + xDiv, transform.position.y + 100, -7f);
		}
		transform.position = tp;
	}

    void Shifts(params object[] list)
    {
        if (GameManager.instance.inLab)
            vertShift -= 100;
        else
            vertShift += 100;
    }

	void CanMoveSwitching(params object[] list) {
		canMove = !canMove;
	}
}
