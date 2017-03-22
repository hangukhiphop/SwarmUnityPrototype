using UnityEngine;
using System.Collections;

public abstract class Effect
{
	protected GameObject SourceAbility;
}

class Knockback : Effect
{
	float distance;

	void Execute(GameObject target)
	{
		//target.transform.position += SourceAbility.
	}
}
