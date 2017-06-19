using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSand : MonoBehaviour {

	public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
	public float delay = 0.0F;
    private float startTime;
    private float journeyLength;

    void Start() {
		startTime = Time.timeSinceLevelLoad;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }
    void Update() {
		if (Time.timeSinceLevelLoad >= delay) {
			float distCovered = (Time.timeSinceLevelLoad - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
		}
    }

}
