using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour {

	public GameObject mainRotor;
	public GameObject tailRotor;
	private Rigidbody rb;

	// Main rotor
	public float maxRotorForce;
	public float maxRotorVelocity;
	static float rotorVelocity;
	private float rotorRotation;
	public bool mainRotorActive;
	// Tail rotor
	public float maxTailRotorForce;
	public float maxTailRotorVelocity;
	private float tailRotorVelocity;
	private float tailRotorRotation;
	public bool mainTailRotorActive;
	// Helicopter torque
	public float forwardRotorTorqueMultiplier;
	public float sidewaysRotorTorqueMultiplier;

	// Use this for initialization
	void Start () {
		maxRotorForce = 24000f;
		maxRotorVelocity = 7200f;
		rotorVelocity = 0f;
		rotorRotation = 0f;
		mainRotorActive = true;
		maxTailRotorForce = 25000f;
		maxTailRotorVelocity = 2200f;
		tailRotorVelocity = 0f;
		tailRotorRotation = 0f;
		mainTailRotorActive = true;
		forwardRotorTorqueMultiplier = 0.5f;
		sidewaysRotorTorqueMultiplier = 0.5f;

		rb = GetComponent (Rigidbody);
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 torqueValue;
		Vector3 controlTorque = new Vector3(Input.GetAxis("Vertical")*forwardRotorTorqueMultiplier, 1.0, -Input.GetAxis("Horizontal2") * sidewaysRotorTorqueMultiplier);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
