using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class HelicopterController : MonoBehaviour {

	public GameObject mainRotor; // This object groups the following two rotors (static + moving)
	public GameObject staticMainRotor; // The normal rotor object, that begins motionless
	public GameObject movingMainRotor; // Another rotor object that has another texture representing moving blades
	public GameObject tailRotor; // The normal rotor object, that begins motionless (for tail rotor)
	public GameObject movingTailRotor; // Another rotor object that has another texture representing moving blades (for tail rotor)
	private Rigidbody rb; // This represents the helicopter!
	public Text uitxtSpeed;
	public Text uitxtAltitude;
	private bool freeLook;

	// Scripts references for camera type (SmoothFollow vs MouseOrbit):
	SmoothFollowImproved sf2;
	MouseOrbit mo;

	// Main rotor
	public float maxRotorForce = 22241f;
	public float maxRotorVelocity = 7200f;
	static float rotorVelocity = 0f;
	private float rotorRotation = 0f;
	public bool mainRotorActive = true;
	// Tail rotor
	public float maxTailRotorForce = 15000f;
	public float maxTailRotorVelocity = 2200f;
	private float tailRotorVelocity = 0f;
	private float tailRotorRotation = 0f;
	public bool tailRotorActive = true;
	// Helicopter torque
	public float forwardRotorTorqueMultiplier = 0.5f;
	public float sidewaysRotorTorqueMultiplier = 0.5f;

	// HUD related variables:
	double powerBoost;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		uitxtSpeed.text = "--";
		uitxtAltitude.text = "--";
		freeLook = false;
		sf2 = GameObject.Find("MainCamera").GetComponent<SmoothFollowImproved> ();
		mo = GameObject.Find("MainCamera").GetComponent<MouseOrbit> ();
	}

	// Called every physics step. FixedUpdate intervals are consistent.
	// Used for regular updates related to physics (rigidbody) objects etc.
	void FixedUpdate () {
		Vector3 torqueValue = new Vector3();
		Vector3 controlTorque = new Vector3(Input.GetAxis("Vertical")*forwardRotorTorqueMultiplier, 1.0f, -1 * Input.GetAxis("Horizontal2") * sidewaysRotorTorqueMultiplier);
		if (mainRotorActive == true) {
			torqueValue += (controlTorque * maxRotorForce * rotorVelocity);
			rb.AddRelativeForce (Vector3.up * maxRotorForce * rotorVelocity);
		}
		if (Vector3.Angle(Vector3.up, transform.up) < 80) {
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y,0), Time.deltaTime * rotorVelocity * 2);
		}
		if (tailRotorActive == true) {
			torqueValue -= (Vector3.up * maxTailRotorForce * tailRotorVelocity);
			rb.AddRelativeTorque (torqueValue);
		}
	}
	
	// Update is called every frame. Used for general purposes: moving non-pyhisics objects, simple timers receiving INPUT etc.
	// A frame can take longer than another one, so the temporality is not the same for every frame.
	void Update () {
		//Adjusting the pitch of the Audio source depending on the velocity of the rotor. Since the latter goes from 0.0 to 1.0 values.
		GetComponents<AudioSource>()[0].pitch = rotorVelocity;
		GetComponents<AudioSource>()[1].pitch = rotorVelocity;

		// We would like to activate/deactivate some objects when the blades are fast enough to barely see them, or viceversa.
		// To find out the difference between Activating/Deactivating and Enabling/Disabling and Object or Component, take a look at: http://answers.unity3d.com/questions/785273/whats-the-difference-between-activatedeactivate-en.html
		if (staticMainRotor !=null &&  movingMainRotor !=null && movingTailRotor !=null ){
			if (rotorVelocity > 0.4){
				GetComponents<AudioSource> () [0].volume = 1;
				movingMainRotor.SetActive(true);
				staticMainRotor.SetActive(false);
				movingTailRotor.SetActive(true);
			} else {
				GetComponents<AudioSource> () [0].volume = rotorVelocity*0.6f;
				movingMainRotor.SetActive(false);
				staticMainRotor.SetActive(true);
				movingTailRotor.SetActive(false);
			}
		}
		
		if (mainRotorActive == true) {
			mainRotor.transform.rotation = transform.rotation * Quaternion.Euler (0, rotorRotation, 0);
		}
		if (tailRotorActive == true) {
			tailRotor.transform.rotation = transform.rotation * Quaternion.Euler (tailRotorRotation, 0, 0);
		}
		rotorRotation += maxRotorVelocity * rotorVelocity * Time.deltaTime;
		tailRotorRotation += maxTailRotorVelocity * rotorVelocity * Time.deltaTime;

		float hoverRotorVelocity = (rb.mass * Mathf.Abs (Physics.gravity.y) / maxRotorForce);
		float hoverTailRotorVelocity = (maxRotorForce * rotorVelocity) / maxTailRotorForce;

		// Now, if the player is pressing the key to increase the rotor throttle, then increase the throttle of the main rotor.
		// Otherwise, slowly interpolate it back to the hover velocity to maintain the helicopter steady; to hover in place.
		if (Input.GetAxis ("Vertical2") != 0.0f) {
			rotorVelocity += Input.GetAxis ("Vertical2") * 0.001f;
			// Updating HUD text for power boost:
			powerBoost = System.Math.Round (rotorVelocity * 100, 2);
			if (powerBoost >= 100) {uitxtSpeed.text = "100%";} else if (powerBoost <= 0) {uitxtSpeed.text = "0%";} else {
				uitxtSpeed.text = powerBoost.ToString() + "%";
			}
		} else {
			rotorVelocity = Mathf.Lerp (rotorVelocity, hoverRotorVelocity, Time.deltaTime * Time.deltaTime * 5);
			// Updating HUD text for power boost:
			powerBoost = System.Math.Round (rotorVelocity * 100, 2);
			if (powerBoost >= 100) {uitxtSpeed.text = "100%";} else if (powerBoost <= 0) {uitxtSpeed.text = "0%";} else {
				uitxtSpeed.text = powerBoost.ToString() + "%";
			}
		}
		tailRotorVelocity = hoverTailRotorVelocity - Input.GetAxis ("Horizontal");

		if (rotorVelocity > 1.0) {
			rotorVelocity = 1.0f;
		} else if (rotorVelocity < 0.0) {
			rotorVelocity = 0.0f;
		}

		// Updating HUD text for altitude:
		uitxtAltitude.text = estimateAltitude().ToString() + " m";

		///////////////
		///////////////
		// Chopper KEYBOARD controls:
		if (Input.GetKeyDown(KeyCode.F)) {
			freeLook = !freeLook;
			sf2.enabled = !sf2.enabled;
			mo.enabled = !mo.enabled;
		}
	}

	float estimateAltitude() {
		RaycastHit hit;
		Vector3 heliPos = GameObject.FindWithTag("Helicopter").transform.position; // To find out more about tags, see: https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html
		Physics.Raycast (heliPos- Vector3.up, -Vector3.up, out hit);
		return Mathf.Round(hit.distance);
	}

}
