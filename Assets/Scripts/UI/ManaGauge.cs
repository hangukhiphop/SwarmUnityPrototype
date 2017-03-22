using UnityEngine;
using System.Collections;

public class ManaGauge : MonoBehaviour {

	public GameObject myPlayer;
	public GUITexture manaGauge;
	StatManager playerStats;
	GUIText mana;
	// Use this for initialization
	void Start () 
	{
		mana = gameObject.GetComponent<GUIText>();
		playerStats = myPlayer.GetComponent<StatManager> ();
		manaGauge = gameObject.GetComponent<GUITexture> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		mana.text = (int)playerStats.current_mana + "/" + playerStats.max_mana;
		Rect rect = manaGauge.pixelInset;
		rect.width = 128*playerStats.current_mana / playerStats.max_mana;
		manaGauge.pixelInset = rect;
		float manaRatio = (float)playerStats.current_mana / (float)playerStats.max_mana;
		

		
	}
}
