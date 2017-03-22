
using UnityEngine;
using System.Collections;

public class CameraStarview_Script : MonoBehaviour {

    
	// Use this for initialization
	void Start () {

	}
	
	public Vector3 rayPos = new Vector3(0.5f, 0.5f, 0f);
	
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			UpdateCenterPos(0, -1f);
		}else if(Input.GetKeyDown(KeyCode.UpArrow)){
			UpdateCenterPos(0, 1f);
		}else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			UpdateCenterPos(-1f, 0);
		}else if(Input.GetKeyDown(KeyCode.RightArrow)){
			UpdateCenterPos(1f, 0);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		
		
		
		//check if looking at a bad cube
		RaycastHit hit;
        Ray ray;
        if (gameObject.tag == "MainCamera")
        {
            ray = Camera.main.ViewportPointToRay(rayPos);
			Debug.DrawLine(rayPos, rayPos * rayPos.magnitude * 100, Color.red, 1.0f);
        }
        else
        {
			//print ("Using FPS Camera : " + Camera.current.name);
            ray = Camera.current.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }
        
		if (Physics.Raycast(ray, out hit)) {
			if (hit.transform.gameObject.tag == "star") {
				//Debug.Log("hit star");
                hit.collider.gameObject.SendMessage("Glance");  //Just tell whatever GO that I'm looking at that I am looking at you
			}
			
		}
		
	}
	
	void UpdateCenterPos(float x, float y){
		
		rayPos = new Vector3(((rayPos.x*100f) + x)/100f, ((rayPos.y*100f) + y)/100f, 0f);
		
		print ("rayPos: " + rayPos + ", " + x + " , " + y);
	}
	
	
}
