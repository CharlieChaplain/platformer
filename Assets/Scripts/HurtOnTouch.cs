using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtOnTouch : MonoBehaviour {

	public float hurtValue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			PlayerManager.Instance.currentHealth -= hurtValue;
		}
	}
}
