using UnityEngine;
using System.Collections;

public class EyeLid_Script : MonoBehaviour {
	

	
	// Use this for initialization
	void Start () {
		this.GetComponent<MeshRenderer>().enabled=true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (AudioController_Script.startProcedure == true) {
			if (this.gameObject.tag == "upLid") {
				iTween.MoveBy(this.gameObject, iTween.Hash("y", 1.0f, "time", 10.0f, "easetype", iTween.EaseType.linear, "islocal", true ));
			}
			else if (this.gameObject.tag == "downLid") {
				iTween.MoveBy(this.gameObject, iTween.Hash("y", -1.0f, "time", 10.0f, "easetype", iTween.EaseType.linear, "islocal", true ));
			}
		}
		
		if (AudioController_Script.endProcedure == true) {
			if (this.gameObject.tag == "upLid") {
				iTween.MoveBy(this.gameObject, iTween.Hash("y", -1.0f, "time", 15.0f, "easetype", iTween.EaseType.linear, "islocal", true ));
			}
			else if (this.gameObject.tag == "downLid") {
				iTween.MoveBy(this.gameObject, iTween.Hash("y", 1.0f, "time", 15.0f, "easetype", iTween.EaseType.linear, "islocal", true ));
			}
			
			StartCoroutine (SwitchScene (18f));
		}
	
	}
	
		
	private IEnumerator SwitchScene (float seconds)
	{
        
		yield return new WaitForSeconds(seconds);
		
		Application.LoadLevel(1);
	}
	
	
	
}