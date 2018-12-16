using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefab : MonoBehaviour {

	private float timer;

	public GameObject obj;
	public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer >= 3) {
			timer = 0;
			GameObject newGuy = Object.Instantiate (obj, transform.position, transform.rotation);
			newGuy.gameObject.GetComponent<Chase> ().target = target.transform;
		}
	}
}
