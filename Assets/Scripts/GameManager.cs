using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

    public float gravity;
	public bool inputModeKeyboard;
    public Queue<GameObject> InactivePellets; //slingshot pellets shot by Punkin
    public GameObject pelletPrefab; //the prefab of the slingshot pellet

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

        InactivePellets = new Queue<GameObject>();

        for(int i = 0; i < 6; i++)
        {
            GameObject proj = GameObject.Instantiate(pelletPrefab, transform.position, Quaternion.identity);
            proj.GetComponent<ProjectileInfo>().homeQueue = InactivePellets;
            InactivePellets.Enqueue(proj);
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
