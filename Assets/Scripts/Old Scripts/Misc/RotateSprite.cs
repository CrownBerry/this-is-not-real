using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour {

	void Update () {
		transform.Rotate (new Vector3 (0, 0, -600 * Time.deltaTime));
	}
}
