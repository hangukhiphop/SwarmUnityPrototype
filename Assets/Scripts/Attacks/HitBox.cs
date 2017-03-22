using UnityEngine;
using System.Collections;

public class HitBox : DamageSource {

	private int colony;
	PlayerControl parentScript;
	Animator enemyAnim;
	Transform pTransform;
	//float halfConeDeg = 30f;
	StatManager SM;

	private SphereCollider MyCollider;
	private AudioSource HitSound;

	public Transform Origin; 
	//private SkeletonBone Attacker;

	// Use this for initialization
	void Start () 
	{
		parentScript = transform.root.GetComponent<PlayerControl> ();
		pTransform = transform.root.transform;
		damage_source = transform.root.gameObject;		
		colony = (int)damage_source.tag [damage_source.tag.Length - 1] - '0';
		SM = transform.root.GetComponent<StatManager> ();
		damage = SM.physicalStrength;
		MyCollider = GetComponent<SphereCollider> ();
		HitSound = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Origin.position;

		//Debug.Log (transform.root.transform.forward);

		/*Vector3 origin = Vector3.zero;
		Vector3 target = Vector3.zero;
		switch(parentScript.currentAttack)
		{
		case 1:
			
			origin = pTransform.position + 1.2f*Vector3.up;
			target = new Vector3(1.7f, 0, -1.2f);
			break;
		case 2:
			target = new Vector3(1.13f, 2, -1f);
			break;
		}

		target = Quaternion.AngleAxis (pTransform.eulerAngles.y - 90, Vector3.up) * target;
		Vector3 maxLeft = Quaternion.AngleAxis (-halfConeDeg, Vector3.up) * target;
		Vector3 maxRight = Quaternion.AngleAxis (halfConeDeg, Vector3.up) * target;
		
		float hitLength = Vector3.Magnitude (target);
		Debug.DrawLine (origin, origin + Vector3.Normalize(target)*hitLength, Color.yellow);
		Debug.DrawLine (origin, origin + Vector3.Normalize(maxLeft)*hitLength, Color.red);
		Debug.DrawLine (origin, origin + Vector3.Normalize(maxRight)*hitLength, Color.blue);
		RaycastHit hit;
		RaycastHit leftHit;
		RaycastHit rightHit;
		if(!Physics.Raycast (origin,  Vector3.Normalize (target), out hit, hitLength))// && parentScript.currentAttack != 0)
		{
			bool hitsLeft = Physics.Raycast (origin,  Vector3.Normalize (maxLeft), out leftHit, hitLength);
			bool hitsRight = Physics.Raycast (origin,  Vector3.Normalize (maxRight), out rightHit, hitLength);
			if(!hitsRight && hitsLeft && leftHit.transform.root != transform.root)
			{
				Debug.DrawLine (origin, leftHit.point, Color.green);
				hit = leftHit;
			}
			else if(hitsRight && rightHit.transform.root != transform.root)
			{
				Debug.DrawLine (origin, rightHit.point, Color.green);
				hit = rightHit;
			}
			else
			{
				return;
			}
			Vector3 targetDistance = hit.point - pTransform.position;
			Quaternion newRotation = Quaternion.FromToRotation(target - target.y*Vector3.up, Vector3.Normalize (targetDistance - targetDistance.y*Vector3.up));
			pTransform.rotation = Quaternion.Lerp( pTransform.rotation, pTransform.rotation*newRotation, Time.deltaTime*5);
		}*/
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.root != transform.root && collider.transform.root.gameObject.tag.Contains ("" + (3 - colony)))
		{
			if(transform.root.gameObject.tag.StartsWith ("W") || (transform.root.gameObject.tag.StartsWith ("I") && parentScript.CurrentControl == PlayerControl.ControlType.Attack))
			{
				if(transform.root.gameObject.tag.StartsWith ("W"))
				{
					MyCollider.enabled = false;
				}
				else
				{
					HitSound.time = .5f;
				}

				HitSound.Play ();
				target = collider.transform.root.gameObject;
				OnTargetHit (true);
				if(collider.gameObject.tag.StartsWith ("I"))
				{
					collider.gameObject.GetComponent<AbilityManager>().DeactivateAbilityGuide();
				}
			}
		}
	}

/*	public IEnumerator PlayAnimationOnce(string parameterName, int tagID, Collider collider)
	{
		enemyAnim.SetInteger( parameterName, tagID );
		yield return null;
		enemyAnim.SetInteger( parameterName, 0 );


	}*/
}
