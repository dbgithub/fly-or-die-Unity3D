using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBackgroundMusic : MonoBehaviour {

	private AudioSource audioSrc;

	// Use this for initialization
	void Awake () {
		audioSrc = GetComponents<AudioSource> () [0];
	}

	void OnLevelWasLoaded (int level) {
		if (level == 0) {
			audioSrc.Stop ();
		}
	}
}
