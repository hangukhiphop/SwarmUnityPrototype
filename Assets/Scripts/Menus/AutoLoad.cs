using UnityEngine;
using System.Collections;

public class AutoLoad : MonoBehaviour {

	float loadTimer = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				loadTimer -= Time.deltaTime;
			
				if (loadTimer <= 0.0f) {
						Application.LoadLevel (1);
				}
		}
}
