using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour {

	private int MyColony;

	public GameObject enemyNest;

	public AutoAttacker AA;
	public DamageTaker DT;

	public Material mat_team1;
	public Material mat_team2;

	SphereCollider myCollider;
	Rigidbody myRB;

	public enum Action {Move, Attack, Die};
	public Action currentAction;

	Animator MyAnimator;

	string enemyWorkerTag;

	StatManager SM;

	Steerer MySteerer;

	int LorR;
	// Use this for initialization
	void Start () 
	{
		AA = gameObject.GetComponent<AutoAttacker>();
		AA.ATTACK_DELAY = 2f;		
		AA.Start ();

		LorR = (int)Mathf.Pow (-1, Random.Range (0, 2));
		myCollider = gameObject.GetComponent<SphereCollider> ();
		myRB = gameObject.GetComponent<Rigidbody> ();
		if(transform.forward == Vector3.right)
		{
			renderer.material = mat_team1; 
			gameObject.tag = "W1";
			enemyWorkerTag = "W2";
			enemyNest = GameObject.FindGameObjectWithTag ("N2");
		}
		else
		{
			renderer.material = mat_team2;
			gameObject.tag = "W2";
			enemyWorkerTag = "W1";
			enemyNest = GameObject.FindGameObjectWithTag ("N1");
		}
		AA.target = enemyNest;

		currentAction = Action.Move;
		MyAnimator = gameObject.GetComponent<Animator> ();		
		AA.SetAttackAnimation ("State", 2);

		SM = gameObject.GetComponent<StatManager> ();

		
		DT = gameObject.GetComponent<DamageTaker> ();
		DT.Start ();
		DT.CurrentPriority = DamageTaker.Signal.None;

		
		MyColony = (int)gameObject.tag [gameObject.tag.Length - 1] - '0';

		MySteerer = gameObject.GetComponent<Steerer> ();
	}

	void Update()
	{
	}

	// Update is called once per frame
	void FixedUpdate () 
	{

		GameObject nearestSignal = findNearestSignal();		
		//Check if any target other than enemy nest is available
		if(nearestSignal != null)
		{
			DT.SetSignal (nearestSignal);	
			AA.target = DT.CurrentSignal.transform.parent.gameObject;
		}
		else
		{
			GameObject nearestTarget = findNearestTarget ();
			if(nearestTarget != null)
			{
				AA.target = nearestTarget;
			}
		}

		//If target just got destroyed, default is enemy nest
		//Lose signal
		if(AA.target == null || Vector3.Distance (AA.target.transform.position, transform.position) > DT.SIGNAL_RANGE)
		{
			currentAction = Action.Move;
			DT.CurrentPriority = DamageTaker.Signal.None;
			AA.target = enemyNest;
		}






		//Check if within range of target
		if(AA.target != null && Vector3.Distance (AA.target.transform.position, transform.position) < AA.ATTACK_RANGE)
		{
			currentAction = Action.Attack;
		}
		else if(AA.target != null)
		{
			currentAction = Action.Move;
		}
		else
		{
			currentAction = Action.Die;
		}

		switch(currentAction)
		{
			case Action.Move:
				MyAnimator.SetInteger("State", 1);
				MySteerer.Reposition();
				transform.position += transform.forward*Time.deltaTime*SM.movementSpeed;
				AA.attackTimer.Stop ();

			break;
		    case Action.Attack:
				MyAnimator.SetInteger("State", 2);
				transform.rotation = Quaternion.LookRotation 
					(new Vector3(AA.target.transform.position.x - transform.position.x, 0, AA.target.transform.position.z - transform.position.z));
				AA.attackTimer.Begin (true);
			break;
			case Action.Die:
			MyAnimator.SetInteger("State", 3);
			break;

		}
		myRB.velocity = Vector3.zero;
	}


	GameObject findNearestSignal()
	{
		GameObject closest = null;
		GameObject[] signals = GameObject.FindGameObjectsWithTag ("Signal" + MyColony);
		for(int i = 0; i < signals.Length; i++)
		{

			float shortestDistance = DT.SIGNAL_RANGE;
			float thisDistance = Vector3.Distance (transform.position, signals[i].transform.position);
			if(thisDistance < shortestDistance)
			{
				shortestDistance = thisDistance;
				closest = signals[i];
			}
		}
		return closest;
	}

	GameObject findNearestTarget()
	{
		GameObject closest = enemyNest;
		float shortestDistance = DT.SIGNAL_RANGE;
		GameObject[] enemyWorkers = GameObject.FindGameObjectsWithTag(enemyWorkerTag);
		for(int i = 0; i < enemyWorkers.Length; i++)
		{
			float thisDistance = Vector3.Distance (transform.position, enemyWorkers[i].transform.position);
			if(thisDistance < shortestDistance)
			{
				shortestDistance = thisDistance;
				closest = enemyWorkers[i];
			}
		}
		//if(closest == null && GameObject.FindGameObjectWithTag ("I)
	//	{
			//AA.target = enemyNest;
		//}
		return closest;
	}
}
