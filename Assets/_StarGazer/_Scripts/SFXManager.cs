using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

public class SFXManager : MonoBehaviour {
	public AudioClip[] sfx = new AudioClip[2]; 
	
	enum effects {
		CHIRP
	}
	
	public void playEffect (int effect) {
		audio.PlayOneShot(sfx[effect]);
	}
}