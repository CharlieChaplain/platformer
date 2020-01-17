using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour {

	public Animator anim;
	public EnemyInfo enemyInfo;

	public ParticleSystem hitParticles;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        enemyInfo = GetComponent<EnemyInfo> ();
        hitParticles = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {

        //if the trigger that entered is a hurtbox
        if (other.tag == "HurtBox") {
            //if the trigger that entered is a player projectile
            if(other.gameObject.layer == 12) //12 = PlayerProjectile
            {
                getHurt(other.GetComponent<ProjectileInfo>().damage, other);
            }
            //else if non projectile and player is currently attacking. (attacking doesn't matter because the player does no damage with ranged, only the projectile does)
            else
            {
                getHurt(other.transform.parent.GetComponent<WeaponInfo>().damage, other);
            }
		}
	}

	void resetState () {
		anim.SetInteger ("state", 0);
		enemyInfo.enemyState = EnemyInfo.EnemyState.Move;
	}

	void getHurt(float damage, Collider other){
        Vector3 dir;
        if (other.transform.root != null)
            dir = transform.position - other.transform.root.position;
        else
            dir = transform.position - other.transform.position;
        dir.Normalize();
        SendMessage("BounceBack", dir);
        anim.SetInteger("state", 1);
        enemyInfo.enemyState = EnemyInfo.EnemyState.Hit;

		enemyInfo.currentHealth -= damage;

        if (hitParticles != null)
        {
            hitParticles.Emit(15);
        }

        if (GetComponent<EnemyInfo>() != null)
        {
            GetComponent<EnemyInfo>().aggroed = true;
        }
    }
}
