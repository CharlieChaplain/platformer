using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour {

	public float moveSpeed = 10.0f;
	public float turnSpeed = 5.0f;
	public float gravMult = 0.25f;

	public CharacterController controller;
	public Animator anim;

	public Transform target;
	public Transform transform;

	private bool doChase = true;
	private Vector3 moveDirection;
	private Vector3 moveXZ;

	public float knockbackTime;
	private float knockbackTimer;

	public EnemyInfo enemyInfo;


	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		transform = GetComponent<Transform> ();
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
			break;
		case EnemyInfo.EnemyState.Invincible:
			break;
		default:
			break;
		}

		controller.Move (moveDirection * Time.deltaTime);

	}

	void Move(){
		if (doChase)
			Pursue ();
		else
			Wander ();

		transform.forward = Vector3.RotateTowards (transform.forward, moveXZ, turnSpeed * Time.deltaTime, 0.0f);
		moveDirection.x = moveXZ.x;
		moveDirection.z = moveXZ.z;
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
