using UnityEngine;
using System.Collections;

public class tweenLightColor : MonoBehaviour {
	public Color colorTo;
	public float lightTweenSpeedMod;
	
	private Light myLight;
	
	void Start () {
		myLight = GetComponent<Light>();
	}
	
	public IEnumerator tweenLight () {
		float progress = 0f;
		while (progress <= 1) {
			myLight.color = Color.Lerp(myLight.color, colorTo, progress);
			progress += Time.deltaTime * lightTweenSpeedMod;
			yield return null;
		}
	}
}
