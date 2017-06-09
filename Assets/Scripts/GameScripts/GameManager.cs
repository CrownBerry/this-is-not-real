using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ItsHarshdeep.LoadingScene.Controller;

public partial class GameManager : MonoBehaviour {

	public static GameManager instance;

	public float stamina;
	public float stress;
	public bool isPause;
	public bool canSwitch;
	public bool inLab;
	public bool keyPuzzle2;

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
		int levelId = SceneManager.GetActiveScene ().buildIndex;
		if (levelId == 2)
			canSwitch = false;
		else
			canSwitch = true;
		inLab = false;
		keyPuzzle2 = false;
	}

	void OnEnable() {
		EventManager.StartListening ("OnLevelFinish",FinishLevel);
	}

	void OnDisable() {
		EventManager.StopListening ("OnLevelFinish",FinishLevel);
	}

	void OnDestroy() {
		EventManager.StopListening ("OnLevelFinish",FinishLevel);		
	}

	void Update () {
		InputChecking ();
        if (UIManager.instance) { UIManager.instance.staminaSlider.value = stamina; }

		//STRESS//
		if (inLab && stress < 255)
			stress += 35f * Time.deltaTime;
		else if (!inLab && stress > 0) {
			stress -= 60f * Time.deltaTime;
		}

		if (stress > 243 && inLab)
			EventManager.TriggerEvent ("OnPlayerSwitch", true);
			EventManager.TriggerEvent ("OnLightSwitch");

		if (stress < 0)
			stress = 0;
        //STRESS END//

        if (UIManager.instance) { UIManager.instance.pausePanel.SetActive(isPause); }
		if (isPause) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
	}

	void FinishLevel(params object[] list) {
		string levelIndex = (string)list [0];
		PlayerPrefs.SetString ("lastLevel", levelIndex);
		//SceneManager.LoadScene (levelIndex);
		SceneController.LoadLevel(levelIndex,2f);
	}
}
