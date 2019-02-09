using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaterial : MonoBehaviour {

    public float multiplier;
    private Material mat;

	// Use this for initialization
	void Start () {
        mat = gameObject.GetComponent<Renderer>().materials[1];
	}
	
	// Update is called once per frame
	void Update () {
        Color newColor = new Color(1.0f, 1.0f, 1.0f, Mathf.PingPong(Time.time * multiplier, 1.0f));
        mat.color = newColor;
	}
}
