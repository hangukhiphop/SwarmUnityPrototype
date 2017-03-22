using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityButtons : MonoBehaviour {

	public GameObject myPlayer;
	Combat tempCombat;
	public Text X_AbilityText;
	// Use this for initialization
	void Start () 
	{
		tempCombat = myPlayer.gameObject.GetComponent<Combat> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//X_AbilityText.text = tempCombat.GetCooldownStatus ();
	}


}
