using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

	bool Player1Confirmed;
	bool Player2Confirmed;
	// Use this for initialization
	public GameObject p1Text;
	public GameObject p2Text;
	public GameObject p1Btn;
	public GameObject p2Btn;
	float timeTillStart;

	public AudioSource ConfirmSound;

	void Start () {
		Player1Confirmed = false;
		Player2Confirmed = false;
		p1Text = GameObject.Find ("P1 Ready");
		p2Text = GameObject.Find ("P2 Ready");
		p1Btn = GameObject.Find ("P1 A pressed");
		p2Btn = GameObject.Find ("P2 A pressed");
		timeTillStart = 3.0f;

		ConfirmSound = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("A2"))
		{
			Player1Confirmed = true;
			p1Text.renderer.enabled = true; 
			p1Btn.renderer.enabled = true; 
			ConfirmSound.Play ();
		}

		if(Input.GetButtonDown ("A1"))
		{
			Player2Confirmed = true;
			p2Text.renderer.enabled = true;
			p2Btn.renderer.enabled = true;
			ConfirmSound.Play ();
		}

		/*if (Input.GetButtonDown ("B2"))
		{
			Player1Confirmed = false;
			p1Text.renderer.enabled = false; 
			p1Btn.renderer.enabled = false;
			timeTillStart = 3.0f;
		}

		if (Input.GetButtonDown ("B1"))
		{
			Player2Confirmed = false;
			p2Text.renderer.enabled = false; 
			p2Btn.renderer.enabled = false; 
			timeTillStart = 3.0f;
		}*/

		if (Input.GetButtonDown ("AbilityX1") || Input.GetButtonDown ("AbilityX2"))
		{
			Application.LoadLevel (2);
		}

		if(Player1Confirmed && Player2Confirmed)
		{
			timeTillStart -= Time.deltaTime;

			if(timeTillStart <= 0.0f)
			{
				Application.LoadLevel (4);
			}
		}


	}
}
