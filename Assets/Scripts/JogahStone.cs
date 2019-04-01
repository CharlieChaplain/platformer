using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogahStone : EnemyLogic
{

    private Animator anim;
    private int currentAttackNum;

    private float throwTimer = 3.0f; //time between throws

    // Use this for initialization
    void Start()
    {
        enemyInfo = GetComponent<EnemyInfo>();
        anim = GetComponent<Animator>();
        currentAttack = attacks[0];
        currentAttackNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyInfo.aggroed)
        {
            Attack();
        }
    }

    void Attack()
    {
        float dist = 1.8f;
        if (enemyInfo.enemyState != EnemyInfo.EnemyState.Attacking)
        {
            throwTimer -= Time.deltaTime;

            if ((transform.position - player.transform.position).sqrMagnitude <= dist * dist) //checks if player is close enough to be swiped at
            {
                currentAttackNum = 0;
                enemyInfo.enemyState = EnemyInfo.EnemyState.Attacking;
                currentAttack = attacks[currentAttackNum];
                anim.SetInteger("attack", currentAttackNum);
                anim.SetInteger("state", 2);
                currentAttack.StartAttack();
            }
            else if (throwTimer <= 0) //otherwise checks if throw timer is up, then will throw a stone
            {
                currentAttackNum = 1;
                enemyInfo.enemyState = EnemyInfo.EnemyState.Attacking;
                currentAttack = attacks[currentAttackNum];
                anim.SetInteger("attack", currentAttackNum);
                anim.SetInteger("state", 2);
                throwTimer = 3.0f;
                GetComponent<Tosser>().CreateProjectile();
                currentAttack.StartAttack();
            }
        }
        else
        {
            if (currentAttack.getTimer() >= currentAttack.time)
            {
                enemyInfo.enemyState = EnemyInfo.EnemyState.Move;
            }
        }
    }
}
