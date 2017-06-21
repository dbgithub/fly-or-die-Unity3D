using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RotorDestructionScript : MonoBehaviour {

	public GameObject ah64body;
	public GameObject explosionPrefab;
	private bool isDestroyed;

	// Use this for initialization
	void Start () {
		isDestroyed = false;
	}

	void OnTriggerEnter(Collider other) {
		if (!other.name.StartsWith ("PickUpSphere")) {
			ah64body.GetComponent<HelicopterController> ().mainRotorActive = false;
			ah64body.GetComponent<HelicopterController> ().tailRotorActive = false;

			// Instantiate the explosion prefab object.
			Instantiate( explosionPrefab, transform.position, transform.rotation );

			// and finally, we destroy the propeller gameobject itself.
			if (!isDestroyed)  {
				isDestroyed = true;
				GameManager.youwin = true;
				GameObject.Find ("Apache Ah-04 Sand").GetComponents<AudioSource>()[0].volume = 0.0f;
				GameObject.Find ("Apache Ah-04 Sand").GetComponents<AudioSource>()[1].volume = 0.0f;
				Destroy( gameObject );
			}
		}
	}
}
