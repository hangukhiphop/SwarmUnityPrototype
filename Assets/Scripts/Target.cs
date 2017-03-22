using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
	
	GUITexture targetGUI;
	Transform followee;
	float xScreenPos;
	float yScreenPos;
	float sensitivity = 15;
	float centerx;
	float centery;
	int xMin;
	int xMax;
	float yMin;
	float yMax;
	public Camera myCam;
	public bool visible;
	public Vector3 TargetLocation;

		// Use this for initialization
	void Start () 
	{		
		followee = myCam.GetComponent<FollowPlayer>().followee.transform;
		targetGUI = gameObject.GetComponent<GUITexture> ();
		xScreenPos = -targetGUI.pixelInset.width*.5f;
		yScreenPos = -targetGUI.pixelInset.height*.5f;
		xMin = -Screen.width/2;
		xMax = Screen.width/2 - (int)targetGUI.pixelInset.width;
		yMin = -myCam.pixelHeight / 2;
		yMax = myCam.pixelHeight/2 - (int)targetGUI.pixelInset.height;
		centerx = xScreenPos;
		centery = yScreenPos;
		visible = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(false)
		{
			targetGUI.enabled = false;
		}
		else
		{
			targetGUI.enabled = false;
			PositionTarget ();
		}

		//float dxScreenPos = sensitivity*Input.GetAxis ("Mouse X" + gameObject.name[gameObject.name.Length - 1]);
		//float dyScreenPos = -sensitivity*Input.GetAxis ("Mouse Y" + gameObject.name[gameObject.name.Length - 1]);

		//float xScreenPos = myCam.camera.WorldToScreenPoint(TargetLocation).x;// + 10*Input.GetAxis ("Mouse X" + gameObject.name [gameObject.name.Length - 1]);
		//float yScreenPos = myCam.camera.WorldToScreenPoint(TargetLocation).y;// + 10*Input.GetAxis ("Mouse Y" + gameObject.name [gameObject.name.Length - 1]);



		//Debug.Log (centerx + " " + centery);

		//rect.x = centerx;
		//rect.y = centery;
		//xScreenPos = centerx;
	//	yScreenPos = centery;
		/*if(dxScreenPos == 0 && dyScreenPos == 0)
		{

		}
		else
		{
			if((dxScreenPos + xScreenPos) > xMin && (dxScreenPos + xScreenPos) < xMax)
			{			
				xScreenPos += dxScreenPos;
			}
			if((dyScreenPos + yScreenPos) > yMin && (dyScreenPos + yScreenPos) < yMax)
			{			
				yScreenPos += dyScreenPos;
			}
			rect.x = xScreenPos;
			rect.y = yScreenPos;
		}*/


		


	}

	public Quaternion WorldAimConversion()
	{
		Vector3 ScreenToWorld;
		ScreenToWorld = myCam.camera.ScreenToViewportPoint 
						(new Vector3(targetGUI.pixelInset.x + Screen.width*.5f + targetGUI.pixelInset.width*.5f,
			             targetGUI.pixelInset.y + myCam.pixelHeight*.5f + myCam.pixelRect.yMin + targetGUI.pixelInset.height*.5f, 0));
		ScreenToWorld -= ScreenToWorld.z*Vector3.forward;
		Vector3 proj = Vector3.Project (ScreenToWorld, followee.position + myCam.transform.forward);
		float length = 10f - Vector3.Magnitude(ScreenToWorld);
		ScreenToWorld = ScreenToWorld + (myCam.transform.forward - myCam.transform.forward.y*Vector3.up)*length;
		ScreenToWorld = myCam.camera.ScreenToWorldPoint (new Vector3(targetGUI.pixelInset.x + Screen.width*.5f + targetGUI.pixelInset.width*.5f, 
		                                                             targetGUI.pixelInset.y + myCam.pixelHeight*.5f  + myCam.pixelRect.yMin + targetGUI.pixelInset.height*.5f, 
		                                                         	 myCam.camera.nearClipPlane + Vector3.Distance (followee.position, myCam.transform.position) 
		                                                                   + Vector3.Magnitude(ScreenToWorld)));	

		Quaternion dir = Quaternion.LookRotation (ScreenToWorld - followee.position);
		return dir;
	}

	public void SetTargetLocation()
	{
		TargetLocation = followee.transform.position + new Vector3 (myCam.transform.forward.x, 0, myCam.transform.forward.z)*10;//myCam.transform.forward * 10;
	}

	public Vector3 moveTarget()
	{
		float xAim = sensitivity*Input.GetAxis ("Mouse X" + gameObject.name [gameObject.name.Length - 1]);
		float yAim = sensitivity*Input.GetAxis ("Mouse Y" + gameObject.name [gameObject.name.Length - 1]);
		return (TargetLocation + yAim*Vector3.up + xAim*myCam.transform.right);
	}

	public void PositionTarget()
	{
		SetTargetLocation ();
		Vector3 WorldToScreen = myCam.camera.WorldToScreenPoint (followee.transform.position + new Vector3 (myCam.transform.forward.x, 0, myCam.transform.forward.z)*10);	
		centerx = WorldToScreen.x - (Screen.width + targetGUI.pixelInset.width)*.5f;
		centery = WorldToScreen.y - (Screen.height + targetGUI.pixelInset.height)*.5f;
		//Debug.Log (WorldToScreen);

		Rect rect = targetGUI.pixelInset;
		//Debug.Log (targetGUI.pixelInset.width);

		rect.x = centerx;
		rect.y = centery;
		//rect.width = 64;
		//rect.height = 64;

		targetGUI.pixelInset = rect;

	}
}
