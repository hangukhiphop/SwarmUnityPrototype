using UnityEngine;
using System.Collections;

public class DamageSource : MonoBehaviour {

	public GameObject damage_source;
	public GameObject target;
	public int damage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTargetHit(bool physical)
	{	
		target.GetComponent<DamageTaker>().OnRecAttack(damage_source, damage, physical);
	}



}
