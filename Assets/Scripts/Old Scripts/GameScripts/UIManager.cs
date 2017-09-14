using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	Text tipsText;
	Text infoText;
	public GameObject pausePanel;
	public Slider staminaSlider;
	public Image stressImage;
	public static UIManager instance;
	public Color col;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
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
		col.a = GameManager.instance.stress / 255.0f;
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


}
