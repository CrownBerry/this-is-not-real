using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	Text tipsText;
	Text infoText;

	private GameObject pausePanel;
	private Slider staminaSlider;
	private Image stressImage;
	private Color col;

	private bool isPause;

	// Use this for initialization
	void Awake () {
		tipsText = GameObject.Find("Tips").GetComponent<Text> ();
		infoText = GameObject.Find ("Info").GetComponent<Text> ();
		staminaSlider = GameObject.Find ("StaminaSlider").GetComponent<Slider> ();
		stressImage = GameObject.Find ("StressImage").GetComponent<Image> ();
		pausePanel = GameObject.Find ("PauseMenu");
		pausePanel.SetActive (false);
		tipsText.text = "";
		col = stressImage.color;
	}

	void Update() {
		col.a = 0 / 255.0f;
		stressImage.color = col;
	}
	
	public void setText(string str) {
		tipsText.text = str;
	}

	public void setInfo(bool bl) {
		if (bl)
			infoText.text = "Grounded: true";
		else 
			infoText.text = "Grounded: false";
	}


	public void SwitchPause()
	{
        isPause = !isPause;

        Time.timeScale = isPause ? 0.0f : 1.0f;
        pausePanel.SetActive(isPause);
	}
}
