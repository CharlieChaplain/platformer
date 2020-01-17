using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager Instance { get; private set; }
    public GameObject player;
	public float currentHealth;
	public float maxHealth;
	public GameObject currentWep;
    public GameObject currentTarget; //the current target of the player for ranged attacks
    public bool canMove; //determines if the player can move around
    public bool canLook; //determines if the player can look around for purposes of ranged weapons (rotates at hip)
    public Vector3 faceDir; //the direction the player is facing (not looking via camera)

    //used for attacks
    public bool canAttack;
    //public bool attacking;        Do I need this?

    private void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
        canLook = false;
        canMove = true;
        canAttack = true;
        //attacking = false;
    }

	// Use this for initialization
	void Start () {
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
