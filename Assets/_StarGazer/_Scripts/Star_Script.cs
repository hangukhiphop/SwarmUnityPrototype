using UnityEngine;
using System.Collections;



public class Star_Script : MonoBehaviour {
	
	static public int		AwakenedStars=0;
	
	public AudioClip[]		WarmUpTones;
	public AudioClip[]		AwakenTones;
	public AudioClip[]		StarTones;

	
    public enum StarState
    {
        sleeping,   //invisible, does not react to anything
        awake,      //visible as a small dot, gathers energy when stared & evolves into a singing star
        singing     //visible as a large dot, emits particles & sound when stared
    }

    public StarState STATE;

    private float _starEnergy = 0f;     //a star's current energy timer
    private float _starEnergyMax = 3f;  //Time it takes for a star to burst

    private StarSphere_Script SSS;

    private float _singReloadTimer = _singReloadInterval;
    private const float _singReloadInterval = 2.0f;

    private const float _awakeNearbyStarRadius = 30f;

    //starEnergy for public access 0 ~ 100
    public int starEnergy
    {
        get
        {
            return (int)(_starEnergy / _starEnergyMax * 100f);
        }
        set
        {
            _starEnergy = (int)(value * _starEnergyMax * 0.01f);
        }
    }

    public GameObject Prefab_StarBurst; //Burst effect to be instantiated

    
	// Use this for initialization
	void Awake () {
                
        ChangeState(StarState.sleeping);
        
        SSS = FindObjectOfType(typeof(StarSphere_Script)) as StarSphere_Script;
	}
	
	
	// Update is called once per frame
	void Update () {


        switch (STATE)
        {
            case StarState.sleeping:
                
                break;
            case StarState.awake:
                if (_starEnergy > 0)
                {
                    _starEnergy -= Time.deltaTime;
                }

                //change particle's emission behavior based on starEnergy
                particleSystem.startSpeed = 5f + starEnergy * 0.2f;
                particleSystem.emissionRate = starEnergy + 10f;
                break;

            case StarState.singing:

                //resetting reload timer
                if(_singReloadTimer > 0){
                    _singReloadTimer -= Time.deltaTime;
                }

                break;
        }
        
	}

    //While a player looks at this star, this function is continuously called
    public void Glance()
    {
        switch (STATE)
        {
            case StarState.sleeping:
                                
                break;
            case StarState.awake:
                //Gather energy and evolve into next stage when fed with enough energy
				print ("Getting enerhy");
				
                _starEnergy += Time.deltaTime * 3f;
			
				if (starEnergy >= 15) {
					this.audio.PlayOneShot(WarmUpTones[Random.Range (0,WarmUpTones.Length)], 0.25f);
				}
			
                if (starEnergy >= 100) {
					this.audio.Stop ();
					ChangeState(StarState.singing);
					this.audio.PlayOneShot(AwakenTones[Random.Range (0,AwakenTones.Length)], 1.0f);
				}
			break;

            case StarState.singing:
			
				AwakenedStars++;
			
                //Sing (Emit particles & sound)
                //Timer to make sure it does not sing 60 times a sec
                if (_singReloadTimer <= 0)
                {
					this.audio.PlayOneShot(StarTones[Random.Range (0,StarTones.Length)], 1.0f);
                    EmitBurst();
                    _singReloadTimer = _singReloadInterval;
                }

                break;
        }
    }

    
    //To do: Will change burst's color and size dependent on the parent's size & color
    private void EmitBurst()
    {

        GameObject burst = GameObject.Instantiate(Prefab_StarBurst, transform.position, Quaternion.identity) as GameObject;
        burst.transform.parent = transform.parent;

    }
    
    //Things that happen once when a star changes to different state.
    //This is where most star sound effect should be played
    public void ChangeState(StarState _STATE)
    {
        //if the parameter state is current or previous state, ignore it
        if ((int)_STATE <= (int)STATE) return;


        //print("ChangeState to " + _STATE);
        STATE = _STATE;

        switch (STATE)
        {
            case StarState.sleeping:
                particleSystem.startSize = Random.value + 5f;
                break;
            case StarState.awake:
                iTween.ValueTo(this.gameObject, iTween.Hash("from", particleSystem.startColor.a, "to", 0.5f, "time", 5f, "easetype", iTween.EaseType.linear, "onupdate", "SetStarBrightness"));
                
                //tween position from the parent star's position to it's original pos

                break;
            case StarState.singing:
                //Tell StarSphere_Script to set my nearby stars to awake

                EmitBurst();

                object[] _parms = new object[3]{transform.position, particleSystem.startColor, _awakeNearbyStarRadius};   //my position, myColor, radius

                SSS.SendMessage("AwakeNearbyStars", _parms);


                //Color of active stars - needs to be modified
				
				HSBColor starColor = new HSBColor(0.65f, Random.value * 0.5f + 0.25f, 1f);
                particleSystem.startColor = starColor.ToColor();
                particleSystem.startSpeed = 5f;
                break;
        }
    }

    public void LerpPositionFrom(Vector3 _targetPos)
    {
        //Only lerp position when the star was sleeping
        if (STATE != StarState.sleeping) return;
        print("LerpPositionFrom called");
        //Note: This motion can be vary based on each star's property(size maybe) to make it seem more dynamic
        iTween.MoveFrom(this.gameObject, iTween.Hash("position", _targetPos, "time", 3.0f));

    }



    public void SetStarBrightness(float value)
    {
        particleSystem.startColor = new Color(particleSystem.startColor.r, particleSystem.startColor.g, particleSystem.startColor.b, value);
    }

}