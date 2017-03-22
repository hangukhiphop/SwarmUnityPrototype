using UnityEngine;
using System.Collections;

public class ThingsToTween : MonoBehaviour {
	
	
	public Light		groundLight;
	public Material		skyMat;
	
	public Color		startColor;
	public Color		targetColor = new Color(226.0f, 124.0f, 27.0f, 255.0f);
	public float		deltaMultiplier;
	
	// Use this for initialization
	void Start () {
		startColor = RenderSettings.skybox.color;
		
	}
	
	// Update is called once per frames
	void Update () {
	
		
		//iTween.ValueTo(groundLight.gameObject, iTween.Hash ("from", groundLight.intensity, "to", .56f, "time", 90.0f, "onupdatetarget", groundLight.gameObject, "onupdate", "ChangeValue") );
	
		startColor = Color.Lerp(startColor, targetColor, Time.deltaTime*deltaMultiplier);
		
		RenderSettings.skybox.color = startColor;
	
	}
	
	/*
	 * void ChangeValue (float newValue) {
		groundLight.intensity = newValue;
		Debug.Log (groundLight.intensity);
		
	}
	*/
	
}
