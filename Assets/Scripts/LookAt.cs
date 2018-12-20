using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	public Transform target;
	public Transform joint;
	public Vector3 rotOffset;
	public float lookSpeed;
	public float outerRadius;
	public float innerRadius;
	public float fov;

	public bool takeAGander;
	public bool canLook;
	private Quaternion offsetQuat;

	// Use this for initialization
	void Start () {
		takeAGander = false;
		offsetQuat = Quaternion.Euler (rotOffset.x, rotOffset.y, rotOffset.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate() {
		//last update happens after animations are resolved
		if (canLook) {
			float dist = Vector3.Distance (target.position, transform.position);
			if (dist <= outerRadius && dist >= innerRadius) {
				float angle = Vector3.Angle (transform.forward, target.position - transform.position);
				if (angle <= fov / 2) {
					takeAGander = true;
				} else {
					takeAGander = false;
				}
			} else {
				takeAGander = false;
			}

			if (takeAGander) {
				Vector3 lookDir = target.position - joint.position;
				Quaternion toRotation = Quaternion.LookRotation (lookDir) * offsetQuat;
				joint.rotation = Quaternion.Lerp (joint.rotation, toRotation, lookSpeed * Time.deltaTime);
			}
		}
		//transform.rotation *= Quaternion.Euler (rotOffset.x, rotOffset.y, rotOffset.z);
	}
}
