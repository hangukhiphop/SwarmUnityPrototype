
using UnityEngine;
using System.Collections;

public class CameraStarview_Script2 : MonoBehaviour {
	
	public Camera cameraL;
	public Camera cameraR;
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		Vector3 avrgPos = (cameraL.transform.position + cameraR.transform.position) / 2f;
		Vector3 avrgDirection = (cameraL.transform.forward + cameraR.transform.forward) * 200f;
		
		Ray centerRay = new Ray(avrgPos, avrgDirection);
		RaycastHit hit;
                
		if (Physics.Raycast(centerRay, out hit)) {
			if (hit.transform.gameObject.tag == "star") {
				//Debug.Log("hit star");
                hit.collider.gameObject.SendMessage("Glance");  //Just tell whatever GO that I'm looking at that I am looking at you
			}
		}
		
		Debug.DrawRay(avrgPos, avrgDirection, Color.magenta);
		
	}
	
}
