using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ItsHarshdeep.LoadingScene.Controller;

public class MainMenu : MonoBehaviour {

	public GameObject resumeBtn;

	void Awake() {
		if (PlayerPrefs.HasKey ("lastLevel")) {
			resumeBtn.SetActive (true);
		}
	}

	public void ClearData() {
		PlayerPrefs.DeleteKey ("lastLevel");
		resumeBtn.SetActive (false);
	}

	public void ContGame()
    {
        SceneController.LoadLevel(PlayerPrefs.GetString("lastLevel"), 2f);
    }

	public void LoadByIndex(string levelIndex) {
		//SceneManager.LoadScene (index);
		SceneController.LoadLevel(levelIndex,2f);
    }

	public void Quit() {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
