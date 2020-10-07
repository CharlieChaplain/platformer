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

    public Material playerTex; //the opaque player texture
    public Material playerTexTrans; //the transparent player texture
    public ParticleSystem poisonBubbles; //the bubbles that emit when player is poisoned

    public string savePointScene;
    public int savePointIndex;

    //used for attacks
    public bool canAttack;
    //public bool attacking;        Do I need this?

    public enum StatusEffect
    {
        none,
        poisoned
    };

    public StatusEffect currentStatus = StatusEffect.none;

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
        player = GameObject.FindGameObjectWithTag("Player");
        currentTarget = player;
        playerTex = player.GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        playerTexTrans = player.GetComponentInChildren<SkinnedMeshRenderer>().materials[1];
        poisonBubbles = GameObject.Find("PlayerPoisonParticles").GetComponent<ParticleSystem>();
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
