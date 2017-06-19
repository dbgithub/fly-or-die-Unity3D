using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Trying to figure out how to make text blink (fade out, fade in): https://stackoverflow.com/questions/27885201/fade-out-unity-ui-text
// http://answers.unity3d.com/questions/661882/blinking-gui-text-question.html
public class BlinkingText : MonoBehaviour {

	public GameObject textToFade;
	private bool triggered;
	private Text txt;
	private float fadingDuration = 2.0f;
	private LoadResumeSceneManager script;

	public void Start()
	{
		txt = textToFade.GetComponent<Text> ();
		script = GetComponent<LoadResumeSceneManager> (); // loading the next scene after certain time via coroutine
		script.LoadByIndexAfterXseconds (2, 27.0f); // loading the next scene after certain time via coroutine
		triggered = false;
		StartCoroutine (Blink());
	}
		
	private IEnumerator Blink () {
		while(true) {
			print (Time.timeSinceLevelLoad);
			if (!triggered) {
				//print (Time.timeSinceLevelLoad);
				txt.CrossFadeAlpha (0.0f, fadingDuration, false);
				triggered = true;
			} else {
				//print (Time.timeSinceLevelLoad);
				txt.CrossFadeAlpha (1.0f, fadingDuration, false);
				triggered = false;
			}
			yield return new WaitForSeconds(fadingDuration);


		}
	}
}
