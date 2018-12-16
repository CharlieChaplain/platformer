using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public GameObject targetLoc;
    public GameObject targetRot;
    public float distance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = targetRot.transform.forward;
        transform.position = targetLoc.transform.position + (transform.forward * distance);
	}
}
