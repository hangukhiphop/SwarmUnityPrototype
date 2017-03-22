using UnityEngine;
using System.Collections;

public class Flee : MonoBehaviour {
	public Transform[] exitPoints = new Transform[5];
	public float[] fleeDelay = new float[2];
	public Transform focalPoint;
	public float fleeTime;
	
	private Transform myExitPoint;
	
	void Start () {
		myExitPoint = exitPoints[Random.Range(exitPoints.GetLowerBound(0), exitPoints.GetUpperBound(0))];
		Debug.Log(myExitPoint.name + " " + exitPoints.GetLowerBound(0) + " " + exitPoints.GetUpperBound(0));
		StartCoroutine(runAway());
	}
	
	IEnumerator runAway () {
		transform.LookAt(myExitPoint.position);
		yield return new WaitForSeconds(Random.Range(fleeDelay[0], fleeDelay[1]));
		iTween.MoveTo(gameObject, myExitPoint.position, fleeTime);
	}
}
