using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "HurtBox")
        {
            if(other.gameObject.layer == 10) //if hurtbox is on enemy layer
            {
                EnemyLogic enemy = other.GetComponentInParent<EnemyLogic>();
                if (enemy.currentAttack.getAttacking() && enemy.currentAttack.getCanHurt())
                {
                    PlayerManager.Instance.currentHealth -= enemy.currentAttack.damage;

                    Vector3 direction = transform.position - other.transform.root.position;
                    direction.y = 100.0f;
                    direction *= 5.0f * enemy.currentAttack.knockbackMult;
                    float time = 0.2f;
                    object[] array = { direction, time };
                    SendMessage("PlayerKnockback", array);
                }
            }
            else if (other.gameObject.layer == 11) //if hurtbox is on enemyProjectile layer
            {
                ProjectileInfo projectile = other.GetComponentInParent<ProjectileInfo>();

                PlayerManager.Instance.currentHealth -= projectile.damage;

                Vector3 direction = transform.position - other.transform.root.position;
                direction.y = 100.0f;
                direction *= 5.0f * projectile.knockbackMult;
                float time = 0.2f;
                object[] array = { direction, time };
                SendMessage("PlayerKnockback", array);
            }
        }
    }
}
