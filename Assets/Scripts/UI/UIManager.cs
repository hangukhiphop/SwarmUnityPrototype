using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	
	public Text AbilityText;
	public Image AbilityImage;

	public Text HealthText;
	public Image HealthGauge;

	public Text Kills;
	public Text Deaths;
	public Text Executions;
	
	PlayerControl PC;
	// Use this for initialization
	void Start () 
	{
		PC = GetComponent<PlayerControl> ();
		
		AbilityText.text = "";
		//HealthText.text = "0/0";

		Kills.text = "0";
		Deaths.text = "0";
		Executions.text = "0";
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void SetAbility(int cd)
	{
		if(cd != 0)
		{
			AbilityText.text = "" + cd;
		}
		else
		{
			AbilityText.text = "";
		}
	}

	public void SetHealth(int current, int max)
	{
		HealthText.text = current+ "/" + max;
		HealthGauge.transform.localScale = new Vector3(1.33f*(float)current / (float)max, .625f, 1);
		if(current > (.5f*max))
		{
			HealthGauge.color = Color.green;
		}
		else if(current > (.25f*max))
		{
			HealthGauge.color = Color.yellow;
		}
		else
		{
			HealthGauge.color = Color.red;
		}
	}

	public void AddKill()
	{
		Kills.text = "" + (int.Parse (Kills.text) + 1);
	}

	public void AddDeath()
	{
		Deaths.text = "" + (int.Parse (Deaths.text) + 1);
	}

	public void AddExecution()
	{
		Executions.text = "" + (int.Parse (Executions.text) + 1);
	}
}
