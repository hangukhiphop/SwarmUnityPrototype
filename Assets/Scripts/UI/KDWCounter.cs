using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KDWCounter : MonoBehaviour {

	public Text DeathCount;
	int deathcount;

	// Use this for initialization
	void Start () 
	{
		deathcount = 0;
		DeathCount.text = "0";
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void IncrementDeathCount()
	{
		deathcount++;
		DeathCount.text = deathcount.ToString ();
	}
}
