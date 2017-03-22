using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public GameObject myPlayer;
	public GUITexture healthBar;
	StatManager playerStats;
	GUIText health;
	// Use this for initialization
	void Start () 
	{
		health = gameObject.GetComponent<GUIText>();
		playerStats = myPlayer.GetComponent<StatManager> ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		health.text = (int)playerStats.current_health + "/" + playerStats.max_health;
		Rect rect = healthBar.pixelInset;
		rect.width = 128*playerStats.current_health / playerStats.max_health;
		healthBar.pixelInset = rect;
		float healthRatio = (float)playerStats.current_health / (float)playerStats.max_health;

		if(healthRatio <= .25f)
		{
			healthBar.color = Color.red;
		}
		else if(healthRatio <= .9f)
		{
			healthBar.color = Color.yellow;
		}
		else
		{
			healthBar.color = Color.green;
		}

	}
}
