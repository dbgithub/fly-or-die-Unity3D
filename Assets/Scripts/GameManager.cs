using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Representative simple example of pausing a game (YouTube): https://www.youtube.com/watch?v=tdU9ujYMA_k
// Creating a scene menu tutorial: https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/creating-a-scene-menu
public class GameManager : MonoBehaviour {

	private bool isPaused;
	private float proficiencyLevel; // This represents the points the player is adquaring (destroyed Spheres)
	private int numSpheres; // number of spheres that will act as objectives to capture by the player
	public static bool triggerResume = false;
	public GameObject HUD;
	private bool wasMouseOrbitActivated;
	public GameObject inGameMenu;
	public GameObject mainCamera;
	public GameObject particleSys;
	public GameObject uitxtProficiency; // UI text (label) used to display the proficiency level of the player
	MouseOrbit mo; // Scripts reference for MouseOrbit camera type
	private Vector3 offset; // distance offset between the camera and the particle system for in-game menu
	private AudioSource audiosrc;

	// Use this for initialization
	void Start () {
		isPaused = false;
		proficiencyLevel = 0.0f;
		numSpheres = GameObject.Find ("Spheres").transform.childCount;
		Time.timeScale = 1.0F;
		mo = GameObject.Find("MainCamera").GetComponent<MouseOrbit> ();
		offset = particleSys.transform.position - mainCamera.transform.position;
		audiosrc = inGameMenu.GetComponent<AudioSource> ();
		audiosrc.ignoreListenerPause = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel") || triggerResume) {
			isPaused = !isPaused;
			UpdateProficiencyLevel ();

			if (isPaused) {
				Time.timeScale = 0.0F;
				wasMouseOrbitActivated = mo.enabled;
				mo.enabled = false;
				HUD.SetActive (false);
				particleSys.transform.position = mainCamera.transform.position + offset;
				inGameMenu.SetActive (true);
				audiosrc.PlayOneShot (audiosrc.clip);
			} else {
				triggerResume = false;
				Time.timeScale = 1.0F;
				mo.enabled = wasMouseOrbitActivated;
				HUD.SetActive (true);
				inGameMenu.SetActive (false);
			}

			AudioListener.pause = !AudioListener.pause; // You can access AudioListener in a static way, because it is supposed to only have one audio listener in you scenes.
		}
	}

	/*
	 * It resets the state of the level so that when we go back everything is tidy and clean.
	 * This function will be called when the user exist current level.
	 * More information about switching between levels: 
	 */
	void OnDestroy() {
		// *NOTE: Apparently there is no need to worry about displaying or making dissapear the menus, enabling HUD etc.
		// All GameObjects in the hierarchy are destroyed when another level is loaded.
		Debug.Log ("Game Manager was destroyed!!!");
		Time.timeScale = 1.0F;
		if (AudioListener.pause) {AudioListener.pause = false;}
	}

	// Updates the UI label for proficiency level in the HUD
	public void UpdateProficiencyLevel() {
		uitxtProficiency.GetComponent<Text>().text = ((++proficiencyLevel / numSpheres) * 100).ToString() + "%";
	}
}
