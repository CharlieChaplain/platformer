using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFly : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Launch(Vector3 force)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<RockBreak>().setThrown(true);
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
