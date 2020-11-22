using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public Animator anim;
    public GameObject currentWep;

    public int numCoroutines = 0;

    private bool twoHand;
    //private bool coroutFired = false;
    //private bool timeSet = false;
    //private bool notShot = false;
    private int stateNum;
    private AnimatorStateInfo animInfo;
    //private AnimatorClipInfo[] animClip;

    private enum AttackState
    {
        none = 0,
        lightOneHand = 1,
        thrustTwoHand = 2,
        fireSlingshot = 3
    };

    private AttackState state = AttackState.none;
    //private float attackTime = 0;
    //private float timer = 0;

    // Use this for initialization
    void Start()
    {
    }

    /*
	// Update is called once per frame
	void Update () {
		currentWep = PlayerManager.Instance.currentWep;
        /* haha huge ass commented code pls help me
		if (state == AttackState.none) {
			if (Input.GetAxis ("Attack") > 0) {
				state = (AttackState)currentWep.GetComponent<WeaponInfo> ().attackState;
				PlayerManager.Instance.attacking = true;
                //attackTimer = currentWep.GetComponent<WeaponInfo> ().swingTime;
                // sets attack timer to length of animation adjusted by the animations play speed
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
                Debug.Log("stop-");
				attackTimer = 0;
				state = AttackState.none;
				PlayerManager.Instance.attacking = false;
				MovementManager.Instance.canMove = true;
			}
		}

        if (state == AttackState.none)
        {
            if (Input.GetAxis("Attack") > 0)
            {
                state = (AttackState)currentWep.GetComponent<WeaponInfo>().attackState;
                PlayerManager.Instance.attacking = true;

                
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                Debug.Log("stop-");
                attackTimer = 0;
                state = AttackState.none;
                PlayerManager.Instance.attacking = false;
                MovementManager.Instance.canMove = true;
            }
        }

        twoHand = currentWep.GetComponent<WeaponInfo> ().twoHand;

		stateNum = (int)state;

		anim.SetInteger ("AttackState", stateNum);
		anim.SetBool ("TwoHand", twoHand);
	}*/

    void Update()
    {
        //every frame
        currentWep = PlayerManager.Instance.currentWep;
        twoHand = currentWep.GetComponent<WeaponInfo>().twoHand;
        anim.SetBool("TwoHand", twoHand);

        /*
        //conditional
        if (!PlayerManager.Instance.attacking && Input.GetAxis("Attack") > 0)
        {
            state = (AttackState)currentWep.GetComponent<WeaponInfo>().attackState;
            PlayerManager.Instance.attacking = true;
            timeSet = false;
            notShot = true;
            attackTime = 0;
            timer = 0;
        }
        */
        stateNum = (int)state;
        anim.SetInteger("AttackState", stateNum);
    }

    //happens after animations updated. 
    //This is due to finding the length of the animation via the mecanim animator, which is only updated btw update and lateupdate
    private void LateUpdate()
    {
        /*
        //once attacking
        if (PlayerManager.Instance.attacking)
        {
            animInfo = anim.GetCurrentAnimatorStateInfo(1);
            animClip = anim.GetCurrentAnimatorClipInfo(1);

            //will set the time for the animation if it's not set yet
            if (!coroutFired)
            {
                coroutFired = true;
                StartCoroutine(attack());
            }

            
        }
        */
    }
    /*
    private IEnumerator attack()
    {
        numCoroutines++;
        while (animClip.Length == 0)
        {
            yield return null;
        }
        attackTime = animClip[0].clip.length / animInfo.speed;
        timeSet = true;

        while(timer < attackTime) { 
            //will shoot a projectile at the appropriate time for weapons that require that
            if ((int)state == 3 && notShot && timer >= currentWep.GetComponent<WeaponInfo>().timeWhenShoot)
            {
                Vector3 force;
                if (PlayerManager.Instance.currentTarget != gameObject)
                    force = (PlayerManager.Instance.currentTarget.transform.position - currentWep.transform.position).normalized * currentWep.GetComponent<WeaponInfo>().projectileSpeed;
                else
                    force = PlayerManager.Instance.faceDir.normalized * currentWep.GetComponent<WeaponInfo>().projectileSpeed;

                currentWep.GetComponent<PlayerFire>().Launch(force);
                notShot = false;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        state = AttackState.none;
        //PlayerManager.Instance.attacking = false;
        timeSet = false;
        timer = 0;
        notShot = true;
        coroutFired = false;
        numCoroutines--;
    }
    */
}
