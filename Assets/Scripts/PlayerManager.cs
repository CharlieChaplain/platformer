using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager Instance { get; private set; }

	public float currentHealth;
	public float maxHealth;
	public bool attacking;
	public GameObject currentWep;
    public GameObject currentTarget; //the current target of the player for ranged attacks
    public bool canLook; //determines if the player can look around
    public Vector3 faceDir; //the direction the player is facing (not looking via camera)

	private void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
        canLook = false;
	}

	// Use this for initialization
	void Start () {
		attacking = false;
        currentTarget = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;
		if (currentHealth < 0)
			currentHealth = 0;

        if (currentWep.GetComponent<WeaponInfo>().ranged)
            canLook = true;
        else
            canLook = false;
	}
}
