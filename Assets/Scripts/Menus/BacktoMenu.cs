using UnityEngine;
using System.Collections;

public class BacktoMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetButtonDown ("AbilityX1")) || (Input.GetButtonDown ("AbilityX2"))){
						Application.LoadLevel (1);
				}
	}
}
