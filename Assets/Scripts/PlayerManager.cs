using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public static PlayerManager Instance { get; private set; }

	public float currentHealth;
	public float maxHealth;
	public bool attacking;
	public GameObject currentWep;

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
		attacking = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;
		if (currentHealth < 0)
			currentHealth = 0;
	}
}
