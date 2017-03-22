using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {
	public float[] timeGate = new float[4];
	public float postEffectSpeedMod = .05f;
	
	private GameObject[] buildings;
	private GameObject[] trees;
	private GameObject[] animals;
	private GameObject[] tweenableLights;
	public GameObject[] fiyah;
	
	public Color targetColor;
	public float speedCntrl = .5f;
	
	private bool[] sceneTrigger = new bool[4];
	private GameObject[] OVRCams;
	void Start () {
		OVRCams = GameObject.FindGameObjectsWithTag("OVR");
		foreach (GameObject ovrCam in OVRCams) 
			Debug.Log(ovrCam.name);
		foreach (GameObject thisFire in fiyah)
			thisFire.SetActive(false);
		tweenableLights = GameObject.FindGameObjectsWithTag("tweenLight");
	}
	
	void Update () {
		if (!sceneTrigger[0] && Time.time >= timeGate[0]) {
			phaseOne();
		} /*else if (!sceneTrigger[1] && Time.time >= timeGate[1]) {
			Debug.Log("Start Phase 2!!! " + Time.time);
			sceneTrigger[1] = true;
		} else if (!sceneTrigger[2] && Time.time >= timeGate[2]) {
			Debug.Log("Start Phase 3!!! " + Time.time);
			sceneTrigger[2] = true;
		}
		else if (!sceneTrigger[3] && Time.time >= timeGate[3]) {
			Debug.Log("Start Phase 3!!! " + Time.time);
			sceneTrigger[3] = true;
		}*/
		if (Input.GetKeyDown(KeyCode.Space))
			endGame();
	}
	
	void phaseOne () {
		Debug.Log("Start Phase 1!!! " + Time.time);
		sceneTrigger[0] = true;
		StartCoroutine (desaturate(OVRCams[0].GetComponent<ColorCorrectionCurves>()));
		StartCoroutine(desaturate(OVRCams[1].GetComponent<ColorCorrectionCurves>()));
		StartCoroutine(vingetter(OVRCams[0].GetComponent<Vignetting>()));
		StartCoroutine(vingetter(OVRCams[1].GetComponent<Vignetting>()));
		StartCoroutine(lerpSky());
		foreach (GameObject fire in fiyah) 
			fire.SetActive(true);
		foreach (GameObject myLight in tweenableLights)
			StartCoroutine(myLight.GetComponent<tweenLightColor>().tweenLight());
	}
	
	IEnumerator desaturate (ColorCorrectionCurves ourCurve) {
		/*float timeTo = 3f;
		while (timeTo > 0) {
			Mathf.SmoothStep(ourCurve.saturation, .5f, timeTo);
			timeTo -= Time.deltaTime;
			Debug.Log(timeTo);
			yield return null;
		}*/
		float progress = 0f;
		while (progress <= 1) {
			ourCurve.saturation = Mathf.Lerp(ourCurve.saturation, .5f, progress);
			progress += Time.deltaTime * postEffectSpeedMod;
			Debug.Log("saturate " + progress.ToString());
			yield return null;
		}
	}
	
	IEnumerator vingetter (Vignetting ourVin) {
		float progress = 0f;
		while (progress <= 1) {
			ourVin.intensity = Mathf.Lerp(ourVin.intensity, 4f, progress);
			progress += Time.deltaTime * postEffectSpeedMod;
			Debug.Log("vign " + progress.ToString());
			yield return null;
		}
	}
	
	IEnumerator bloomer (Bloom ourBloom) {
		float progress = 0f;
		while (progress <= 1) {
			ourBloom.bloomIntensity = Mathf.Lerp(ourBloom.bloomIntensity, 2f, progress);
			progress += Time.deltaTime * postEffectSpeedMod;
			Debug.Log("vign " + progress.ToString());
			yield return null;
		}
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
	
	static public void endGame () {
		Application.LoadLevel(1);
	}
}
