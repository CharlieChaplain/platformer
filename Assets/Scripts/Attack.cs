using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public Animator anim;
	public GameObject currentWep;

	private enum AttackState { 
		none = 0,
		lightOneHand = 1
	};

	private AttackState state = AttackState.none;
	private float attackTimer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case AttackState.none:
			if (Input.GetAxis ("Attack") > 0) {
				state = AttackState.lightOneHand;
				PlayerManager.Instance.attacking = true;
				attackTimer = currentWep.GetComponent<WeaponInfo>().swingTime;
				//0.4635f is current swing time
				currentWep.SendMessage ("SetTrailTimer");
			}
			break;
		case AttackState.lightOneHand:
			attackTimer -= Time.deltaTime;
			if(attackTimer <= 0){
				attackTimer = 0;
				state = AttackState.none;
				PlayerManager.Instance.attacking = false;
			}
			break;
		}

		int stateNum = (int)state;

		anim.SetInteger ("AttackState", stateNum);
	}
}
