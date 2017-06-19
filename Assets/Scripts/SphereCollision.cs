using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SphereCollision : MonoBehaviour {

	private bool collision; // (static)
	private bool destroyed;
	private float collisionTime;
	private float collisionOutTime; // (static) This captures the time of the last collider exiting the Sphere. So that we can reset the counter laten on.
	private AudioSource audioSrc;
	private float fadingDuration = 1.5f; // fading duration for the text
	public float timeBeforeDestruction = 2.5f; // time that the user needs to accomplish with before destroying the Sphere.
	public float fadeOutDelay = 3.0f; // delay before making the text disappear
	private Text counterText; // time counter displayed in the HUD (on-going time)
	private float timeAvg; // (static) Auxiliary variable to show the on-going time counter in the HUD

	// Use this for initialization
	void Start () {
		collision = false;
		destroyed = false;
		audioSrc = GetComponent<AudioSource> ();
		counterText = GameObject.Find ("SpherePickUpTimeText").GetComponent<Text>();
		timeAvg = 0.0f; // Auxiliary variable to show the on-going time counter in the HUD
		collisionOutTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (collisionOutTime != 0.0f && timeAvg != 0.0f && (Time.time > (collisionOutTime + 1.0f)) && !collision && !destroyed) {
			timeAvg = 0.0f;
			counterText.text = 0.0f.ToString("F1"); // Rounds the value and formats the value as a float value with one decimal: X.X
			StartCoroutine (FadeAwayText());
			print ("HOLA!");
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (!collision && !destroyed) {
			collision = true;
			collisionTime = Time.time;
			counterText.CrossFadeAlpha (1.0f, 0.0f, false);
			counterText.enabled = true;
		}
		Debug.Log("ENTERED!!! (" + other.name + ")");
	}
	void OnTriggerStay(Collider other) {
		if (Time.time >= (collisionTime + timeBeforeDestruction) && !destroyed) {
			destroyed = true;
			audioSrc.PlayOneShot (audioSrc.clip);
			GetComponent<Light> ().enabled = false;
			((Behaviour)GetComponent("Halo")).enabled = false;
			gameObject.transform.GetChild (0).transform.GetChild (0).GetComponent<EllipsoidParticleEmitter> ().emit = false;
			gameObject.transform.GetChild (0).transform.GetChild (1).GetComponent<EllipsoidParticleEmitter> ().emit = false;
			Debug.Log("DINGG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			counterText.text = Math.Round (timeAvg, 1).ToString("F1"); // Rounds the value and formats the value as a float value with one decimal: X.X
			StartCoroutine (FadeAwayText());
			StartCoroutine (DestroySphere());
		}
		if (((Time.time - collisionTime) > timeAvg) && !destroyed) {timeAvg = (Time.time - collisionTime); counterText.text = Math.Round (timeAvg, 1).ToString("F1");}
		//Debug.Log("STAY!!! (" + other.name + "): " + timeAvg);
		
	}
	void OnTriggerExit(Collider other) {
		collision = collision & false; // '&' performs logical AND operation !!
		collisionOutTime = Time.time;
		Debug.Log("EXITED!!! (" + other.name + ")");
	}

	private IEnumerator FadeAwayText(){
		yield return new WaitForSeconds (fadeOutDelay);
		counterText.CrossFadeAlpha (0.0f, fadingDuration, false);
	}

	private IEnumerator DestroySphere(){
		yield return new WaitForSeconds (fadeOutDelay + 10.0f);
		Destroy (gameObject); // Destroys the Sphere to save memory or whatever.
	}
}
