using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterController : MonoBehaviour {

	public GameObject mainRotor;
	public GameObject tailRotor;
	private Rigidbody rb;
	public Text uitxtSpeed;
	public Text uitxtAltitude;

	// Main rotor
	public float maxRotorForce = 24000f;
	public float maxRotorVelocity = 7200f;
	static float rotorVelocity = 0f;
	private float rotorRotation = 0f;
	public bool mainRotorActive = true;
	// Tail rotor
	public float maxTailRotorForce = 25000f;
	public float maxTailRotorVelocity = 2200f;
	private float tailRotorVelocity = 0f;
	private float tailRotorRotation = 0f;
	public bool tailRotorActive = true;
	// Helicopter torque
	public float forwardRotorTorqueMultiplier = 0.5f;
	public float sidewaysRotorTorqueMultiplier = 0.5f;

	// UI labels related variables:
	double powerBoost;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		uitxtSpeed.text = "--";
		uitxtAltitude.text = "--";
	}

	// Update is called once per frame
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
	
	// Update is called once per frame
	void Update () {
		
		if (mainRotorActive == true) {
			mainRotor.transform.rotation = transform.rotation * Quaternion.Euler (270, rotorRotation, 0);
		}
		if (tailRotorActive == true) {
			tailRotor.transform.rotation = transform.rotation * Quaternion.Euler (0, 270, tailRotorRotation);
		}
		rotorRotation += maxRotorVelocity * rotorVelocity * Time.deltaTime;
		tailRotorRotation += maxTailRotorVelocity * rotorVelocity * Time.deltaTime;

		float hoverRotorVelocity = (rb.mass * Mathf.Abs (Physics.gravity.y) / maxRotorForce);
		float hoverTailRotorVelocity = (maxRotorForce * rotorVelocity) / maxTailRotorForce;

		if (Input.GetAxis ("Vertical2") != 0.0f) {
			rotorVelocity += Input.GetAxis ("Vertical2") * 0.005f;
			powerBoost = System.Math.Round (rotorVelocity * 100, 2);
			if (powerBoost >= 100) {uitxtSpeed.text = "100%";} else if (powerBoost <= 0) {uitxtSpeed.text = "0%";} else {
				uitxtSpeed.text = powerBoost.ToString() + "%";
			}
		} else {
			rotorVelocity = Mathf.Lerp (rotorVelocity, hoverRotorVelocity, Time.deltaTime * 5);
			powerBoost = System.Math.Round (rotorVelocity * 100, 2);
			if (powerBoost >= 100) {uitxtSpeed.text = "100%";} else if (powerBoost <= 0) {uitxtSpeed.text = "0%";} else {
				uitxtSpeed.text = powerBoost.ToString() + "%";
			}
		}
		tailRotorVelocity = hoverTailRotorVelocity - Input.GetAxis ("Horizontal") * 0.4f;

		if (rotorVelocity > 1.0) {
			rotorVelocity = 1.0f;
		} else if (rotorVelocity < 0.0) {
			rotorVelocity = 0.0f;
		}

		//uitxtAltitude.text = estimateAltitude().ToString() + " m";
	}
	/*
	float estimateAltitude() {
		
		RaycastHit hit;
		Vector3 heliPos = GameObject.Find ("Apache Ah-04 Sand").transform.position;
		Physics.Raycast (heliPos, new Vector3(0, -1, 0), out hit, 3000f, 8);
		return (Vector3.Distance (heliPos, hit.collider.transform.position));

	}*/

}
