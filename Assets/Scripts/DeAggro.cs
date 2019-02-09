using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeAggro : MonoBehaviour {

    public GameObject target;
    public float range;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if((target.transform.position - transform.position).sqrMagnitude > range * range)
        {
            SendMessage("ResetWaitToChase");
        }
	}
}
