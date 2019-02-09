using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogah : EnemyLogic {

	// Use this for initialization
	void Start () {
        enemyInfo = GetComponent<EnemyInfo>();
        currentAttack = attacks[0];
	}
	
	// Update is called once per frame
	void Update () {
        float dist = 1.8f;
		if((transform.position - player.transform.position).sqrMagnitude <= dist * dist &&
            enemyInfo.enemyState != EnemyInfo.EnemyState.Attacking)
        {
            enemyInfo.enemyState = EnemyInfo.EnemyState.Attacking;
            currentAttack.StartAttack();
        }

        if(enemyInfo.enemyState == EnemyInfo.EnemyState.Attacking)
        {
            if(currentAttack.getTimer() >= currentAttack.time)
            {
                enemyInfo.enemyState = EnemyInfo.EnemyState.Move;
            }
        }
	}
}
