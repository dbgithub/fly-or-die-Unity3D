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
	public static bool youwin = false; // booleano para que en HelicopterController evite subir el volumen al sonido del rotor.
	public GameObject HUD; // CANVAS
	public GameObject MissionAccomplishedImage; // MissionAccomplished image
	public GameObject MissionAccomplishedLabel; // MissionAccomplished label
	private bool wasMouseOrbitActivated;
	public GameObject inGameMenu;
	public GameObject mainCamera;
	public GameObject particleSys;
	public GameObject uitxtProficiency; // UI text (label) used to display the proficiency level of the player
	MouseOrbit mo; // Scripts reference for MouseOrbit camera type
	private Vector3 offset; // distance offset between the camera and the particle system for in-game menu
	private AudioSource audiosrc;
	private AudioSource youwinAudioSrc; // You Win sound!
	private AudioSource militarychatter2;

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
		youwinAudioSrc = GameObject.Find ("CanvasMissionAccomplished").GetComponents<AudioSource> ()[0];
		youwinAudioSrc.ignoreListenerPause = true;
		militarychatter2 = GameObject.Find ("CanvasMissionAccomplished").GetComponents<AudioSource> ()[1];
		militarychatter2.ignoreListenerPause = true;
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

		// TODO: This is a little trick to WIN the game straightforward so that in the DEMO, it's not necessary to go through the whole process!
		// In other words, we accelerate the winning process!!
		if (Input.GetKeyDown(KeyCode.Space)) {
			for (int i = 0; i < numSpheres; i++) {
				UpdateProficiencyLevel ();
			}
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
		if ((proficiencyLevel / numSpheres) * 100 == 100) {
			// MISSION ACCOMPLISHED!!
			StartCoroutine(YouWin());
		}
	}

	private IEnumerator YouWin() {
		yield return new WaitForSeconds (1.5f);
		youwinAudioSrc.PlayOneShot (youwinAudioSrc.clip);
		militarychatter2.PlayOneShot (militarychatter2.clip);
		MissionAccomplishedImage.GetComponent<Image>().enabled = true;
		MissionAccomplishedLabel.GetComponent<Text> ().enabled = true;
		Time.timeScale = 0.2F;
		youwin = true;
		GameObject.Find ("Apache Ah-04 Sand").GetComponents<AudioSource> () [0].volume = 0.2f;
		GameObject.Find ("Apache Ah-04 Sand").GetComponents<AudioSource> () [1].volume = 0.2f;
	}
}
