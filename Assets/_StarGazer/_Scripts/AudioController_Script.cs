using UnityEngine;
using System.Collections;

public class AudioController_Script : MonoBehaviour {
	
	public bool				stoppedWind=false;
	public bool				playingStaraza=false;
	
	public AudioClip		windSounds;
	public AudioClip		Staraza;
	
	public static bool		endProcedure=false;
	public static bool		startProcedure=false;
	
	TutorialScript tutoScript;
	SkySphere_Script skyScript;
	StarSphere_Script starSphereScript;
	GameObject skyDome;


	// Use this for initialization
	void Start () {
		stoppedWind=false;
		playingStaraza=false;
		endProcedure=false;
		startProcedure=false;

		tutoScript = Transform.FindObjectOfType(typeof(TutorialScript)) as TutorialScript;
		skyScript = Transform.FindObjectOfType(typeof(SkySphere_Script)) as SkySphere_Script;
		starSphereScript = Transform.FindObjectOfType(typeof(StarSphere_Script)) as StarSphere_Script;
		skyDome = GameObject.Find ("Sky Dome");
		print ("tutoScript : " + tutoScript);
		tutoScript.StartCoroutine("StartTutorial");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if((Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)) && startProcedure == false){
			print ("Starting Procedure");
			startProcedure = true;
			
			this.audio.clip = windSounds;
			this.audio.loop = true;
			this.audio.Play();
			
			skyScript.StartCoroutine("Sunset");
			skyDome.SendMessage("Sunset");
			tutoScript.StartCoroutine("EndTutorial");
			starSphereScript.ReserveOneStarToAwake(20f);
		}
		
		if (Star_Script.AwakenedStars > 1 && stoppedWind == false) {
			this.audio.Stop ();
			stoppedWind=true;
		}
		
		if (Star_Script.AwakenedStars > 1 && stoppedWind == true && playingStaraza == false) {
			this.audio.clip = Staraza;
			this.audio.loop = false;
			this.audio.Play();
			playingStaraza=true;
		}
		
		
		if ((this.audio.clip == Staraza && this.audio.isPlaying == false) || Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonUp(1)) {
			print ("Ending Procedure");
			endProcedure = true;
		}
	
		
	}

}
