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
                float damage = other.gameObject.transform.root.GetComponentInChildren<Enemy>().currentAttack.damage;
                PlayerManager.Instance.currentHealth -= damage;
            }
            else if (other.gameObject.layer == 11) //if hurtbox is on enemyProjectile layer
            {
                ProjectileInfo projectile = other.GetComponentInParent<ProjectileInfo>();
                PlayerManager.Instance.currentHealth -= projectile.damage;
            }
        }
    }
}
