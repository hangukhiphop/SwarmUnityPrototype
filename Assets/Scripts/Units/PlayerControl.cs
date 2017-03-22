using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

	//Identifies which control set to use
	int Player;
	
	private GameObject MyCamera;
	FollowPlayer FollowCam;

	//Mutually exclusive control states
	public enum ControlType {None, Moving, Attack, Shield, Ability, Dead};
	public ControlType CurrentControl;

	//Controls the animation state for the player
	Animator MyAnimator; //States: 0-Idle, 1-Run, 2-AbilityX, 3-Punch, 4-Block

	//Ability-related components
	AbilityManager MyAbilities;

	StatManager MyStatManager;
	DamageTaker MyDamageTaker;

	public GameObject TimerPrefab;
	Timer RespawnTimer;
	Vector3 SpawnPoint;
	Quaternion SpawnRotation;



	// Use this for initialization
	void Start () 
	{
		Player = (int)(gameObject.tag [gameObject.tag.Length - 1] - '0');
		//Get the followcam for this player
		MyCamera = GameObject.Find ("Camera" + Player);
		FollowCam = MyCamera.GetComponent<FollowPlayer> ();

		//Initialize to no input
		CurrentControl = ControlType.None;

		//Get Animator component
		MyAnimator = GetComponent<Animator> ();

		MyStatManager = GetComponent<StatManager> ();
		MyDamageTaker = GetComponent<DamageTaker> ();

		//Get AbilityManager Script for this character
		MyAbilities = GetComponent<AbilityManager> ();

		RespawnTimer = ((GameObject)Instantiate (TimerPrefab, transform.position, Quaternion.identity)).GetComponent<Timer>();	
		RespawnTimer.Init (5, Respawn, gameObject);
		SpawnPoint = transform.position;
		SpawnRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandleInput ();
	}

	void HandleInput()
	{
		if(CurrentControl != ControlType.Dead)
		{
			ListenLeftJoystickInput ();			
			ListenAbilityInput ();
			ListenResetInput ();
			//ListenPunchInput ();
			ListenStingInput ();
			ListenShieldInput ();
		}
		else
		{
			Despawn();
		}		
		GetComponent<Rigidbody> ().velocity = Vector3.zero;
	}

	void Despawn()
	{
		if(MyAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Die") && MyAnimator.GetNextAnimatorStateInfo (0).IsName ("Idle"))
		{	
			gameObject.active = false;
		}
		else
		{
			gameObject.active = true;
		}
	}

	void CancelJoystick()
	{
		CurrentControl = ControlType.None;
	}

	void ListenLeftJoystickInput()
	{
		float HorizontalAxis = Input.GetAxis ("Horizontal" + Player);
		float VerticalAxis = Input.GetAxis ("Vertical" + Player);

		if(MyAbilities.GetCurrentGuide() != null)
		{
			MyAbilities.SetCurrentGuide(HorizontalAxis, VerticalAxis);
		}
		else if((HorizontalAxis != 0 || VerticalAxis != 0))
		{
			if(CurrentControl != ControlType.Ability && CurrentControl != ControlType.Shield)
			{
				MovePlayer (HorizontalAxis, VerticalAxis);
			}
		}
		else if(CurrentControl != ControlType.Attack)
		{
			SetAnimState (0);
			CancelJoystick();
		}
	}

	void MovePlayer(float horizontal, float vertical)
	{
		CurrentControl = ControlType.Moving;
		SetAnimState (1);

		if(!MyAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Sting"))
		{
			transform.rotation = Quaternion.Euler (new Vector3(0, MyCamera.transform.eulerAngles.y, 0))*Quaternion.LookRotation(new Vector3(horizontal, 0, vertical));
			Vector3 deltaForward = transform.forward*MyStatManager.movementSpeed*Time.deltaTime;
			deltaForward = Vector3.Project (deltaForward, new Vector3(MyCamera.transform.forward.x, 0, MyCamera.transform.forward.z));
			transform.position += deltaForward;
			MyCamera.transform.position += deltaForward;

			if(horizontal != 0)
			{
				OrbitCamera (-horizontal);
			}
		}
	}

	void OrbitCamera(float direction)
	{
		Vector3 dist = transform.position - MyCamera.transform.position;
		float mag = Vector3.Magnitude (dist);
		transform.RotateAround (MyCamera.transform.position, Vector3.up, -direction*60*Time.deltaTime);
		FollowCam.offset = Quaternion.Euler (new Vector3 (0, -direction * 60 * Time.deltaTime, 0))*FollowCam.offset;
	}

	//Reset the game
	void ListenResetInput()
	{
		Text exit = GameObject.Find ("ExitText").GetComponent<Text> ();
		if(Input.GetButtonDown ("Start" + Player))
		{
			if(exit.text.Equals (""))
			{			
				exit.text = "Player " + Player + " has surrendered \n Press start to confirm. \n Either player press B to cancel.";
			}
			else if(!exit.text.StartsWith ("Player " + Player))
			{
				Application.LoadLevel (0);
			}
		}
		else if(Input.GetButtonDown("B" + Player))
		{
			if(!exit.text.Equals(""))
			{
				exit.text = "";
			}
		}
	}

	//Listen for Attack input if conditions are satisfied
	/*void ListenPunchInput()
	{
		if(CurrentControl != ControlType.Moving && CurrentControl != ControlType.Shield && CurrentControl != ControlType.Ability)
		{
			if(Input.GetButtonDown ("RB" + Player))
			{
				CurrentControl = ControlType.Attack;
				SetAnimState (3);

			}
		}
	}*/

	void ListenStingInput()
	{
		if(CurrentControl != ControlType.Shield && CurrentControl != ControlType.Ability)
		{
			if(Input.GetButtonDown ("RB" + Player))
			{
				CurrentControl = ControlType.Attack;
				StartCoroutine(PlayAnimationOnce(5, 0));
				MetricManagerScript.AddToAttacks ();
			}
		}
	}
	
	//Listen for Shield input if conditions are satisfied
	void ListenShieldInput()
	{
		if(CurrentControl != ControlType.Moving && CurrentControl != ControlType.Attack && CurrentControl != ControlType.Ability)
		{
			if(Input.GetButton("LB" + Player))
			{
				SetAnimState (4);			
				MyDamageTaker.Vulnerable = false;
				CurrentControl = ControlType.Shield;
				MetricManagerScript.AddToShields ();
			}
			if(Input.GetButtonUp ("LB" + Player))
			{
				MyDamageTaker.Vulnerable = true;
			}
		}
	}

	void ListenAbilityInput()
	{
		if(MyAbilities.GetCooldown(0) == 0)
		{
			if(Input.GetAxis("Trigger" + Player) < 0 &&  MyAbilities.GetCurrentGuide () == null && CurrentControl != ControlType.Shield && CurrentControl != ControlType.Attack)
			{
				MyAbilities.ActivateAbilityGuide(Player, MyCamera, MyAbilities.GetAbilityName(0));
				SetAnimState (0);
				CurrentControl = ControlType.Ability;
			}
			if(Input.GetAxis ("Trigger" + Player) == 0 && MyAbilities.GetCurrentGuide() != null)
			{
				StartCoroutine(PlayAnimationOnce(2, 0));
				MyAbilities.InstantiateAbility(0);
				transform.eulerAngles = new Vector3(0, MyAbilities.GetCurrentGuide().transform.eulerAngles.y + 90, 0);
				MyAbilities.DeactivateAbilityGuide();
			}
		}
	}

	public void SetControlState(ControlType whichState)
	{
		CurrentControl = whichState;
	}

	public void SetAnimState(int whichState)
	{
		MyAnimator.SetInteger ("AnimState", whichState);
	}

	public IEnumerator PlayAnimationOnce(int before, int after)
	{
		MyAnimator.SetInteger( "AnimState", before);
		yield return null;
		MyAnimator.SetInteger( "AnimState", after);
	}

	public void OnDeath()
	{
		StartCoroutine (PlayAnimationOnce (6, 0));
		CurrentControl = ControlType.Dead;	
		if(MyAbilities.GetCurrentGuide () != null)
		{
			MyAbilities.DeactivateAbilityGuide();
		}
		RespawnTimer.Begin (false);	
	}

	
	public void Respawn()
	{
		transform.position = SpawnPoint;
		transform.rotation = SpawnRotation;
		MyCamera.GetComponent<FollowPlayer> ().InitCamera ();
		MyStatManager.current_health = MyStatManager.max_health;
		CurrentControl = ControlType.None;
		gameObject.active = true;
		MyStatManager.Dying = false;
		RespawnTimer.Stop ();
	}


}
