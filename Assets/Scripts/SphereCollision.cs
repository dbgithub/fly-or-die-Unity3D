using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollision : MonoBehaviour {

	int hola;
	private bool collision;

	// Use this for initialization
	void Start () {
		hola = 0;
		collision = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other) {
		collision = true;
		Debug.Log("ENTERED!!! (" + other.name + ")");
	}
	void OnTriggerStay(Collider other) {
		hola++;
		if (hola <4) {
			Debug.Log("STAY!!!");
		}
		
	}
	void OnTriggerExit(Collider other) {
		Debug.Log("EXITED!!!");
		hola = 0;
	}
}
