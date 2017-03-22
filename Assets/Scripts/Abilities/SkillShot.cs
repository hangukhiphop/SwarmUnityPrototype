using UnityEngine;
using System.Collections;

public class SkillShot : DamageSource {

	public float range;
	Vector3 origin;
	public float SPEED;

	public Material OrangeSkin;
	public Material CyanSkin;
	// Use this for initialization
	void Start () 
	{
		origin = transform.position;
		if(damage_source.tag.Contains ("1"))
		{
			renderer.material = CyanSkin;
		}
		else if(damage_source.tag.Contains ("2"))
		{
			renderer.material = OrangeSkin;
		}
	}

	// Update is called once per frame
	void Update () {

		if(Vector3.Distance (transform.position, origin) > range)
		{
			Destroy (gameObject);
		}
		transform.position += transform.forward * SPEED *Time.deltaTime;
	}

	public void OnTriggerEnter(Collider collider)
	{
		string allyTag = damage_source.gameObject.tag;
		string enemyTag = collider.transform.root.gameObject.tag;
		if(collider.transform.root.gameObject != damage_source && 
		   (enemyTag.StartsWith ("I") || enemyTag.StartsWith ("W")) && 
		   !enemyTag.EndsWith("" + allyTag[allyTag.Length - 1]))
		{
			target = collider.transform.root.gameObject;
			OnTargetHit (false);			
			Destroy (gameObject);
		}
	}
}
