using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Enemy
{
    private Vector3 desiredDir;

    //---------projectile variables---------------
    public GameObject projectileOrigin; //where projectiles are shot from
    private Queue<GameObject> inactiveProjectiles;
    private float shootSpeed = 7f;
    public GameObject projectile; //the projectile that the enemy shoots
    
    protected override void Start()
    {
        base.Start();

        //--------instantiates projectiles-----------
        inactiveProjectiles = new Queue<GameObject>();
        for(int i = 0; i<5; i++)
        {
            GameObject proj = GameObject.Instantiate(projectile);
            proj.GetComponent<ProjectileInfo>().homeQueue = inactiveProjectiles;
            inactiveProjectiles.Enqueue(proj);
        }
    }
    
    protected override void Update()
    {
        base.Update();
        CheckAggro();
        if (state == EnemyState.Move)
            Move();
        else if (state == EnemyState.Attacking)
            Aim();

        attackTimer -= Time.deltaTime;
    }

    protected override void Move()
    {
        if (aggroed)
            desiredDir = MoveIntelligence();
        else
            desiredDir = Vector3.zero;
            

        //-------applies desiredDir to the controller, moving the character-----------
        Vector3 deltaPos = Vector3.zero;
        if (desiredDir != Vector3.zero) //only lerps if movement is desired
            deltaPos = Vector3.Lerp(transform.forward, desiredDir.normalized, 0.2f);
        deltaPos *= speed;
        float netSpeed = deltaPos.magnitude;
        if (deltaPos != Vector3.zero)
            transform.forward = deltaPos;
        deltaPos += Physics.gravity;
        controller.Move(deltaPos * Time.deltaTime);
        //-------END MOVEMENT---------------------------------------------------------

        anim.SetFloat("speed", netSpeed);
    }

    protected override void CheckAggro()
    {
        if (aggroed)
        {
            //checks if it should deaggro
            if ((target.transform.position - transform.position).magnitude > 30f)
                aggroed = false;
        }
        else
        {
            //checks if it should aggro
            aggroed = GetComponent<E_FieldOfView>().CheckFOV();
        }
    }


    //determines where to move
    Vector3 MoveIntelligence()
    {
        Vector3 moveDir = Vector3.zero;
        Vector3 dirToTarget = target.transform.position - transform.position;
        float distToTarget = (dirToTarget).magnitude;

        //will move enemy closer if it's too far away, and farther away if it's too close
        if(distToTarget > 10f)
        {
            moveDir = GetComponent<E_Pursue>().Pursue(target.transform.position);
            moveDir.y = 0;
        }
        else if(distToTarget < 8f)
        {
            moveDir = GetComponent<E_Pursue>().Pursue(dirToTarget.normalized * 8f);
            moveDir.y = 0;
        }
        else
        {
            if(inactiveProjectiles.Count > 0 && attackTimer < 0)
            {
                Attack();
                attackTimer = 5f;
            }
        }

        return moveDir;
    }

    //begins the attack process. This is called the moment the enemy decides to attack
    void Attack()
    {
        currentAttack = allAttacks[0];
        anim.SetTrigger(currentAttack.animTrigger);
        state = EnemyState.Attacking;
    }

    //will face the target when aiming a shot
    void Aim()
    {
        Vector3 faceDir = target.transform.position - transform.position;
        faceDir.y = 0;
        faceDir = Vector3.Lerp(transform.forward, faceDir.normalized, 0.2f);
        transform.forward = faceDir;
    }


    //actually dequeues and fires a projectile. Called via animation event
    public override void Shoot()
    {
        Vector3 origin = projectileOrigin.transform.position;
        Vector3 shootDir = (target.transform.position - origin).normalized;

        GameObject projToShoot = inactiveProjectiles.Dequeue();
        projToShoot.transform.position = origin;
        projToShoot.GetComponent<ProjectileInfo>().ToggleActive(true);
        projToShoot.GetComponent<Rigidbody>().AddForce(shootDir * shootSpeed);
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
