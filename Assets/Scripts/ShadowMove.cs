using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMove : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 posXZ = target.position;
		RaycastHit hit;
		int layerMask = 1 << 8;
		Physics.Raycast (target.position, Vector3.down, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

        posXZ.y = hit.point.y;
        transform.position = posXZ;
        transform.up = hit.normal;
	}
}
