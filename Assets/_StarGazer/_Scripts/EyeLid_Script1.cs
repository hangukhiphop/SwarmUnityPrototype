using UnityEngine;
using System.Collections;

public class EyeLid_Script1 : MonoBehaviour {
	

	
	// Use this for initialization
	void Start () {
		if (this.gameObject.tag == "upLid") {
			iTween.MoveBy(this.gameObject, iTween.Hash("y", 1.0f, "time", 10.0f, "easetype", iTween.EaseType.linear, "islocal", true ));
		}
		else if (this.gameObject.tag == "downLid") {
			iTween.MoveBy(this.gameObject, iTween.Hash("y", -1.0f, "time", 10.0f, "easetype", iTween.EaseType.linear, "islocal", true ));
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
