using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMove : MonoBehaviour {

	public Transform target;
	public Transform transform;

	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 posXZ = target.position;
		RaycastHit hit;
		int layerMask = 1 << 8;
		Physics.Raycast (target.position, Vector3.down, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);

		transform.position = new Vector3 (posXZ.x, hit.point.y + 0.05f, posXZ.z);
		transform.up = hit.normal;
	}
}
