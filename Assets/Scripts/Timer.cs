using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	GameObject Source;
	public delegate void TimerFunc();
	TimerFunc onTimer;
	bool ON;
	float counter;
	float interval;

	// Update is called once per frame
	public void Init(float time, TimerFunc timerFunction, GameObject source)
	{
		onTimer = timerFunction;
		ON = false;
		counter = 0;
		interval = time;
		Source = source;
	}


	public void Begin(bool execOnStart)
	{
		if (ON)
		{
			return;
		}		
		ON = true;
		if(execOnStart)
		{
			onTimer ();
		}
	}

	void FixedUpdate () 
	{
		if(!ON || Source == null)
		{
			return;
		}

		counter += Time.deltaTime;
		if(counter > interval)
		{
			onTimer();
			counter -= interval;
		}
	}

	public void Stop()
	{
		ON = false;
	}

	public void Reset()
	{
		counter = 0;
	}




}
