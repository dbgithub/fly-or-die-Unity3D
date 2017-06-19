using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAway : MonoBehaviour {

	public GameObject imageToFade;
	public GameObject textToFade;
	private bool triggered;
	private Image img;
	private Text txt;
	private float fadingDuration = 4.0f;

	public void Start()
	{
		img = imageToFade.GetComponent<Image>();
		txt = textToFade.GetComponent<Text> ();
		triggered = false;
	}

	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > 5 && triggered == false) {
			img.CrossFadeAlpha (0.0f, fadingDuration, false);
			txt.CrossFadeAlpha (0.0f, fadingDuration, false);
			triggered = true;
		}
		if (Time.timeSinceLevelLoad > 5 + fadingDuration) {
			imageToFade.SetActive (false);
			textToFade.SetActive (false);
			//Destroy (imageToFade);
			//Destroy (textToFade);
		}
	}
}
