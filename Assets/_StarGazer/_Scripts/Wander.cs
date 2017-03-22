using UnityEngine;
using System.Collections;
 
/// <summary>
/// Creates wandering behaviour for a CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Wander : MonoBehaviour
{
	public float[] pauseGate = new float[2];
	public float[] pauseDuration = new float[2];
	public float lookAhead = 1.5f;
	public bool pause = false;
	private float currPauseDurration;
	private float nextPause;
	
	public float speed = 5;
	public float directionChangeInterval = 1;
	public float maxHeadingChange = 30;
 
	CharacterController controller;
	float heading;
	Vector3 targetRotation;
 
	void Awake ()
	{
		controller = GetComponent<CharacterController>();
 
		// Set random initial rotation
		heading = Random.Range(0, 360);
		transform.eulerAngles = new Vector3(0, heading, 0);
		StartCoroutine(NewHeading());
		animation.Play("move");
		nextPause = Random.Range(pauseGate[0], pauseGate[1]);
	}
 
	void Update ()
	{	
		if (Physics.Raycast(transform.position, transform.forward, lookAhead)) {
			targetRotation = new Vector3(0, transform.rotation.y + 180, 0);
			//Debug.Log("cast hit! New Rot! " + targetRotation.ToString());
		}
		
		if (nextPause <= 0) 
			StartCoroutine(pauseMovement());
		
		if (!pause) {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
			var forward = transform.TransformDirection(Vector3.forward);
			controller.SimpleMove(forward * speed);
			nextPause -= Time.deltaTime;
		}	
	}
 
	/// <summary>
	/// Repeatedly calculates a new direction to move towards.
	/// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
	/// </summary>
	IEnumerator NewHeading ()
	{
		while (true) {
			NewHeadingRoutine();
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}
	
	//Method to pause the movement of the thing
	IEnumerator pauseMovement () {
		pause = true;
		animation.CrossFade("idle");
		currPauseDurration = Random.Range(pauseDuration[0], pauseDuration[1]);
		while (currPauseDurration >= 0) {
			currPauseDurration -= Time.deltaTime;
			yield return null;
		}
		nextPause = Random.Range(pauseGate[0], pauseGate[1]);
		pause = false;
		animation.CrossFade("move");
	}
 
	/// <summary>
	/// Calculates a new direction to move towards.
	/// </summary>
	void NewHeadingRoutine ()
	{
		var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
		var ceil  = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
		heading = Random.Range(floor, ceil);
		targetRotation = new Vector3(0, heading, 0);
		//Debug.Log(targetRotation.ToString());
	}
}