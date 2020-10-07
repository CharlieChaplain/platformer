using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasp : Enemy
{
    private Vector3 desiredDir;
    public bool grounded = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        allSounds[0].PlaySound();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CheckAggro();
        if (state == EnemyState.Move)
            Move();
        else if(state == EnemyState.Dead)
        {
            controller.enabled = false;
            if (!grounded)
            {
                controller.enabled = true;
                controller.radius = 0.15f;
                controller.height = 0.3f;
                Fall();
            }
            int layerMask = 1 << 8;
            grounded = Physics.Raycast(transform.position, Vector3.down, 0.16f, layerMask);

            if (grounded)
            {
                anim.SetTrigger("grounded");
            }
        }

        attackTimer -= Time.deltaTime;
    }

    protected override void Move()
    {
        if (aggroed)
            desiredDir = GetComponent<E_Pursue>().Pursue(target.transform.position);
        else
            desiredDir = GetComponent<E_Wander>().Wander(spawn, desiredDir);

        //-------applies desiredDir to the controller, moving the character-----------
        Vector3 deltaPos = Vector3.zero;
        if (desiredDir != Vector3.zero) //only lerps if movement is desired
            deltaPos = Vector3.Lerp(transform.forward, desiredDir.normalized, 0.2f);
        deltaPos *= speed;
        float netSpeed = deltaPos.magnitude;
        if (deltaPos != Vector3.zero)
            transform.forward = deltaPos;
        controller.Move(deltaPos * Time.deltaTime);
        //-------END MOVEMENT---------------------------------------------------------

        anim.SetBool("aggro", aggroed);
        CheckAttack();
    }

    protected override void CheckAggro()
    {
        if (aggroed)
        {
            //checks if it should deaggro
            if ((target.transform.position - transform.position).magnitude > 10f)
                aggroed = false;
        }
        else
        {
            //checks if it should aggro
            aggroed = GetComponent<E_FieldOfView>().CheckFOV();
        }
    }

    void CheckAttack()
    {
        //close up attack
        if (aggroed && (target.transform.position - transform.position).magnitude <= 1.5f && attackTimer < 0)
        {
            currentAttack = allAttacks[0];
            anim.SetTrigger(currentAttack.animTrigger);
            state = EnemyState.Attacking;
            attackTimer = 1f;
        }
    }

    //happens after wasp dies, and before it hits the ground
    void Fall()
    {
        desiredDir = Physics.gravity * 0.05f;
        controller.Move(desiredDir * Time.deltaTime);
        transform.forward = new Vector3(Mathf.Sin(Time.time * 5f), 0, Mathf.Cos(Time.time * 5f));
    }

    //------------getting hurt----------------------
    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.layer == 9) //Player weapon layer
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
