using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager Instance { get; private set; }
    public GameObject player;
	public float currentHealth;
	public float maxHealth;
	public GameObject currentWep; //used with base punkin to determine which wep they're using
    public GameObject bigPunkWep; //a gameobject that holds the weapon info for big punk's fist
    public GameObject dollyPunkWep; //a gameobject that holds the weapon info for doll punk's needle
    public GameObject currentTarget; //the current target of the player for ranged attacks
    //public bool canMove; //determines if the player can move around
    public bool canLook; //determines if the player can look around for purposes of ranged weapons (rotates at hip)
    public Vector3 faceDir; //the direction the player is facing (not looking via camera)

    public Material playerTex; //the opaque player texture
    public Material playerTexTrans; //the transparent player texture
    public ParticleSystem poisonBubbles; //the bubbles that emit when player is poisoned

    public string savePointScene;
    public int savePointIndex;

    //used for attacks
    public bool canAttack;

    public enum StatusEffect
    {
        none,
        poisoned
    };

    public enum PunkinType
    {
        basePunkin,
        headPunkin,
        bigPunkin,
        dollyPunkin
    };

    public StatusEffect currentStatus = StatusEffect.none;
    public PunkinType currentType;

    private void Awake(){
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
        canLook = false;
        //canMove = true;
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

    public void SwapEffigy(int index, GameObject effigyToDelete)
    {
        switch (index){
            case 0:
                GetComponent<SwapEffigy>().PunkinToHead();
                break;
            case 1:
                GetComponent<SwapEffigy>().HeadToPunkin(effigyToDelete);
                break;
            case 2:
                GetComponent<SwapEffigy>().HeadToBigPunk(effigyToDelete);
                break;
            case 3:
                GetComponent<SwapEffigy>().HeadToDollPunk(effigyToDelete);
                break;
            case 4:
                GetComponent<SwapEffigy>().BigPunkToHead();
                break;
            case 5:
                GetComponent<SwapEffigy>().DollPunkToHead();
                break;
            default:
                break;
        }
    }
    public void SpawnWeapon()
    {
        GetComponent<ChangeWeapon>().SpawnWeapon();
    }
}
