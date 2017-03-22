using UnityEngine;
using System.Collections;

public abstract class AbilityAimer : MonoBehaviour 
{
	protected float HorizontalDirection;
	protected float VerticalDirection;

	public virtual void Initialize(GameObject playerCamera, float range)
	{

	}

	public void SetAxes(float horizontal, float vertical)
	{
		HorizontalDirection = horizontal;
		VerticalDirection = vertical;
	}
}
