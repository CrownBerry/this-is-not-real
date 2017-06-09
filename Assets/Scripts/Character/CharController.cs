using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class CharController : MonoBehaviour {

	public float jumpSpeed = 36.0f; //14.0f;
	float speed = 6.0f;
	float maxSpeed = 8.0f;

	float xMov;
	float myVelocity;

	float distGround;

	CapsuleCollider capscol;
	public GameObject witchModel;
	public GameObject vrModel;
	public GameObject cameraObject;
	GameObject grabbedObj;

	Rigidbody rb;

	int Iter;

	public bool isInside;
	public bool onPlatform;

	bool canMove;

	Vector3 tp;
	bool isRight;
	public bool grabFlag;

	public float vel;


	void Awake () {
		int levelId = SceneManager.GetActiveScene ().buildIndex;
		if (levelId == 2)
			jumpSpeed = 10.0f;
		else
			jumpSpeed = 16.0f;
		capscol = GetComponent<CapsuleCollider> ();
		distGround = capscol.bounds.extents.y;
		isRight = true;
		rb = GetComponent<Rigidbody> ();
		isInside = false;
		onPlatform = false; 
	}

	void OnEnable() {
		EventManager.StartListening ("OnPlayerSwitch", Switch);
		EventManager.StartListening ("OnChangeSpeed", ChangeSpeed);
		EventManager.StartListening ("OnChangeJump", ChangeJump);
	}

	void OnDisable() {
		EventManager.StopListening ("OnPlayerSwitch", Switch);
		EventManager.StopListening ("OnChangeSpeed", ChangeSpeed);
		EventManager.StopListening ("OnChangeJump", ChangeJump);
		
	}

	void OnDestroy() {
		EventManager.StopListening ("OnPlayerSwitch", Switch);
		EventManager.StopListening ("OnChangeSpeed", ChangeSpeed);
		EventManager.StopListening ("OnChangeJump", ChangeJump);
	}

	void Update () {
		myVelocity = rb.velocity.x;
		xMov = Input.GetAxisRaw ("Horizontal");

		if (!grabFlag) 
			RotateGirl (xMov);
		CheckPoint ();
		if (!grabFlag)
			CheckBox ();
		else
			DropBox (grabbedObj, false);

		if (Input.GetButtonDown ("Jump"))
		if (Physics.Raycast(transform.position,-Vector3.up,distGround - 1.0f))
			Jump ();

		vel = rb.velocity.x;
	}

	void FixedUpdate() {
		RealGravity (!onPlatform);
		Move (xMov);
	}

	void ChangeSpeed(params object[] list) {
		float newSpeed = (float)list [0];
		float newMaxSpeed = (float)list [1];
		speed = newSpeed;
		maxSpeed = newMaxSpeed;
	}

	void ChangeJump(params object[] list) {
		float newJump = (float)list [0];
		jumpSpeed = newJump;
	}

	void GrabBox(GameObject box) {
		FixedJoint myJoint;
		myJoint = box.AddComponent<FixedJoint>();
		myJoint.connectedBody = rb;
		myJoint.enableCollision = true;
		box.GetComponent<Rigidbody> ().useGravity = false;
		box.GetComponent<Rigidbody> ().mass = 0.1f;
		grabFlag = true;
		grabbedObj = box;
	}

	void DropBox(GameObject box, bool forced) {
		if (Input.GetButtonDown ("Use") || forced) {
			FixedJoint myJoint;
			myJoint = box.GetComponent<FixedJoint> ();
			myJoint.connectedBody = null;
			Destroy (myJoint);
			box.GetComponent<Rigidbody> ().useGravity = true;
			box.GetComponent<Rigidbody> ().mass = 5f;
			grabbedObj = null;
			grabFlag = false;
		}
	}

	void Switch(params object[] list) {
		bool forced = (bool)list [0];
		if (grabFlag)
			DropBox (grabbedObj, true);
		float xDev;
		Vector3 point;
		if (isInside && forced) {
			point = FindPoint ();
			xDev = point.x - transform.position.x;
		}
		else
			xDev = 0f;
		if (!isInside || forced) {
			EventManager.TriggerEvent ("OnCameraCanMove");
			if (GameManager.instance.inLab)
            {
                EventManager.TriggerEvent("OnCameraSwitch", 0f);
                EventManager.TriggerEvent("OnViewportGoal", 0f);
                tp = new Vector3 (transform.position.x + xDev, transform.position.y - 100,0f);
				vrModel.SetActive (false);
				witchModel.SetActive (true);
			} else
            {
                EventManager.TriggerEvent("OnCameraSwitch", 1f);
                EventManager.TriggerEvent("OnViewportGoal", 1f);
                tp = new Vector3 (transform.position.x + xDev, transform.position.y + 100,0f);
				vrModel.SetActive (true);
				witchModel.SetActive (false);
			}
			GameManager.instance.inLab = !GameManager.instance.inLab;
			transform.position = tp;
			EventManager.TriggerEvent ("OnCameraCanMove");
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("item"))
			col.gameObject.GetComponent<ItemsScript> ().GiveItem ();
	}

	void Restart() {
		
	}
}
