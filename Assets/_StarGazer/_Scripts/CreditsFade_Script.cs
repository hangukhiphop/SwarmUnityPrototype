using UnityEngine;
using System.Collections;

public class CreditsFade_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//this.gameObject.renderer.material.color.a = 0;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		iTween.ValueTo(this.gameObject, iTween.Hash ("from", 0, "to", 255.0f, "time", 3.0f, "onupdate", "TweenUpdater"));
		
		
		if(Input.GetKeyUp(KeyCode.R)){
			
			Restart();
		}
		
	}

	void TweenUpdater() {

	}
	
	void Restart(){
		
		print ("Restart StarGazer");
		
		Application.LoadLevel(0);
		
	}


}
