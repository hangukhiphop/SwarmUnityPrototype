using UnityEngine;
using System.Collections;

public class animalDespawn : MonoBehaviour {
	void OnTriggerEnter (Collider col) {
		if (col.tag == "animal")
			col.gameObject.SetActive(false);
	}
}