using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ItsHarshdeep.LoadingScene.Controller;
using UnityEngine.EventSystems;

public partial class GameManager : MonoBehaviour {

	public static GameManager instance;

	public float stamina;
	public float stress;
	public bool isPause;
	public bool canSwitch;
	public bool inLab;
	public bool keyPuzzle2;
	private bool isFinishedTransfer;

    void Awake () {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
		stamina = 100;
		stress = 0;
		isPause = false;
		var levelId = SceneManager.GetActiveScene ().buildIndex;
		canSwitch = levelId != 2;
		inLab = false;
		keyPuzzle2 = false;
	    isFinishedTransfer = false;
    }

	void OnEnable() {
		EventManager.StartListening ("OnLevelFinish",FinishLevel);
        EventManager.StartListening("OnFinishLineSliding", SetFinishLineSLiding);
	}

	void OnDisable() {
		EventManager.StopListening ("OnLevelFinish",FinishLevel);
	    EventManager.StopListening("OnFinishLineSliding", SetFinishLineSLiding);
    }

	void OnDestroy() {
		EventManager.StopListening ("OnLevelFinish",FinishLevel);
	    EventManager.StopListening("OnFinishLineSliding", SetFinishLineSLiding);
    }

	void Update () {
		InputChecking ();
        if (UIManager.instance) { UIManager.instance.staminaSlider.value = stamina; }
		staminaManage();
        if (UIManager.instance) { UIManager.instance.pausePanel.SetActive(isPause); }
		Time.timeScale = isPause ? 0.0f : 1.0f;
	}

	void FinishLevel(params object[] list) {
		var levelIndex = (string)list [0];
		PlayerPrefs.SetString ("lastLevel", levelIndex);
		SceneController.LoadLevel(levelIndex,2f);
	}

    void SetFinishLineSLiding(params object[] list)
    {
        isFinishedTransfer = true;
	    print("Set isFini bla bla to TRUE");
    }

	private void staminaManage()
	{
		if (inLab && stress < 255)
			stress += 35f * Time.deltaTime;
		else if (!inLab && stress > 0) {
			stress -= 60f * Time.deltaTime;
		}
		if (stress > 243 && inLab)
		{
			var transgressionRoutine = Transgression(true);
			StartCoroutine(transgressionRoutine);
			inLab = !inLab;
		}
		if (stress < 0)
			stress = 0;
	}
}
