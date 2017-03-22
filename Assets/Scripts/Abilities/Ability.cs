using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 
using System.Collections.Generic;

public class Ability
{

	//Ability member variables
	private string Name;
	private float BaseDamage;
	private float Cooldown;
	private GameObject Spawn;
	private string ControlType;
	private float Range;

	//Map containing member variables
	private Dictionary<string, object> MemberMap;

	//Control interface for ability once potentially activated
	private GameObject AbilityControl;
	//For skillshots
	private static GameObject SkillShotAim = Resources.Load("Prefabs/Arrow", typeof(GameObject)) as GameObject;

	public Ability(string name)
	{
		//SkillShotAim = Resources.Load("Arrow.prefab", typeof(GameObject)) as GameObject;
		//Debug.Log (SkillShotAim);
		MemberMap = new Dictionary<string, object> ();
		//Unity acts weird if these variables are not initialized when casting things as object
		Name = "";
		BaseDamage = new float();
		Cooldown = new float ();
		Spawn = new GameObject ();
		ControlType = "";
		Range = new float();

		//Parse
		GetInfoFromTxt ();

		//Add results to MemberMap
		MemberMap.Add ("Name", Name);
		MemberMap.Add ("Base Damage", BaseDamage);
		MemberMap.Add ("Cooldown", Cooldown);
		MemberMap.Add ("Spawn", Spawn);
		MemberMap.Add ("Control", ControlType);
		MemberMap.Add ("Range", Range);

		//Get the corresponding GameObject for the control type found in the text file
		AbilityControlInit ();
	}

	//Parse ability data from txt file and store into variables
	void GetInfoFromTxt()
	{
		try
		{
			StreamReader reader = new StreamReader("Assets/Resources/AbilityDatabase.txt");
			string line = reader.ReadLine ();
			while(line != null)
			{		
				Name = (string)ProcessDataBlock (line, "Name", Name);
				BaseDamage = (float)ProcessDataBlock (line, "Base Damage", BaseDamage);
				Cooldown = (float)ProcessDataBlock(line, "Cooldown", Cooldown);
				Spawn = (GameObject)ProcessDataBlock (line, "Spawn", Spawn);
				ControlType = (string)ProcessDataBlock (line, "Control", ControlType);
				Range = (float)ProcessDataBlock (line, "Range", Range);

				line = reader.ReadLine ();
			}
		}
		catch
		{
			Debug.Log ("IOException");
		}
	}

	//Get Control element of MemberMap and set AbilityControl to the proper GameObject
	void AbilityControlInit ()
	{
		if(MemberMap["Control"].Equals ("Skill Shot"))
		{
			AbilityControl = SkillShotAim;
		}
	}

	public GameObject GetAbilityGuide()
	{
		return AbilityControl;
	}

	//Find object by name and type and load it into corresponding member variable
	//object is used so that the same function can be used for all types of variables
	object ProcessDataBlock(string line, string tagType, object vari)
	{	
		//formatting
		string tag = "[" + tagType + "]";

		if(line.StartsWith (tag))
		{
			//Remove tag from line and replace tag with result
			tag = (string)line.Replace (tag, "");	

			//Get type to sort values by type
			System.Type varType = vari.GetType ();
			if(varType == typeof(string))
			{
				return tag;
			}
			else if(varType == typeof(float))
			{
				return float.Parse (tag);
			}
			else if(varType == typeof(GameObject))
			{
				GameObject x = Resources.Load("Prefabs/" + tag, typeof(GameObject)) as GameObject;
				return x;
			}
		}

		//If tag does not match or object is not any of the above types, return original object value
		return vari;
	}

	public object GetMember(string type)
	{
		return MemberMap[type];
	}


}
