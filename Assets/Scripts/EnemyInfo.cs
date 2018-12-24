﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour {

	public float currentHealth;
	public float maxHealth;
	public float attackTimer;

	public Animator anim;

	public enum EnemyState
	{
		Move = 0,
		Hit = 1,
		Attacking = 2,
		Invincible = 3,
		Dead = 4
	};

	public EnemyState enemyState;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		enemyState = EnemyState.Move;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		checkDeath ();
	}

	void checkDeath(){
		if (currentHealth <= 0) {
			StartCoroutine (Die ());
		}
	}

	IEnumerator Die(){
		enemyState = EnemyState.Dead;
		anim.SetInteger ("State", (int)enemyState);
		GetComponent<CharacterController> ().enabled = false;
		if (GetComponent<LookAt> () != null)
			GetComponent<LookAt> ().canLook = false;

		yield return new WaitForSeconds (5f);
		GameObject.Destroy (this.gameObject);
	}
}
