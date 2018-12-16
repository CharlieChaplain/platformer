using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyShader : MonoBehaviour {

    public Shader shader;
    public Camera cam;

	// Use this for initialization
	void Start () {
        //cam.RenderWithShader(shader, "Test");
        cam.SetReplacementShader(shader, "RenderType");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
