using UnityEngine;
using System.Collections;

public class Combat : MonoBehaviour{

	FollowPlayer FollowCam;
	public int player;

	Animator animator;
	public int currentAttack;
	float attackTime;
	public bool LDodging = false;
	Rigidbody myRB;

	bool CanMove;

	float startTime;
	Vector3 dodgeStart;
	Vector3 dodgeEnd;
	Vector3 center;
	Quaternion startRot;
	Quaternion endRot;

	public DamageTaker DT;

	public GameObject tempAtk;
	Vector3 AtkRestPos;
	public GameObject SkillShot;
	Vector3 SSRelPos;

	public GameObject rangeIndicator;
	GameObject arrow;
	bool indicating = false;

	public GameObject myTimer;
	Timer CoolDownTimer;
	const float CoolDownDuration = 7f;
	bool onCoolDown;

	public GameObject Shield;

	StatManager SM;

	public Camera myCam;
	public Target target;

	enum CurrentMove {None, Attack, Shield, Ability};
	CurrentMove currentMove;

	Quaternion rot;
	// Use this for initialization
	void Start () 
	{
		animator = gameObject.GetComponent<Animator> ();
		player = (int)(gameObject.name [gameObject.name.Length - 1] - '0');
		FollowCam = myCam.GetComponent<FollowPlayer> ();
		DT = gameObject.GetComponent<DamageTaker> ();
		DT.Start ();
		DT.colony = player;
		DT.SIGNAL_RANGE = 10f;
		currentAttack = 0;
		attackTime = 0;
		myRB = gameObject.GetComponent<Rigidbody> ();

		AtkRestPos = new Vector3 (.25f, 0, 0);
		SSRelPos = new Vector3 (0, .25f, 1f);
		CoolDownTimer = ((GameObject)Instantiate (myTimer, Vector3.zero, Quaternion.identity)).GetComponent<Timer>();
		onCoolDown = false;
		CoolDownTimer.Init (CoolDownDuration, AbilityRefresh, gameObject);
		Shield.renderer.enabled = false;
		SM = gameObject.GetComponent<StatManager> ();
		CanMove = true;
		Shield.renderer.enabled = false;

		currentMove = CurrentMove.None;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(currentMove != CurrentMove.Shield && currentMove != CurrentMove.Ability)
		{
			if(Input.GetButtonDown ("RB" + player))
			{
				currentAttack = 1;
				tempAtk.animation.Play();
				currentMove = CurrentMove.Attack;
				//CanMove = false;
				StartCoroutine( PlayAnimationOnce("Punching") );
			}
			else if(!tempAtk.animation.isPlaying)
			{
				currentMove = CurrentMove.None;
			}
		}

		if(currentMove != CurrentMove.Attack && currentMove != CurrentMove.Ability)
		{
			if(Input.GetButton("LB" + player))
			{
				//currentAttack = 3;
				Shield.renderer.enabled = true;
				currentMove = CurrentMove.Shield;
				if(arrow != null)
				{
					Destroy (arrow.gameObject);
					indicating = false;
				}
				//StartCoroutine( PlayAnimationOnce("Attack_Down") );
			}
			else
			{
				Shield.renderer.enabled = false;
				currentMove = CurrentMove.None;
			}
		}


		if(Input.GetButtonDown("AbilityX" + player) && !onCoolDown && currentMove != CurrentMove.Shield && currentMove != CurrentMove.Attack)
		{
			arrow = (GameObject)Instantiate (rangeIndicator, transform.position + Vector3.up*.5f, Quaternion.identity);
			arrow.layer = LayerMask.NameToLayer("Cam" + (player) + "Layer");
			arrow.transform.localScale = new Vector3(10, 1, 1);
			arrow.transform.position += arrow.transform.right*arrow.transform.localScale.x*.5f;
				indicating = true;				
				target.SetTargetLocation();
				rot = Quaternion.LookRotation (target.TargetLocation - transform.position)*Quaternion.Euler (90, 0, 90);
				currentMove = CurrentMove.Ability;
			//}//SM.current_mana -= 35;
			
			//StartCoroutine( PlayAnimationOnce("Kicking") );
		}
		if(Input.GetButtonUp ("AbilityX" + player) && indicating && !onCoolDown)
		{
			StartCoroutine( PlayAnimationOnce("Ability1") );
			Quaternion dir;
			dir = Quaternion.LookRotation (target.TargetLocation - transform.position);

			GameObject ability = (GameObject)Instantiate(SkillShot, transform.position + Vector3.up*.5f, arrow.transform.rotation*Quaternion.Euler (90, 90, 0));
			ability.GetComponent<SkillShot>().damage_source = gameObject;
			Destroy (arrow.gameObject);
			transform.eulerAngles = new Vector3(0, dir.eulerAngles.y, 0);
			indicating = false;
			onCoolDown = true;
			CoolDownTimer.Begin(false);
			target.visible = false;
			currentMove = CurrentMove.None;
		}

		if(indicating)
		{	
			if(Vector3.Magnitude(new Vector3(Input.GetAxis ("Horizontal" + gameObject.name [gameObject.name.Length - 1]), Input.GetAxis ("Vertical" + gameObject.name [gameObject.name.Length - 1]), 0)) > .8f)
			{
				rot = Quaternion.LookRotation (target.TargetLocation - transform.position)*Quaternion.LookRotation(new Vector3(Input.GetAxis ("Horizontal" + gameObject.name [gameObject.name.Length - 1]), 0, Input.GetAxis ("Vertical" + gameObject.name [gameObject.name.Length - 1])))*Quaternion.Euler (90, -90, 0);
				
			}
			arrow.transform.rotation = rot;
			//Quaternion dir = target.WorldAimConversion();			
			//arrow.transform.rotation = dir*Quaternion.Euler (90, -90, 0);
			//arrow.transform.rotation = Quaternion.LookRotation (target.TargetLocation - transform.position)*Quaternion.Euler (90, -90, 0);
			//arrow.transform.rotation *= Quaternion.Euler (0, 60*Input.GetAxis ("Mouse Y" + gameObject.name [gameObject.name.Length - 1]), 0)
			//*Quaternion.AngleAxis (-60*Input.GetAxis ("Mouse X" + gameObject.name [gameObject.name.Length - 1]), new Vector3(0, 0, 1));
			arrow.transform.position = transform.position + arrow.transform.right*arrow.transform.localScale.x*.5f;
		}

		if(arrow != null)
		{
			arrow.transform.rotation = Quaternion.LookRotation (target.moveTarget() - transform.position);
			Debug.DrawLine (transform.position, target.TargetLocation, Color.red);
		}

		if(currentMove == CurrentMove.None)
		{
			float horizontal = Input.GetAxis ("Horizontal" + player);
			float vertical = Input.GetAxis ("Vertical" + player);
			if(horizontal != 0 || vertical != 0)
			{
				animator.SetInteger ("AnimState", 1);
				transform.rotation = Quaternion.Euler (new Vector3(0, myCam.transform.eulerAngles.y, 0))*Quaternion.LookRotation(new Vector3(horizontal, 0, vertical));
				Vector3 deltaForward = transform.forward*SM.movementSpeed*Time.deltaTime;
				deltaForward = Vector3.Project (deltaForward, new Vector3(myCam.transform.forward.x, 0, myCam.transform.forward.z));
				transform.position += deltaForward;
				myCam.transform.position += deltaForward;
				if(horizontal != 0)
				{
					OrbitCamera (-horizontal);
				}				
				//animator.SetBool ("Running", true);
			}
			else
			{
				animator.SetInteger ("AnimState", 0);
			}
		}
		else
		{
			Debug.Log (currentMove);
		}

		if(indicating)
		{	
			if(Vector3.Magnitude(new Vector3(Input.GetAxis ("Horizontal" + gameObject.name [gameObject.name.Length - 1]), Input.GetAxis ("Vertical" + gameObject.name [gameObject.name.Length - 1]), 0)) > .8f)
			{
				rot = Quaternion.LookRotation (target.TargetLocation - transform.position)*Quaternion.LookRotation(new Vector3(Input.GetAxis ("Horizontal" + gameObject.name [gameObject.name.Length - 1]), 0, Input.GetAxis ("Vertical" + gameObject.name [gameObject.name.Length - 1])))*Quaternion.Euler (90, -90, 0);

			}
			arrow.transform.rotation = rot;
			//Quaternion dir = target.WorldAimConversion();			
			//arrow.transform.rotation = dir*Quaternion.Euler (90, -90, 0);
			//arrow.transform.rotation = Quaternion.LookRotation (target.TargetLocation - transform.position)*Quaternion.Euler (90, -90, 0);
			//arrow.transform.rotation *= Quaternion.Euler (0, 60*Input.GetAxis ("Mouse Y" + gameObject.name [gameObject.name.Length - 1]), 0)
			                        //*Quaternion.AngleAxis (-60*Input.GetAxis ("Mouse X" + gameObject.name [gameObject.name.Length - 1]), new Vector3(0, 0, 1));
			arrow.transform.position = transform.position + arrow.transform.right*arrow.transform.localScale.x*.5f;
		}


		/*else if(Input.GetKeyDown (KeyCode.A) && Input.GetKey (KeyCode.LeftShift))
		{
			LDodging = true;
			startTime = Time.time;
			dodgeStart = transform.position;
			dodgeEnd = dodgeStart + (transform.forward - Quaternion.AngleAxis (30, Vector3.up)*transform.forward)*2;
			startRot = transform.rotation;
			endRot = Quaternion.Euler (startRot.eulerAngles + Vector3.up*30);			
			StartCoroutine( PlayAnimationOnce("Dodging_left") );
		}*/

		/*if(Input.GetKey(KeyCode.A) && !Input.GetKey (KeyCode.LeftShift) && !Shield.renderer.enabled)
		{
			OrbitCamera (true);

		}
		else if(Input.GetKey(KeyCode.D) && !Input.GetKey (KeyCode.LeftShift) && !Shield.renderer.enabled)
		{
			OrbitCamera (false);
		}*/
		/*else if(Input.GetKeyDown (KeyCode.D) && Input.GetKey (KeyCode.LeftShift))
		{
			LDodging = true;
			startTime = Time.time;
			dodgeStart = transform.position;
			dodgeEnd = dodgeStart + (transform.forward - Quaternion.AngleAxis (-30, Vector3.up)*transform.forward)*2;
			startRot = transform.rotation;
			endRot = Quaternion.Euler (startRot.eulerAngles - Vector3.up*30);			
			StartCoroutine( PlayAnimationOnce("Dodging_right") );		
		}*/







		if(!tempAtk.animation.isPlaying)
		{
			tempAtk.renderer.enabled = false;
		}
		else
		{
			tempAtk.renderer.enabled = true;
		}
		/*else if(!animator.GetCurrentAnimatorStateInfo(0).IsName ("idle") && animator.IsInTransition(0))
		{
			currentAttack = 0;
		}


		if(LDodging)
		{
			float timeScale = (Time.time - startTime)*3;
			transform.position = Vector3.Slerp(dodgeStart, dodgeEnd, timeScale);
			transform.rotation = Quaternion.Lerp (startRot, endRot ,timeScale);
			if(Time.time - startTime >= 1)
			{
				LDodging = false;
			}
		}*/
	}

	void OrbitCamera(float direction)
	{
		Vector3 dist = transform.position - myCam.transform.position;
		float mag = Vector3.Magnitude (dist);
		transform.RotateAround (myCam.transform.position, Vector3.up, -direction*60*Time.deltaTime);
		FollowCam.offset = Quaternion.Euler (new Vector3 (0, -direction * 60 * Time.deltaTime, 0))*FollowCam.offset;
	}


	void AbilityRefresh()
	{
		onCoolDown = false;
	}

	public IEnumerator PlayAnimationOnce(string parameterName)
	{
		animator.SetBool( parameterName, true );
		yield return null;
		animator.SetBool( parameterName, false );
	}
}
