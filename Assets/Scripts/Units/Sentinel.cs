using UnityEngine;
using System.Collections;

public class Sentinel : MonoBehaviour {
	
	private int colony;
	private Animator MyAnimator;
	public AutoAttacker AA;
	public DamageTaker DT;


	// Use this for initialization
	void Start () 
	{
		colony = gameObject.tag [gameObject.tag.Length - 1] - '0';
		MyAnimator = GetComponent<Animator> ();

		DT = gameObject.GetComponent<DamageTaker> ();
		AA = gameObject.GetComponent<AutoAttacker> ();
		AA.ATTACK_RANGE = 20f;
		AA.ATTACK_DELAY = 2f;		
		DT.SIGNAL_RANGE = 8f;
		
		//AA.sourceLoc = //transform.localScale.z * Vector3.forward + transform.localScale.y * Vector3.up;
		AA.Start ();
		AA.SetAttackAnimation ("AnimState", 1);

		DT.Start ();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(AA.target == null || Vector3.Distance (AA.target.transform.position, transform.position) > AA.ATTACK_RANGE)
		{
			AA.target = findNearestTarget ();
			if(AA.target != null)
			{
				AA.attackTimer.Begin (true);
			}
			else
			{				
				MyAnimator.SetInteger ("AnimState", 0);
				AA.attackTimer.Stop ();
			}
		}
	}

	GameObject findNearestTarget()
	{
		GameObject closest = null;
		float shortestDistance = AA.ATTACK_RANGE;
		GameObject[] enemyWorkers = GameObject.FindGameObjectsWithTag("W" + (3 - colony));
		for(int i = 0; i < enemyWorkers.Length; i++)
		{
			float thisDistance = Vector3.Distance (transform.position, enemyWorkers[i].transform.position);
			if(thisDistance < shortestDistance)
			{
				shortestDistance = thisDistance;
				closest = enemyWorkers[i];
			}
		}

		if(closest == null)
		{
			GameObject enemyInsectoid = GameObject.FindGameObjectWithTag ("I" + (3 - colony));
			if(Vector3.Distance (transform.position, enemyInsectoid.transform.position) < shortestDistance)
			{
				closest = enemyInsectoid;
			}
		}

		return closest;
	}

	//GameObject findEnemyInsectoid()
	//{

		//if(Vector3.Distance (transform.position, enemyWorkers[i].transform.position
	//}
}
