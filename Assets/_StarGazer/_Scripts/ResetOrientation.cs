using UnityEngine;
using System.Collections;

public class ResetOrientation : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)){
			ResetView();
		}
	}

	void ResetView() {
		Debug.Log("Reset view");
		OVRDevice.ResetOrientation(0);
	}

}
