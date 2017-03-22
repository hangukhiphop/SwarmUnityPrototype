using UnityEngine;
using System.Collections;

public class StopTimeAtValue : MonoBehaviour {

	public float m_timeThreshold;
	public float m_newDayDuration;

	private bool m_slowedTime;

	TOD_Sky m_mySky;
	

	// Use this for initialization
	void Start () {
		m_slowedTime=false;
		m_mySky=this.GetComponent<TOD_Sky>();
	}

	// Update is called once per frame
	void Update () {
		if (m_mySky.Cycle.Hour>m_timeThreshold && !m_slowedTime) {
			m_slowedTime=true;
			this.GetComponent<TOD_Time>().ProgressTime=false;
			//this.GetComponent<TOD_Time>().DayLengthInMinutes=m_newDayDuration;
		}
	}

	void Sunset(){
		this.GetComponent<TOD_Time>().ProgressTime=true;
	}
}
