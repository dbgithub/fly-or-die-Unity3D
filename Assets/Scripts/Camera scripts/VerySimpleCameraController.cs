using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject targetObject;
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		offset = targetObject.transform.TransformPoint (0,3,-8);
		transform.position = Vector3.Lerp (transform.position, offset, 7 * Time.deltaTime);
		transform.LookAt (targetObject.transform.position);
	}
}
