using UnityEngine;
using System.Collections;

public class SkyLerp : MonoBehaviour {
	public Color targetColor;
	public float speedCntrl = .5f;
	// Use this for initialization
	void Start () {
		StartCoroutine(lerpSky());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator lerpSky () {
		float progress = 0f;
		while (progress <= 1) {
			RenderSettings.skybox.color = Color.Lerp(RenderSettings.skybox.color, targetColor, progress);
			progress += Time.deltaTime * speedCntrl;
			Debug.Log(progress.ToString());
			yield return null;
		}
	}
}
