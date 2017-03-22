using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class planetoidMotor : MonoBehaviour {
	public Transform tweenDest;
	public float timeToImpact = 180;
	public float turnSpeed = 3; 
		
	private Hashtable itweenOptions = new Hashtable();
	
	void Awake () {
		itweenOptions.Add("position", tweenDest.position);
		itweenOptions.Add("time", timeToImpact);
		itweenOptions.Add("easeType", "easeInOutExpo");
		itweenOptions.Add("oncomplete", "endTheGame");
	}
	
	void Start () {
		iTween.MoveTo(gameObject, itweenOptions);
	}
	
	void Update () {
		transform.Rotate(Vector3.right * Time.deltaTime * turnSpeed);
	}
	
	void OnCollisionEnter (Collision col) {
		if(col.gameObject.tag == "player") {
			Debug.Log("BOOM!");
			Application.LoadLevel(1);
		}	
	}
		
	void endTheGame () {
		gameManager.endGame();
	}
}
