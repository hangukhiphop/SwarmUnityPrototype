using UnityEngine;
using System.Collections;

public class GoToScene : MonoBehaviour {

	public string m_changeToThisLevel;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Application.LoadLevel(m_changeToThisLevel);
		}
	}
}
