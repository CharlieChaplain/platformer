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
        if(other.tag == "HurtBox" && other.gameObject.layer == 10)
        {
            EnemyLogic enemy = other.GetComponentInParent<EnemyLogic>();
            if (enemy.currentAttack.getAttacking() && enemy.currentAttack.getCanHurt())
            {
                PlayerManager.Instance.currentHealth -= enemy.currentAttack.damage;

                Vector3 direction = transform.position - other.transform.root.position;
                direction *= 2.0f;
                float time = 0.2f;
                object[] array = { direction, time };
                SendMessage("PlayerKnockback", array);
            }
        }
    }
}
