using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatwoodsMonster : EnemyLogic
{
    private CharacterController controller;
    private Animator anim;

    public float pursueSpeed;
    public float turnSpeed;

    private Vector3 direction;

    private GameObject target;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        enemyInfo = GetComponent<EnemyInfo>();
        //currentAttack = attacks[0];

        target = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Pursue();
            CheckDistance();
        }
    }

    //pursues the target's transform position
    void Pursue()
    {
        
        Vector3 intendedDir = (target.transform.position - transform.position).normalized;
        direction = Vector3.Lerp(transform.forward, intendedDir, Time.deltaTime * turnSpeed);
        direction.Normalize();

        transform.forward = direction;
        controller.Move(direction * pursueSpeed * Time.deltaTime);
    }

    //checks if they are too far away from player and deactivates them
    void CheckDistance()
    {
        if ((transform.position - target.transform.position).magnitude > 21.0f)
        {
            isActive = false;
        }
    }

}
