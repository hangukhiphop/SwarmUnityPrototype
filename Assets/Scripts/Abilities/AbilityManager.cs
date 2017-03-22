using UnityEngine;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour
{
	public string[] AbilityNames;
	private Dictionary<string, Ability> AbilitySet;

	public GameObject Arrow;
	private GameObject Aimer;
	private AbilityAimer AimerScript;

	public GameObject DronePrefab;

	private float[] AbilityCooldowns;
	private float[] ActiveCooldowns;

	UIManager PlayerUI;

	// Use this for initialization
	public void Start()
	{
		AbilityCooldowns = new float[AbilityNames.Length];
		ActiveCooldowns = new float[AbilityNames.Length];

		AbilitySet = new Dictionary<string, Ability>();
		for(int i = 0; i < AbilityNames.Length; i++)
		{
			AddAbility (AbilityNames[i]);
			AbilityCooldowns[i] = 6;//(float)AbilitySet[AbilityNames[i]].GetMember ("Cooldown");
			ActiveCooldowns[i] = 0;
		}

		PlayerUI = GetComponent<UIManager> ();
	}

	public void Update()
	{
		for(int i = 0; i < AbilitySet.Count; i++)
		{
			float newTime = ActiveCooldowns[i] - Time.deltaTime;
			if(newTime >= 0)
			{
				ActiveCooldowns[i] = newTime;
			}
			else
			{
				ActiveCooldowns[i] = 0;
			}
			
			PlayerUI.SetAbility ((int)ActiveCooldowns [i]);
		}
	}

	private void AddAbility(string name)
	{
		AbilitySet.Add (name, new Ability(name));
	}

	public string GetAbilityName(int index)
	{
		return AbilityNames[index];
	}

	public float GetCooldown(int index)
	{
		return ActiveCooldowns [index];
	}

	public void InstantiateAbility(int index)
	{
		Vector3 spawnPoint = transform.position + transform.localScale.y * Vector3.up + transform.forward;
		GameObject ability = (GameObject)Instantiate (
			//(GameObject)AbilitySet [AbilityNames [index]].GetMember ("Spawn"), 
			DronePrefab,
			spawnPoint, Aimer.transform.rotation*Quaternion.Euler (0, 90, -90)); 
		ability.GetComponent<SkillShot> ().damage_source = gameObject;
		ActiveCooldowns [index] = AbilityCooldowns[index];
	}


	public void ActivateAbilityGuide(int player, GameObject playerCamera, string whichAbility)
	{
		//Aimer = (GameObject)Instantiate(AbilitySet[whichAbility].GetAbilityGuide (), transform.position, Quaternion.identity);
		Aimer = (GameObject)Instantiate(Arrow, transform.position, Quaternion.identity);
		Aimer.transform.parent = transform;

		Aimer.layer = LayerMask.NameToLayer("Cam" + (player) + "Layer");

		AimerScript = Aimer.GetComponent<AbilityAimer> ();
		AimerScript.Initialize (playerCamera, 10);//(float)AbilitySet[whichAbility].GetMember ("Range"));
	}
	
	public void DeactivateAbilityGuide()
	{
		if(Aimer != null)
		{
			Destroy (Aimer.gameObject);
		}
	}

	public GameObject GetCurrentGuide()
	{
		return Aimer;
	}

	public void SetCurrentGuide(float horizontal, float vertical)
	{
		AimerScript.SetAxes (horizontal, vertical);
	}




}
