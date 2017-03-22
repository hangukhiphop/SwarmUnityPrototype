using UnityEngine;
using System.Collections;

public class fwdLine : MonoBehaviour {
	public float length;
	
	void Update () {
		Debug.DrawRay(collider.bounds.center, transform.forward * length, Color.green);	
	}
}
