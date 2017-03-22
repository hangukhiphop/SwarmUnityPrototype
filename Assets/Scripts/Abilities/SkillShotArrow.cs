using UnityEngine;
using System.Collections;

public class SkillShotArrow : AbilityAimer {

	private GameObject PlayerCamera;
	private Vector3 RelativeToPlayer;

	private float Range;
	
	// Update is called once per frame
	void Update () 
	{
		Quaternion JoyStickDirection = Quaternion.LookRotation (new Vector3 (HorizontalDirection, 0, VerticalDirection));

		if(Mathf.Abs(HorizontalDirection) < .5f && Mathf.Abs(VerticalDirection) < .5f)
		{
			HorizontalDirection = transform.parent.forward.x;
			VerticalDirection = transform.parent.forward.z;
			transform.rotation = Quaternion.LookRotation (transform.parent.forward)*Quaternion.Euler (90, -90, 0);
		}
		else
		{
			HorizontalDirection = transform.parent.forward.x;
			VerticalDirection = transform.parent.forward.z;
			//transform.rotation = Quaternion.LookRotation (transform.parent.forward)*Quaternion.Euler (90, -90, 0);
		transform.rotation = 
			Quaternion.LookRotation (transform.position + new Vector3 (PlayerCamera.transform.forward.x, 0, PlayerCamera.transform.forward.z)*Range - transform.position)
				*JoyStickDirection
					*Quaternion.Euler (90, -90, 0);
		}
		transform.position = transform.parent.position + RelativeToPlayer + transform.right*transform.localScale.x*.5f;
	}

	public override void Initialize(GameObject playerCamera, float range)
	{
		PlayerCamera = playerCamera;
		Range = range;

		RelativeToPlayer = transform.parent.localScale.y*Vector3.up;
		transform.localScale = new Vector3(Range, 1, 1);
		transform.position += transform.right*transform.localScale.x*.5f;
		transform.rotation = Quaternion.LookRotation (
			transform.position + new Vector3 (transform.parent.forward.x, 0, transform.parent.forward.z)*Range - transform.position)
			*Quaternion.Euler (90, 0, 90);
		transform.position = transform.parent.position + RelativeToPlayer+ transform.right*transform.localScale.x*.5f;

		HorizontalDirection = 0;
		VerticalDirection = 1;
	}
}
