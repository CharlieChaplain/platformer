using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrail : MonoBehaviour {

	public GameObject[] allTrails;
	private float trailTimer;
	public float time;

	// Use this for initialization
	void Start () {
		foreach (GameObject trail in allTrails) {
			trail.GetComponent<TrailRenderer> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject trail in allTrails) {
			if (trailTimer > 0) {
				trail.GetComponent<TrailRenderer> ().enabled = true;
			} else {
				trail.GetComponent<TrailRenderer> ().enabled = false;
			}
		}

		trailTimer -= Time.deltaTime;
		if (trailTimer < 0) {
			trailTimer = 0;
		}
	}

	public void SetTrailTimer(){
		trailTimer = time;
	}
}
