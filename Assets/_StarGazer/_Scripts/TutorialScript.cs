using UnityEngine;
using System.Collections;


//A sphere to only simulate sunset effect
public class TutorialScript : MonoBehaviour {
	
	public Texture black;
	public GameObject tuto_1;
	public GameObject tuto_2;
	public GameObject tuto_3;
	
	private float alphaFadeValue = 1f;
	
	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public IEnumerator StartTutorial()
    {
        print("Starting Tutorial sequnece");
		
		iTween.ValueTo(this.gameObject, iTween.Hash("name", "Tuto1", "from", 0, "to", 0.5f, "time", 4f, "easetype", iTween.EaseType.linear, "onupdate", "SetTuto1Alpha"));
		yield return new WaitForSeconds(4.0f);
		iTween.ValueTo(this.gameObject, iTween.Hash("name", "Tuto2", "from", 0, "to", 0.5f, "time", 4f, "easetype", iTween.EaseType.linear, "onupdate", "SetTuto2Alpha"));
		yield return new WaitForSeconds(4.0f);
		iTween.ValueTo(this.gameObject, iTween.Hash("name", "Tuto3", "from", 0, "to", 0.5f, "time", 4f, "easetype", iTween.EaseType.linear, "onupdate", "SetTuto3Alpha"));
        
        yield return null;
    }
	
	public IEnumerator EndTutorial(){
		print ("EndTutorial called");

		iTween.StopByName("Tuto1");
		iTween.StopByName("Tuto2");
		iTween.StopByName("Tuto3");
		StopCoroutine("StartTutorial");
		
		iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to", 0, "time", 4f, "easetype", iTween.EaseType.linear, "onupdate", "SetTuto1Alpha"));
		iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to", 0, "time", 4f, "easetype", iTween.EaseType.linear, "onupdate", "SetTuto2Alpha"));
		iTween.ValueTo(this.gameObject, iTween.Hash("from", 0.5f, "to", 0, "time", 4f, "easetype", iTween.EaseType.linear, "onupdate", "SetTuto3Alpha"));

		
		yield return new WaitForSeconds(4f);
		
		AudioController_Script.startProcedure = false;
		
	}
	
	void OnGUI(){
		
		//GUI.color = new Color(0, 0, 0, alphaFadeValue);
		//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
		
	}
	
    public void SetTuto1Alpha(float value)
    {	
        tuto_1.renderer.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, value));
    }
	
	public void SetTuto2Alpha(float value)
    {	
        tuto_2.renderer.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, value));
    }

	public void SetTuto3Alpha(float value)
	{	
		tuto_3.renderer.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, value));
	}
	
	public void SetBlackAlpha(float value)
    {	
        alphaFadeValue = value;
    }
	
	
}
