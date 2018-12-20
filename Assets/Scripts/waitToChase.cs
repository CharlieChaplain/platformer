using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitToChase : MonoBehaviour {

	private Chase chase;
	private LookAt lookAt;
	private float timer = 0;

	private bool keepChasing = false;

	// Use this for initialization
	void Start () {
		chase = GetComponent<Chase> ();
		lookAt = GetComponent<LookAt> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (lookAt.takeAGander) {
			timer += Time.deltaTime;
		} else {
			timer = 0;
		}

		//Debug.Log (timer);
		Debug.Log (keepChasing);

		if (timer >= 5 || keepChasing) {
			keepChasing = true;
			chase.doChase = true;
			lookAt.canLook = false;
		}
	}
}
