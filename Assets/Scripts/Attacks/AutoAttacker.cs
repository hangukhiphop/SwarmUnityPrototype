using UnityEngine;
using System.Collections;

public class AutoAttacker : MonoBehaviour {

	Animator MyAnimator;

	public GameObject attackObj;
	public GameObject target;

	public GameObject myTimer;	
	public Timer attackTimer;

	public enum AttackType{Melee, Projectile};
	public AttackType myAttackType;
	public float ATTACK_RANGE;
	public float ATTACK_DELAY;
	public Transform sourceLoc;

	string AttackAnimation_Name;
	int AttackAnimation_Index;

	StatManager SM;

	// Use this for initialization
	public void Start () 
	{
		MyAnimator = gameObject.GetComponent<Animator> ();
		//AttackAnimation_Name = "";
		//myAttackType = AttackType.Projectile;
		attackTimer = ((GameObject)Instantiate (myTimer, transform.position, Quaternion.identity)).GetComponent<Timer>();
		attackTimer.Init(ATTACK_DELAY, Attack, gameObject);

		if(myAttackType == AttackType.Melee)
		{
			HitBox HB = attackObj.GetComponent<HitBox> ();
			HB.damage_source = gameObject;
		}

		SM = GetComponent<StatManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetAttackAnimation(string name, int index)
	{
		AttackAnimation_Name = name;
		AttackAnimation_Index = index;
	}

	void PlayAttackAnimation()
	{
		if(!(AttackAnimation_Name == null))
		{
			MyAnimator.SetInteger (AttackAnimation_Name, AttackAnimation_Index);
		}
		else
		{
			return;
		}
	}

	void Attack()
	{
		PlayAttackAnimation ();
		if(myAttackType == AttackType.Melee)
		{
			Strike ();
		}
		else if(myAttackType == AttackType.Projectile)
		{
			Shoot ();
		}
	}



	public void Shoot()
	{
		if(target != null)
		{
			GameObject proj = (GameObject)Instantiate (attackObj, sourceLoc.position, Quaternion.identity);
			Projectile projS = proj.GetComponent<Projectile> ();
			projS.target = target;
			projS.damage_source = gameObject;
			projS.damage = SM.physicalStrength;
		}

	}

	public void Strike()
	{
		if(target != null)
		{
			attackObj.GetComponent<SphereCollider>().enabled = true;
		}
	}
}
