using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : Enemy
{
    private Vector3 desiredDir;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CheckAggro();
        if (state == EnemyState.Move)
            Move();

        attackTimer -= Time.deltaTime;
    }

    protected override void Move()
    {
        //these functions calculate a desired direction
        if (aggroed)
        {
            desiredDir = GetComponent<E_Pursue>().Pursue(target.transform.position);
            desiredDir.y = 0;
        }
        else
            desiredDir = GetComponent<E_Wander>().Wander(spawn, desiredDir);

        //-------applies desiredDir to the controller, moving the character-----------
        Vector3 deltaPos = Vector3.zero;
        if(desiredDir != Vector3.zero) //only lerps if movement is desired
            deltaPos = Vector3.Lerp(transform.forward, desiredDir.normalized, 0.2f);
        deltaPos *= speed;
        float netSpeed = deltaPos.magnitude;
        if(deltaPos != Vector3.zero)
            transform.forward = deltaPos;
        deltaPos += Physics.gravity;
        controller.Move(deltaPos * Time.deltaTime);
        //-------END MOVEMENT---------------------------------------------------------

        anim.SetFloat("speed", netSpeed);

        CheckAttack();
    }

    protected override void CheckAggro()
    {
        if (aggroed)
        {
            //checks if it should deaggro
            if ((target.transform.position - transform.position).magnitude > 10f)
                aggroed = false;
            speed = 2.0f;
        }
        else
        {
            //checks if it should aggro
            aggroed = GetComponent<E_FieldOfView>().CheckFOV();
            speed = 1.0f;
        }

        anim.SetFloat("walkSpeedMult", speed);
    }

    void CheckAttack()
    {
        //close up attack
        if(aggroed && (target.transform.position - transform.position).magnitude <= 2f && attackTimer < 0)
        {
            currentAttack = allAttacks[0];
            anim.SetTrigger(currentAttack.animTrigger);
            state = EnemyState.Attacking;
            attackTimer = 2f;
        }
    }

    //------------getting hurt----------------------
    private void OnTriggerEnter(Collider other)
    {
        {
            if(other.gameObject.layer == 9) //Player weapon layer
            {
                float damage = other.gameObject.GetComponentInParent<WeaponInfo>().damage;
                Hurt(damage);
                accruedHealth += damage;
                countHurt = true;
                aggroed = true;
            }
            else if (other.gameObject.layer == 12) //Player projectile layer
            {
                float damage = other.gameObject.GetComponent<ProjectileInfo>().damage;
                Hurt(damage);
                accruedHealth += damage;
                countHurt = true;
                aggroed = true;
            }
        }
    }
    //----------------END GETTING HURT-----------------
}
