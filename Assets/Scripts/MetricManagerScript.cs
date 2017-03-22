using UnityEngine;
using System.Collections;
using System.IO;

public class MetricManagerScript : MonoBehaviour {
	
	string createText = "";
	
	public static int timeTilP1AttackedWorker,timeTilP2AttackedWorker,p1Deaths,p2Deaths,attacked,usedShield,usedAbility,duration ;
	public float TotalGameTime;
	
	void Start () 
	{
		timeTilP1AttackedWorker = 0;
		timeTilP2AttackedWorker = 0;
		p1Deaths = 0;
		p2Deaths = 0;
		attacked = 0;
		usedShield = 0;
		usedAbility = 0;
		TotalGameTime = 0;
	}
	void Update () 
	{
		TotalGameTime += Time.deltaTime;
	}
	
	//When the game quits we'll actually write the file.
	void OnApplicationQuit(){
		GenerateMetricsString ();
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString ();//Get the time to tack on to the file name
		time = time.Replace ("/", "-"); //Replace slashes with dashes, because Unity thinks they are directories..
		time = time.Replace (":", "-");
		string reportFile = "Swarm_Metrics_" + time + ".txt"; 
		//createText += GenerateMetricsString ();
		File.WriteAllText (reportFile, createText);
		//In Editor, this will show up in the project folder root (with Library, Assets, etc.)
		//In Standalone, this will show up in the same directory as your executable
	}
	
	string GenerateMetricsString(){
		createText = 
			"\nTime it took player 1 to attack workers: " + timeTilP1AttackedWorker + "\n";
		createText += "Time it took player 2 to attack workers: " + timeTilP2AttackedWorker + 
			"\nPlayer 1 died " + p1Deaths + " times" +
				"\nPlayer 2 died " + p2Deaths + " times" +
				"\nAttack function was used " + attacked + " times" +
				"\nShield function was used " + usedShield + " times" +
				"\nAbility was used " + usedAbility + " times" +
				"\nGame time: " + TotalGameTime;

		return createText;
	}
	
	//Add to your set metrics from other classes whenever you want
	public void AddToP1Time(int P1Timer){
		timeTilP1AttackedWorker = P1Timer;
		//add timestamp at which player 1 attacked a worker
	}
	
	public void AddToP2Time(int P2Timer){
		timeTilP2AttackedWorker = P2Timer;
		//add timestamp at which player 2 attacked a worker
	}
	
	public static void AddToDeaths(int player){
		if(player == 1)
		{
			p1Deaths++;
		}
		else if(player == 2)
		{
			p2Deaths++;
		}
		//increment each time player 1 dies
	}
	
	public static void AddToAttacks(){
		attacked++;
		//increment each time either player attacks
	}
	
	public static void AddToShields(){
		usedShield++;
		//increment each time either player uses shield
	}
	
	public static void AddToAbilities()
	{
		usedAbility++;
		//increment each time either player uses the ability
	}

}