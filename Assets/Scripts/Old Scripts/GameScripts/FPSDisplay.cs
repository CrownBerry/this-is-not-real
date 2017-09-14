using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;
	public Text fpsCounter;
 
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("FPS: {0:0.} ", fps);

		fpsCounter.text = text;
	}
}