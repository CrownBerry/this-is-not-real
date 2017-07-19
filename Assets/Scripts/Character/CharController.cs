using UnityEngine;
using UnityEngine.SceneManagement;

public partial class CharController : MonoBehaviour {

	public float jumpSpeed = 36.0f; //14.0f;
    private float speed = 6.0f;
    private float maxSpeed = 8.0f;

    private float xMov;
    private float myVelocity;

    private float distGround;

    private CapsuleCollider capscol;
	public GameObject witchModel;
	public GameObject vrModel;
	public GameObject cameraObject;
    private GameObject grabbedObj;

    private Rigidbody rb;

    private int iter;

	public bool isInside;
	public bool onPlatform;

	private bool canMove;

    private Vector3 tp;
    private bool isRight;
	public bool grabFlag;

	public float vel;


	void Awake () {
		var levelId = SceneManager.GetActiveScene ().buildIndex;
		jumpSpeed = levelId == 2 ? 10.0f : 16.0f;
		capscol = GetComponent<CapsuleCollider> ();
		distGround = capscol.bounds.extents.y;
		isRight = true;
		rb = GetComponent<Rigidbody> ();
		isInside = false;
		onPlatform = false;
		canMove = true;
	}

	void OnEnable() {
		EventManager.StartListening ("OnPlayerSwitch", Switch);
		EventManager.StartListening ("OnChangeSpeed", ChangeSpeed);
		EventManager.StartListening ("OnChangeJump", ChangeJump);
        EventManager.StartListening("OnGirlsVisible", SetGirlsVisiible);
        EventManager.StartListening("OnModelTransfer", TransferModel);
		EventManager.StartListening("OnPlayerMoveSwitch", SetPlayerMoveAbility);
	}

	void OnDisable() {
		EventManager.StopListening ("OnPlayerSwitch", Switch);
		EventManager.StopListening ("OnChangeSpeed", ChangeSpeed);
		EventManager.StopListening ("OnChangeJump", ChangeJump);
        EventManager.StopListening("OnGirlsVisible", SetGirlsVisiible);
        EventManager.StopListening("OnModelTransfer", TransferModel);
		EventManager.StopListening("OnPlayerMoveSwitch", SetPlayerMoveAbility);

    }

	void OnDestroy() {
		EventManager.StopListening ("OnPlayerSwitch", Switch);
		EventManager.StopListening ("OnChangeSpeed", ChangeSpeed);
		EventManager.StopListening ("OnChangeJump", ChangeJump);
        EventManager.StopListening("OnGirlsVisible", SetGirlsVisiible);
        EventManager.StopListening("OnModelTransfer", TransferModel);
		EventManager.StopListening("OnPlayerMoveSwitch", SetPlayerMoveAbility);
    }

	void Update ()
	{
		rb.isKinematic = !canMove;
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
		if (canMove)
			Move (xMov);
	}

    private void ChangeSpeed(params object[] list) {
		float newSpeed = (float)list [0];
		float newMaxSpeed = (float)list [1];
		speed = newSpeed;
		maxSpeed = newMaxSpeed;
	}

    private void ChangeJump(params object[] list) {
		float newJump = (float)list [0];
		jumpSpeed = newJump;
	}

    private void GrabBox(GameObject box) {
	    var myJoint = box.AddComponent<FixedJoint>();
		myJoint.connectedBody = rb;
		myJoint.enableCollision = true;
		box.GetComponent<Rigidbody> ().useGravity = false;
		box.GetComponent<Rigidbody> ().mass = 0.1f;
		grabFlag = true;
		grabbedObj = box;
	}

    private void DropBox(GameObject box, bool forced) {
		if (Input.GetButtonDown ("Use") || forced) {
		    var myJoint = box.GetComponent<FixedJoint> ();
			myJoint.connectedBody = null;
			Destroy (myJoint);
			box.GetComponent<Rigidbody> ().useGravity = true;
			box.GetComponent<Rigidbody> ().mass = 5f;
			grabbedObj = null;
			grabFlag = false;
		}
	}

    private void Switch(params object[] list) {
		var forced = (bool)list [0];
        var directionDown = (bool)list[1];
        var playerPos = transform.position;
		if (grabFlag)
			DropBox (grabbedObj, true);
		float xDev;
        if (isInside && forced)
		{
		    var point = FindPoint ();
		    xDev = point.x - transform.position.x;
		}
		else
			xDev = 0f;
		if (!isInside || forced) {
			//EventManager.TriggerEvent ("OnCameraCanMove");
			if (directionDown)
            {
                //EventManager.TriggerEvent("OnCameraSwitch", 0f);
                //EventManager.TriggerEvent("OnViewportGoal", 0f);
                tp = new Vector3 (playerPos.x + xDev, playerPos.y - 100,0f);
				//vrModel.SetActive (false);
				//witchModel.SetActive (true);
			} else
            {
                //EventManager.TriggerEvent("OnCameraSwitch", 1f);
                //EventManager.TriggerEvent("OnViewportGoal", 1f);
                tp = new Vector3 (playerPos.x + xDev, playerPos.y + 100,0f);
				//vrModel.SetActive (true);
				//witchModel.SetActive (false);
			}
			//GameManager.instance.inLab = !GameManager.instance.inLab;
			transform.position = tp;
			//EventManager.TriggerEvent ("OnCameraCanMove");
		}
	}

    private void SetGirlsVisiible(params object[] list) {
        var witch = (bool)list[0];
        var vr = (bool)list[1];
        witchModel.SetActive(witch);
        vrModel.SetActive(vr);
    }

    private void TransferModel(params object[] list)
    {
	    var witchShift = (float) list[0];
	    var vrShift = (float) list[1];
        witchModel.transform.localPosition = new Vector3(0f, witchShift, 0f);
        vrModel.transform.localPosition = new Vector3(0f, vrShift, 0f);
    }

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("item"))
			col.gameObject.GetComponent<ItemsScript> ().GiveItem ();
	}

	void Restart() {
		
	}

	private void SetPlayerMoveAbility(params object[] list)
	{
		var newCanMove = (bool) list[0];
		canMove = newCanMove;
	}
}
