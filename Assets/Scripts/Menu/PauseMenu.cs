using ItsHarshdeep.LoadingScene.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

	public void LoadByIndex(string index) {
		GameManager.instance.isPause = false;
		//Time.timeScale = 1.0f;
        SceneController.LoadLevel(index, 2f);
    }

	public void ResumeGame()
    {
        GameManager.instance.isPause = false;
    }
}
