using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatManager : MonoBehaviour {

	int MyColony;

	Vector3 SpawnPoint;
	Quaternion SpawnRotation;
	Animator MyAnimator;

	public bool Dying;

	public GameObject TimerPrefab;
	Timer RespawnTimer;

	UIManager PlayerUI;


	//List of hero stats
	public int max_health;
	public int current_health;
	public float health_regen;
	public int max_mana;
	public float current_mana;
	public float mana_regen;
	public int physicalStrength;
	public int armor;
	public int spellPower;
	public int Immunity;
	public float movementSpeed;

	public GameObject HealthBar;
	GameObject[] healthbars;

	const int NUM_CAMERAS = 2;
	Camera[] Cameras;

	private AudioSource SoundSource;
	public AudioClip HurtSound;
	public AudioClip DeathSound;

	public Vector3 HBScale;
	public float HBHeight;

	// Use this for initialization
	void Start () 
	{
		MyColony = (int)(gameObject.tag [1] - '0');
		if(!gameObject.tag.StartsWith("I"))
		{
			//UIStats = Transform.Find ("KDW" + MyColony);
			//Debug.Log (UIStats);
			//KDWUI = UIStats.gameObject.GetComponent<KDWCounter> ();
			Cameras = new Camera[NUM_CAMERAS];		
			healthbars = new GameObject[NUM_CAMERAS];
			for(int i = 0; i < NUM_CAMERAS; i++)
			{
				Cameras[i] = GameObject.Find ("Camera" + (i + 1)).camera;
				healthbars[i] = (GameObject)Instantiate (HealthBar, Vector3.zero, Quaternion.identity);
				healthbars[i].transform.parent = transform;


			}
			if(MyColony == 1)
			{
				healthbars[0].GetComponent<GUITexture>().color = Color.cyan;
				healthbars[1].GetComponent<GUITexture>().color = Color.cyan;
			}
			else
			{
				healthbars[1].GetComponent<GUITexture>().color = Color.yellow;
				healthbars[0].GetComponent<GUITexture>().color = Color.yellow;
			}
			MyAnimator = gameObject.GetComponent<Animator> ();
		}
		else
		{			
			RespawnTimer = ((GameObject)Instantiate (TimerPrefab, transform.position, Quaternion.identity)).GetComponent<Timer>();	
			RespawnTimer.Init (5, Respawn, gameObject);
			SpawnPoint = transform.position;
			SpawnRotation = transform.rotation;

			
			PlayerUI = gameObject.GetComponent<UIManager> ();
			PlayerUI.SetHealth (max_health, max_health);
		}

		if(gameObject.name.Contains ("Worker"))
		{
			MyAnimator = gameObject.GetComponent<Animator> ();
		}
		else if(gameObject.name.Contains("Sentinel"))
		{
		}
		else if(gameObject.tag.Contains("N"))
		{
		}
		current_health = max_health;		
		//current_mana = max_mana;
		Dying = false;

		SoundSource = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(current_health <= 0 || transform.position.y < -10)
		{
			if(!Dying && gameObject.tag.StartsWith ("I"))
			{
				GetComponent<PlayerControl>().OnDeath ();
				PlayerUI.SetHealth (0, max_health);
				PlayerUI.AddDeath ();
				Dying = true;

				//KDWUI.IncrementDeathCount();


				MetricManagerScript.AddToDeaths(MyColony);
			}
			else if(gameObject.tag.StartsWith ("N"))
			{				
				SoundSource.Play ();

				Text Winner = GameObject.Find ("EndGameText" + (3 - MyColony)).GetComponent<Text>();
				Text Loser = GameObject.Find ("EndGameText" + MyColony).GetComponent<Text>();
				Winner.GetComponent<Text>().text = "VICTORY";
				Winner.GetComponent<Text>().color = Color.blue;
				Loser.GetComponent<Text>().text = "DEFEAT";
				Loser.GetComponent<Text>().color = Color.red;
				Invoke ("EndGame", 5);
			}
			else if(gameObject.tag.StartsWith ("W"))
			{
				PlayDeathAnimation();
				Destroy (gameObject, 1);
			}
			else if(gameObject.tag.StartsWith("S"))
			{
				Destroy (gameObject);
			}
			else if(Dying)
			{
				PlayerUI.SetHealth (max_health, max_health);
			}
			current_health = 0;


		}
		else if(current_health < max_health)
		{
			current_health += (int)(health_regen*Time.deltaTime);
		}
		else
		{
			current_health = max_health;
		}

		/*if(current_mana < max_mana)
		{
			current_mana += mana_regen;
		}
		else if(current_mana > max_mana)
		{
			current_mana = max_mana;
		}*/

		if (healthbars != null) 
		{
			for(int i = 0; i < Cameras.Length; i++)
			{
				if(Cameras[i] == null)
				{
					return;
				}
				Vector3 pos = Cameras[i].WorldToScreenPoint (transform.position + HBHeight*Vector3.up) - Cameras[i].pixelRect.yMin*Vector3.up - Cameras[i].pixelRect.xMin*Vector3.right;
				float ratio = 1f/(Cameras[i].WorldToViewportPoint (transform.position + HBHeight*Vector3.up).z);
				if(ratio > 0 && ratio < 100)
				{
					healthbars[i].layer = LayerMask.NameToLayer("Cam" + (i + 1) + "Layer");
					healthbars[i].transform.GetComponent<GUITexture>().enabled = true;
					healthbars[i].transform.localScale = new Vector3(HBScale.x*(int)current_health/max_health, HBScale.y, 0)*ratio;
					healthbars[i].transform.position = new Vector3(pos.x/Cameras[i].pixelWidth - .5f*ratio*(max_health - (int)current_health)/max_health, pos.y/Cameras[i].pixelHeight, 0);
				}
				else
				{
					healthbars[i].transform.GetComponent<GUITexture>().enabled = false;
				}
			}
		}
	}

	public void ComputeDamage(GameObject source, int damage, bool physical)
	{
		if(physical)
		{
			damage = (int)(damage/(1f + armor/100f));
		}
		else
		{
			damage = (int)(damage/(1f + spellPower/100f));
		}
		current_health -= damage;

		if(gameObject.tag.StartsWith("I"))
		{
			PlayerUI.SetHealth (current_health, max_health);
			if(current_health <= 0 && source.tag.StartsWith ("I"))
			{
				source.GetComponent<UIManager>().AddKill();
			}
		}
		else if(gameObject.tag.StartsWith ("W"))
		{
			if(current_health <= 0 && source.tag.StartsWith ("I"))
			{
				source.GetComponent<UIManager>().AddExecution();
			}
		}
	}

	void PlayDeathAnimation()
	{
		MyAnimator.SetInteger ("State", 3);
	}

	public void Respawn()
	{
		transform.position = SpawnPoint;
		transform.rotation = SpawnRotation;
		GameObject.Find ("Camera" + MyColony).transform.position = transform.position - 6 * transform.forward + 4 * Vector3.up;	
		current_health = max_health;
		//pc.SetControlState(PlayerControl.ControlType.None);
		RespawnTimer.Stop ();
	}

	void EndGame()
	{
		Application.LoadLevel (0);
	}


}
