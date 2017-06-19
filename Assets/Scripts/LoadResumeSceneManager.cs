using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Creating a scene menu tutorial: https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/creating-a-scene-menu
public class LoadResumeSceneManager : MonoBehaviour {

	public void LoadByIndex(int sceneIndex) {
		SceneManager.LoadScene (sceneIndex);
	}

	public void LoadByIndexAfterXseconds(int sceneIndex, float WaitForSecondsRealtime) {
		StartCoroutine (Countdown(sceneIndex, WaitForSecondsRealtime));
	}

	public void ResumeGame() {
		GameManager.triggerResume = true;
	}

	private IEnumerator Countdown(int scene, float delay){
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (scene);
	}
		
}
