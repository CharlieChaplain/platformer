using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitToChase : MonoBehaviour {

	private Chase chase;
	private LookAt lookAt;
	private float timer = 0;
    public float timeToAggro; //set this to 0 for immediate chasing

	public bool keepChasing = false;

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

		if (timer >= timeToAggro || keepChasing) {
			keepChasing = true;
			chase.doChase = true;
			lookAt.canLook = false;
		}
	}

    //used to stop the object from chasing
    void ResetWaitToChase()
    {
        chase.doChase = false;
        lookAt.takeAGander = false;
        keepChasing = false;
        lookAt.canLook = true;
        timer = 0;
    }
}
