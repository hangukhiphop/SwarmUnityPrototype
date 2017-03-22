using UnityEngine;
using System.Collections;

public class Projectile : DamageSource {

	public float SPEED;

	private StatManager TargetSM;
	
	public Material Pollen1;
	public Material Pollen2;

	// Use this for initialization
	void Start () 
	{
		TargetSM = target.GetComponent <StatManager> ();
		if(damage_source.tag.Contains ("1"))
		{
			renderer.material = Pollen1;
		}
		else if(damage_source.tag.Contains ("2"))
		{
			renderer.material = Pollen2;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target == null || TargetSM.current_health <= 0)
		{
			Destroy(gameObject);
		}
		else
		{
			transform.position += SPEED * Time.deltaTime * Vector3.Normalize((target.transform.position + target.transform.localScale.y*Vector3.up) - transform.position);
		}

	}

	public void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject == target)
		{	
			OnTargetHit ();
		}
	}

	public void OnTargetHit()
	{		
		base.OnTargetHit (true);		
		Destroy (gameObject);
	}


}
