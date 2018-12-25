using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

    public float gravity;
	public bool inputModeKeyboard;

	private void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		string[] allJoysticks = Input.GetJoystickNames();
		if (allJoysticks.Length == 0) {
			inputModeKeyboard = true;
		} else {
			inputModeKeyboard = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {
			inputModeKeyboard = true;
		}	
		if (Input.GetAxis ("joystickHorz") > 0 || Input.GetAxis ("joystickVert") > 0) {
			inputModeKeyboard = false;
		}
	}
}
