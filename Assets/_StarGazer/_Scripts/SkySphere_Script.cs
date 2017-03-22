using UnityEngine;
using System.Collections;


//A sphere to only simulate sunset effect
public class SkySphere_Script : MonoBehaviour {

    public Light light1;
	public Light light2;
	public Light light3;
	public Light light4;
	
	
	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
        Screen.showCursor = false;
	}

    public IEnumerator Sunset()
    {
		
        print("Starting sunset sequnece");

        iTween.RotateTo(this.gameObject, iTween.Hash("rotation", new Vector3(0, 0, -60), "time", 120.0f, "easetype", iTween.EaseType.linear));
        
        yield return new WaitForSeconds(1.0f);

        iTween.ValueTo(this.gameObject, iTween.Hash("from", light1.intensity, "to", 0, "time", 30f, "easetype", iTween.EaseType.linear, "onupdate", "SetLight1Intensity"));
		iTween.ValueTo(this.gameObject, iTween.Hash("from", light2.intensity, "to", 0, "time", 10f, "easetype", iTween.EaseType.linear, "onupdate", "SetLight2Intensity"));
		iTween.ValueTo(this.gameObject, iTween.Hash("from", light3.intensity, "to", 0, "time", 80f, "easetype", iTween.EaseType.linear, "onupdate", "SetLight3Intensity"));
		iTween.ValueTo(this.gameObject, iTween.Hash("from", light4.intensity, "to", 0, "time", 120f, "easetype", iTween.EaseType.linear, "onupdate", "SetLight4Intensity"));

        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(this.transform.position.x, this.transform.position.y - 80f, this.transform.position.z), "time", 60.0f, "easetype", iTween.EaseType.linear));

        yield return null;
    }

    public void SetLight1Intensity(float value)
    {	
        light1.intensity = value;
    }
	
	public void SetLight2Intensity(float value)
    {	
        light2.intensity = value;
    }
	
	public void SetLight3Intensity(float value)
    {	
        light3.intensity = value;
    }
	
	public void SetLight4Intensity(float value)
    {	
        light4.intensity = value;
    }

}
