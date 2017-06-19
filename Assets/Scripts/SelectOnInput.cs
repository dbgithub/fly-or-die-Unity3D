using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject gameObj;
	private bool buttonSelected;
	private AudioSource audsource;

	// Use this for initialization
	void Start () {
		audsource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical") != 0 && buttonSelected == false) {
			eventSystem.SetSelectedGameObject (gameObj);
			buttonSelected = true;
		} 
		// Every time the user selects a button with the KEYBOARD, a sound will be heard.
		// The event trigger related to what happens when you HOVER with the mouse POINTER, it's not managed in this script. Take
		// a look to the components for each button.
		if (Input.GetButtonDown ("Vertical")) {
			audsource.PlayOneShot (audsource.clip);
			// More info about event listeners: http://answers.unity3d.com/questions/857810/play-audio-from-46-ui-button-on-click-script.html
		}
	}

	private void OnDisable() {
		buttonSelected = false;
	}
}
