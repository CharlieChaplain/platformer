using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatwoodsMonster : Enemy
{
    public bool isActive;

    protected override void Start()
    {
        base.Start();
        isActive = false;
    }

    protected override void Update()
    {
        base.Update();
        CheckAggro();
        if (isActive && state == EnemyState.Move)
            Move();

    }

    protected override void Move()
    {
        Vector3 moveDir = GetComponent<E_Pursue>().Pursue(target.transform.position);
        controller.Move(moveDir * base.speed * Time.deltaTime);
        transform.forward = moveDir;
    }
}