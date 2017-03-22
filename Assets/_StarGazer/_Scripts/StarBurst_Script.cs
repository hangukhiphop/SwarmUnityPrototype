using UnityEngine;
using System.Collections;

public class StarBurst_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Remove this effect after 2 seconds
        Invoke("RemoveSelf", 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void RemoveSelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
