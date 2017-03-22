using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
	
public class Steerer : MonoBehaviour {

	public float MaxSteer;
	public int numSteerOptions;
	private float DegInterval;
	public float RayCastLength;
	public Vector3 target;
	private Vector3[] SteerOptions;
	private Quaternion ToRotation;
	private int green = 0;

	public AutoAttacker AA;
	Rigidbody rb;

	private LinkedList<Vector3> SteerSequence;
	// Use this for initialization
	void Start () 
	{
		AA = gameObject.GetComponent<AutoAttacker>();
		rb = gameObject.GetComponent<Rigidbody> ();
		SteerOptions = new Vector3[numSteerOptions];
		//SteerSequence = new LinkedList<Vector3> ();
		//SteerSequence.AddFirst (transform.position);
		DegInterval = 2f / (numSteerOptions - 1)*MaxSteer;
		ResetSteerOptions ();
		ToRotation = transform.rotation;
		/*for(int i = 0; i < numSteerOptions; i++)
		{
			float angle = (int)((i + 1)*.5f)*Mathf.Pow (-1, i % 2)*DegInterval;
			//Debug.Log (i + " " + angle);
			Vector3 X = Quaternion.Euler (new Vector3(0, angle, 0))*transform.forward;
			float length;
			Vector3 TC = transform.forward*(RayCastLength + 5);
			float A = Vector3.Dot (X,X);
			float B = 2*Vector3.Dot (TC, X);
			float C = Vector3.Dot (TC, TC) - 5*5;
			Debug.Log (B*B - 4*A*C);
			length = -(-B + Mathf.Sqrt (B*B - 4*A*C))/(2*A);

			//float length = RayCastLength/(Mathf.Cos(angle*Mathf.Deg2Rad));
		    //Debug.Log (length);
			SteerOptions[i] = X*length*(int)(i*.5f + 1.5f);
			//SteerSequence.AddAfter (SteerSequence.First, SteerOptions[i]);
		}*/

	}


	// Update is called once per frame
	void Update()
	{
		//transform.rotation = Quaternion.Lerp (transform.rotation, ToRotation, Time.deltaTime);
	}

	public void Reposition () 
	{
		if(!RayCastTest (Vector3.Normalize (AA.target.transform.position - transform.position)*RayCastLength))
		{
			transform.rotation = Quaternion.LookRotation (new Vector3(AA.target.transform.position.x - transform.position.x, 0, AA.target.transform.position.z - transform.position.z));
			ResetSteerOptions();
		}
		if(RayCastTest(SteerOptions[0]))
		{
			FindPath ();
		}
		for(int i = 0; i < numSteerOptions; i++)
		{
			if(green == i)
			//Debug.DrawLine (transform.position, transform.position + SteerOptions[i]);
			//else
				Debug.DrawLine (transform.position, transform.position + SteerOptions[i], Color.green);
		}

	}

	bool RayCastTest(Vector3 ray)
	{
		RaycastHit hit;
		if(rigidbody.SweepTest(ray, out hit, Vector3.Magnitude (ray)) && hit.collider.gameObject.layer != LayerMask.NameToLayer ("Hittable"))// && hit.transform.gameObject.tag.Contains ("W"))
		{
			return true;
		}
		return false;
	}

	void FindPath()
	{
		int attempt = 1;
		Vector3 ValidDirection = Vector3.zero;
		RaycastHit hit;
		do
		{
			//Debug.Log (attempt);
			if(!RayCastTest (SteerOptions[attempt]))
			{
				ValidDirection = SteerOptions[attempt];
				//SteerSequence.AddAfter (SteerSequence.Find (SteerOptions[attempt]), transform.position + 
				//Debug.Log (attempt);
				transform.rotation = Quaternion.LookRotation (SteerOptions[attempt]);
				//ResetSteerOptions();
				green = attempt;


			}
			attempt++;
			//Debug.Log (attempt);
		}while(attempt < numSteerOptions && ValidDirection == Vector3.zero);
	}

	void ResetSteerOptions()
	{
		for(int i = 0; i < numSteerOptions; i++)
		{
			float angle = (int)((i + 1)*.5f)*Mathf.Pow (-1, i % 2)*DegInterval;
			//Debug.Log (i + " " + angle);
			Vector3 X = Quaternion.Euler (new Vector3(0, angle, 0))*transform.forward;
			/*float length;
			Vector3 TC = transform.forward*(RayCastLength + 5);
			float A = Vector3.Dot (X,X);
			float B = 2*Vector3.Dot (TC, X);
			float C = Vector3.Dot (TC, TC) - 5*5;
			//Debug.Log (B*B - 4*A*C);
			length = -(-B + Mathf.Sqrt (B*B - 4*A*C))/(2*A);
			
			//float length = RayCastLength/(Mathf.Cos(angle*Mathf.Deg2Rad));
			//Debug.Log (length);
			SteerOptions[i] = X*length*(int)(i*.5f + 1.5f);*/
			SteerOptions[i] = X*RayCastLength*((i + 1)/2 + 1);
			//SteerSequence.AddAfter (SteerSequence.First, SteerOptions[i]);
		}
	}
}
