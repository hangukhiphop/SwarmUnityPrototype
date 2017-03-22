using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject followee;
	int followeeID;

	Vector3 startPosition;
	Quaternion startRotation;
	float xRotation;
	float yRotation;
	public Vector3 offset;
	public float OffsetScalar_Forward;
	public float OffsetScalar_Up;
	float offsetMag;
	float cameraRotate;
	// Use this for initialization
	void Start () 
	{
		followeeID = followee.gameObject.name [followee.gameObject.name.Length - 1] - '0';
		//xRotation = 0;
		//yRotation = 0;

		//the vector of fixed length that will be between the player and the camera
		InitCamera ();
		offsetMag = Vector3.Magnitude (offset);
		//startPosition = transform.position;
		//startRotation = Quaternion.identity;
	}

	public void InitCamera()
	{
		offset = OffsetScalar_Forward * followee.transform.forward + OffsetScalar_Up * Vector3.up;
		transform.position = followee.transform.position + offset;		
		cameraRotate = 0;
	}

	// Update is called once per frame
	void Update () 
	{

		//For joystick camera in 3D
		//xRotation = 5*Input.GetAxis ("Mouse X" + gameObject.name [gameObject.name.Length - 1]);
		//yRotation = -5*Input.GetAxis ("Mouse Y" + gameObject.name [gameObject.name.Length - 1]);
		//transform.rotation = Quaternion.LookRotation (startPosition - transform.position);
		//transform.rotation = startRotation*Quaternion.AngleAxis (xRotation, Vector3.up)*Quaternion.AngleAxis (yRotation, -Vector3.right);

		//transform.RotateAround (followee.transform.position, Vector3.up, 100*xRotation*Time.deltaTime);
		//transform.RotateAround (followee.transform.position, Vector3.Cross (transform.position - followee.transform.position, Vector3.up), 100*yRotation*Time.deltaTime);


		cameraRotate = -300*Input.GetAxis ("Mouse X" + followeeID)*Time.deltaTime;
		transform.RotateAround (followee.transform.position, Vector3.up, cameraRotate);

		//orbit player by camera movement controls
		offset = Quaternion.Euler (new Vector3 (0, cameraRotate, 0))*offset;	

		//reposition by offset so that camera distance is not affected by player collisions
		if(Vector3.Distance (transform.position, followee.transform.position) != offsetMag)
		{
			transform.position = followee.transform.position + offset;
		}

		//always look at the player
		transform.rotation = Quaternion.LookRotation (followee.transform.position - transform.position + Vector3.up);
	}
}
