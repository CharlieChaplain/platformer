﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour {

	public float moveSpeed = 10.0f;
	public float turnSpeed = 5.0f;
	public float gravMult = 0.25f;

	public CharacterController controller;
	public Animator anim;

	public Transform target;

	public bool doChase;
	private Vector3 moveDirection;
	private Vector3 moveXZ;

	public float knockbackTime;
	private float knockbackTimer;

	public EnemyInfo enemyInfo;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		enemyInfo = GetComponent<EnemyInfo> ();
	}
	
	// Update is called once per frame
	void Update () {
		ApplyGravity ();
		switch(enemyInfo.enemyState){
		case EnemyInfo.EnemyState.Move:
			Move ();
			break;
		case EnemyInfo.EnemyState.Hit:
			if (knockbackTimer <= 0) {
				SendMessage ("resetState");
			} else {
				knockbackTimer -= Time.deltaTime;
			}
			break;
		case EnemyInfo.EnemyState.Attacking:
            AttackLogic();
			break;
		case EnemyInfo.EnemyState.Invincible:
			break;
		case EnemyInfo.EnemyState.Dead:
			break;
		default:
			break;
		}

        moveDirection.x = moveXZ.x;
        moveDirection.z = moveXZ.z;

        if (enemyInfo.enemyState != EnemyInfo.EnemyState.Dead)
			controller.Move (moveDirection * Time.deltaTime);

		//if animator has speed parameter, sets it
		if (anim.parameters.Length > 0) {
			for (int i = 0; i < anim.parameters.Length; i++) {
				if (anim.parameters [i].name == "speed") {
					anim.SetFloat ("speed", Mathf.Pow(moveXZ.x + moveXZ.z, 2f));
				}
			}
		}

        //will update the animator on what state the enemy is in
        anim.SetInteger("state", (int)enemyInfo.enemyState);
    }

	void Move(){
		if (doChase)
			Pursue ();
		else
			Wander ();

		transform.forward = Vector3.RotateTowards (transform.forward, moveXZ, turnSpeed * Time.deltaTime, 0.0f);
	}

	//pursues the target's transform position
	void Pursue(){
		Vector3 direction = target.position - transform.position;
		direction.y = 0.0f;
		direction.Normalize ();

		moveXZ = direction * moveSpeed;
	}

	//wanders randomly around
	void Wander(){
        moveXZ = new Vector3(0.0f, 0.0f, 0.0f);
	}

    void AttackLogic()
    {
        moveXZ = new Vector3(0.0f, 0.0f, 0.0f);

        Vector3 direction = target.position - transform.position;
        direction.y = 0.0f;
        direction.Normalize();

        transform.forward = direction;

    }

	void ApplyGravity(){
		moveDirection.y += (Physics.gravity.y * gravMult);
	}

	void BounceBack(Vector3 direction){
		knockbackTimer = knockbackTime;
		moveDirection = direction * 10.0f;
	}

	private void OnTriggerEnter(Collider other){
		/*
		if(other.tag.Equals("Player")){
			Vector3 dir = new Vector3 (-transform.forward.x, 5.0f, -transform.forward.z);
			dir.Normalize ();
			BounceBack (dir);
		}
		*/
	}
}
