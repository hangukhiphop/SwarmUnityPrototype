using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Script for the parent sphere of stars
//I made it separetly with SkySphere because SkySphere moves below during the sunset, making a not perfect hemisphere from the viewer
//Plus, this StarSphere rotates 180 degress over 10 minutes <= look at line 20

public class StarSphere_Script : MonoBehaviour {
    
    public GameObject Prefab_Star;
    public List<Star_Script> Star_List;
	
	public Camera cameraL;
    
	Star_Script pickedStar;
	// Use this for initialization
	void Start () {
        PopulateStars(1000);
		
		
		pickedStar = PickOneStarToAwake();
        //iTween.RotateTo(this.gameObject, iTween.Hash("rotation", new Vector3(0, 0, 180), "time", 600.0f, "easetype", iTween.EaseType.linear));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
	public void ReserveOneStarToAwake(float _time){
		
		Invoke("AwakePickedStar", _time);
	}
			
	void AwakePickedStar(){
		pickedStar.SendMessage("ChangeState", Star_Script.StarState.awake);
	}
	
    private void PopulateStars(int _numOfStars)
    {
        print("PopulateStars called");
        Star_List = new List<Star_Script>();

        for (int i = 0; i < _numOfStars; i++)
        {
            GameObject s = GameObject.Instantiate(Prefab_Star, PolarToCartesian(150, Random.value*360f, Random.value * 360f), Quaternion.identity) as GameObject;
            s.transform.parent = this.transform;
            Star_List.Add(s.GetComponent<Star_Script>());
        }
    }

    //Recursive function that selects one star that's certain distance above the horizon
    private Star_Script PickOneStarToAwake()
    {
        print("PickOneStarToAwake called");
        Star_Script pickedStarScript;
        //pick one random star
        int randomStarIndex = (int)(Random.value * Star_List.Count);
        
        //if the star's position is above certain distance from the horizon && visible from the main camera
        //if(Star_List[randomStarIndex].transform.position.y > 20f && Star_List[randomStarIndex].transform.position.y < 40f && Star_List[randomStarIndex].transform.renderer.IsVisibleFrom(Camera.main)){
		//if(Star_List[randomStarIndex].transform.position.y > 20f && Star_List[randomStarIndex].transform.position.y < 40f && Star_List[randomStarIndex].transform.renderer.IsVisibleFrom(cameraL)){
		if(Star_List[randomStarIndex].transform.position.y > 60f && Star_List[randomStarIndex].transform.position.y < 80f && Star_List[randomStarIndex].transform.position.x > -40f && Star_List[randomStarIndex].transform.position.x < 40f && Star_List[randomStarIndex].transform.position.z > 30f){
            pickedStarScript = Star_List[randomStarIndex];			
        }else{
            pickedStarScript = PickOneStarToAwake();
        }
		
		SphereCollider myCollider = pickedStarScript.transform.GetComponent<SphereCollider>();
		myCollider.radius = 10.0f;
		
        return pickedStarScript;
    }

    //Some math to distribute stars evenly onto surface of the StarSphere
    Vector3 PolarToCartesian(float _radius, float _inclination, float _azimuth)
    {
        
        /*
        3x3 matrix
        Mathf.Cos(_azimuth) Mathf.Sin(_inclination), -_radius Mathf.Sin(_inclination) Mathf.Sin(_azimuth),  _radius Mathf.Cos(_inclination) Mathf.Cos(_azimuth)
        Mathf.Sin(_inclination) Mathf.Sin(_azimuth),  _radius Mathf.Cos(_azimuth) Mathf.Sin(_inclination),  _radius Mathf.Cos(_inclination) Mathf.Sin(_azimuth)
        Mathf.Cos(_inclination),             0,                     -_radius Mathf.Sin(_inclination)
        */


        float x = _radius * Mathf.Sin(_inclination) * Mathf.Cos(_azimuth);
        float y = _radius * Mathf.Sin(_inclination) * Mathf.Sin(_azimuth);
        float z = _radius * Mathf.Cos(_inclination);


        //This Matrix thing does not work yet. 
        //I've been trying this, but maybe not correctly. http://en.wikibooks.org/wiki/Mathematica/Uniform_Spherical_Distribution
        //Currently, stars are not "evenly" distributed. 

        /*
        Matrix4x4 mtx = new Matrix4x4();


        mtx.m00 = Mathf.Cos(_azimuth) * Mathf.Sin(_inclination);
        mtx.m01 = -_radius * Mathf.Sin(_inclination) * Mathf.Sin(_azimuth);
        mtx.m02 = _radius * Mathf.Cos(_inclination) * Mathf.Cos(_azimuth);

        mtx.m10 = Mathf.Sin(_inclination) * Mathf.Sin(_azimuth);
        mtx.m11 = _radius * Mathf.Cos(_azimuth) * Mathf.Sin(_inclination);
        mtx.m12 = _radius * Mathf.Cos(_inclination) * Mathf.Sin(_azimuth);
        
        mtx.m20 = Mathf.Cos(_inclination);
        mtx.m21 = 0;
        mtx.m22 = -_radius * Mathf.Sin(_inclination);
        
        mtx.m30 = 0f;
        mtx.m31 = 0f;
        mtx.m32 = 0f;
        mtx.m03 = 0f;
        mtx.m13 = 0f;
        mtx.m23 = 0f;
        mtx.m33 = 1f;
        */

        Vector3 returnVec = new Vector3(x, y, z);

        //returnVec = mtx.MultiplyVector(returnVec);
        //returnVec = mtx.MultiplyPoint(returnVec);
        return returnVec;
    }

    public void AwakeNearbyStars(object[] _parms)
    {
        print("AwakeNearbyStars called");
        Vector3 originStarPos = (Vector3)_parms[0];
        Color originStarColor = (Color)_parms[1];
        float radius = (float)_parms[2];

        //Check nearby Stars
        Collider[] collidingStars = Physics.OverlapSphere(originStarPos, radius);

        foreach (Collider c in collidingStars)
        {
            //Tell them to awake (if it's already awaken, it will ignore the message)
            c.gameObject.SendMessage("LerpPositionFrom", originStarPos, SendMessageOptions.DontRequireReceiver);
            c.gameObject.SendMessage("ChangeState", Star_Script.StarState.awake, SendMessageOptions.DontRequireReceiver);
        }
    }

}
