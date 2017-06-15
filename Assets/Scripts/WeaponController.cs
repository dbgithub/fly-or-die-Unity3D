using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public GameObject machinegun;
	public float fireDelay;
	public GameObject bulletImpactPrefab;
	private float weaponFireTimer;
	private RaycastHit raycasthit;

	// Use this for initialization
	void Start () {
		weaponFireTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		ParticleSystem.EmissionModule em = machinegun.GetComponent<ParticleSystem> ().emission;
		em.enabled = false;
		//Debug.Log ("Is GetButton pressed? " + Input.GetButton ("Fire1") + " | FireTimer: " + weaponFireTimer + " | fireDelay: "+ fireDelay);
		if (Input.GetButton ("Fire1") && weaponFireTimer >= fireDelay) {
			weaponFireTimer = 0.0f;
			machinegun.GetComponent<AudioSource> ().Play ();
			em.enabled = true;

			if (Physics.Raycast(machinegun.transform.position,machinegun.transform.forward, out raycasthit)) {
				Instantiate(bulletImpactPrefab, raycasthit.point, Quaternion.LookRotation(raycasthit.normal));
			}
		}
		weaponFireTimer += Time.deltaTime;
	}
}
