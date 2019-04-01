using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public Animator anim;
	public GameObject currentWep;

	private bool twoHand;

	private enum AttackState { 
		none = 0,
		lightOneHand = 1,
		thrustTwoHand = 2,
        fireSlingshot = 3
	};

	private AttackState state = AttackState.none;
	private float attackTimer = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		currentWep = PlayerManager.Instance.currentWep;

		if (state == AttackState.none) {
			if (Input.GetAxis ("Attack") > 0) {
				state = (AttackState)currentWep.GetComponent<WeaponInfo> ().attackState;
				PlayerManager.Instance.attacking = true;
				attackTimer = currentWep.GetComponent<WeaponInfo> ().swingTime;
				//currentWep.SendMessage ("SetTrailTimer");

				if ((int)state == 2)
                {
                    MovementManager.Instance.canMove = false;
                    SendMessage("PlayerStopMovement");
                }
                else if ((int)state == 3)
                {

                    Vector3 force;
                    if (PlayerManager.Instance.currentTarget != gameObject)
                        force = (PlayerManager.Instance.currentTarget.transform.position - currentWep.transform.position).normalized * currentWep.GetComponent<WeaponInfo>().projectileSpeed;
                    else
                        force = transform.forward * currentWep.GetComponent<WeaponInfo>().projectileSpeed;
                    currentWep.GetComponent<PlayerFire>().Launch(force);
                }
			}
		} else {
			attackTimer -= Time.deltaTime;
			if(attackTimer <= 0){
				attackTimer = 0;
				state = AttackState.none;
				PlayerManager.Instance.attacking = false;
				MovementManager.Instance.canMove = true;
			}
		}

		twoHand = currentWep.GetComponent<WeaponInfo> ().twoHand;

		int stateNum = (int)state;

		anim.SetInteger ("AttackState", stateNum);
		anim.SetBool ("TwoHand", twoHand);
	}
}
