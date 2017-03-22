using UnityEngine;
using System.Collections;

public class Nest : MonoBehaviour {

	public GameObject waveTimer;
	Timer waveTimerS;
	public GameObject spawnTimer;
	Timer spawnTimerS;

	public GameObject worker;
	GameObject workerPrefab;

	const float START_TIME = 0f;
	const float WAVE_TIME = 25f;
	const float SPAWN_TIME = 1f;
	public int waveSize;
	int waveProgress;

	public DamageTaker DT;

	// Use this for initialization
	void Start () 
	{
		spawnTimerS = ((GameObject)Instantiate (spawnTimer, transform.position, Quaternion.identity)).GetComponent<Timer>();
		spawnTimerS.Init(SPAWN_TIME, SpawnWorker, gameObject);

		waveTimerS = ((GameObject)Instantiate (waveTimer, transform.position, Quaternion.identity)).GetComponent<Timer>();
		waveTimerS.Init(WAVE_TIME, StartWave, gameObject);
		//waveSize = 5;
		waveProgress = 0;

		DT = gameObject.GetComponent<DamageTaker> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time > START_TIME)
		{
			waveTimerS.Begin (true);
		}
	}

	void StartWave()
	{
		spawnTimerS.Begin (true);
	}

	void SpawnWorker()
	{
		if(waveProgress < waveSize)
		{
			Instantiate (worker, new Vector3(transform.position.x, 0.7225316f, transform.position.z) + transform.forward*(transform.localScale.x + worker.transform.localScale.x)*.5f, transform.rotation);
			waveProgress++;
		}
		else
		{
			spawnTimerS.Stop();
			waveProgress = 0;
		}
	}

}
