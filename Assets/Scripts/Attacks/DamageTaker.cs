using UnityEngine;
using System.Collections;

public class DamageTaker : MonoBehaviour {

	//Can this unit currently take damage
	public bool Vulnerable;

	public float SIGNAL_RANGE;	
	public int colony;
	public enum Signal{IvI, WvI, SvW, WvW, WvS, IvS, W, I, None};
	public Signal CurrentPriority;

	StatManager SM;
	int Colony;
	Sentinel allySentinel;
	GameObject SignalTarget;

	public GameObject SignalPrefab;
	public GameObject CurrentSignal;

	// Use this for initialization
	public void Start () 
	{		
		Vulnerable = true;
		CurrentPriority = Signal.None;
		Colony = (int)(gameObject.tag [1] - '0');
		SM = gameObject.GetComponent<StatManager> ();
		if(GameObject.FindGameObjectWithTag ("S" + Colony) != null)
		{
			allySentinel = GameObject.FindGameObjectWithTag ("S" + Colony).GetComponent<Sentinel>();
		}
	}
	
	// Update is called once per frame
	public void Update () 
	{

	}

	public void SetSignal(GameObject signal)
	{
		CurrentSignal = signal;
		string x = signal.name;
		
		//ebug.Log ("" + x[7] + x[8] + x[9]);
		CurrentPriority = (Signal)System.Enum.Parse (typeof(Signal),"" + x[7] + x[8] + x[9]);
	}

	public void CreateSignal(GameObject signalTarget, Signal priority)
	{
		if(priority >= CurrentPriority && !(CurrentSignal == null))
		{
			return;
		}
		else
		{	
			GameObject newSig = (GameObject)Instantiate (SignalPrefab, transform.position, Quaternion.identity);
			newSig.gameObject.tag = "Signal" + gameObject.tag[gameObject.tag.Length - 1];	
			newSig.gameObject.name = "Signal" + gameObject.tag[gameObject.tag.Length - 1] + priority;
			CurrentSignal = newSig;
			CurrentPriority = priority;
			newSig.transform.parent = signalTarget.transform;
		}

		/*GameObject[] allyWorkers = GameObject.FindGameObjectsWithTag ("W" + Colony);
		for(int i = 0; i < allyWorkers.Length; i++)
		{
			Worker allyScript = allyWorkers[i].GetComponent<Worker>();
			if(priority < allyScript.DT.currentSignal && Vector3.Distance (transform.position, allyWorkers[i].transform.position) < SIGNAL_RANGE)
			{
				allyScript.AA.target = signalTarget;
				allyScript.DT.currentSignal = priority;			
			}
		}


		if(priority == Signal.IvI)
		{
			if(Vector3.Distance(allySentinel.gameObject.transform.position, signalTarget.transform.position) < allySentinel.AA.ATTACK_RANGE)
			{
				allySentinel.AA.target = signalTarget;
			}
		}*/
	}


	public void OnRecAttack(GameObject damage_source, int damage, bool physical)
	{
		if (Vulnerable && damage_source != null && damage_source.tag.EndsWith ("" + (3 - Colony)))
		{
			SM.ComputeDamage (damage_source, damage, physical);
			string enumStr = damage_source.tag[0] + "v" + gameObject.tag[0];
			if(System.Enum.IsDefined(typeof(Signal), enumStr))
			{
				CreateSignal (damage_source, (Signal)System.Enum.Parse (typeof(Signal),enumStr));
			}

		}

	}





}
