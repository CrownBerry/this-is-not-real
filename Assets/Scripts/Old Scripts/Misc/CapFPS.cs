﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapFPS : MonoBehaviour {

	void Awake () {
		Application.targetFrameRate = 30;
	}
}
