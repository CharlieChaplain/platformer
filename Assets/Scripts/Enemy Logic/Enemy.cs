using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the base class that all enemies inherit from
public class Enemy : MonoBehaviour
{
    protected float currentHealth;
    public float maxHealth;
    public bool aggroed;
    public float speed;

    public List<EnemyAttack> allAttacks;
    public EnemyAttack currentAttack;

    public Animator anim;
    protected Vector3 spawn;
    public GameObject target;
    protected CharacterController controller;
    protected float attackTimer; //the time before another attack can be made

    public enum EnemyState
    {
        Move,
        Hurt,
        Attacking,
        Dead
    };

    public EnemyState state;

    //stuff that determines hurt (staggering)
    protected float accruedHealth; //the amount of health lost in a certain amount of time
    public float timeTilHurt; //the amount of time to deal enough damage to hurt (staggering)
    private float hurtTimer; //the timer that counts that time
    public float hurtThreshold; //the threshold for accrued health loss to reach in order to stagger
    protected bool countHurt = false; //determines whether timer should count;
        

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        spawn = transform.position;
        state = EnemyState.Move;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckDeath();
        CheckHurt();
    }

    void CheckDeath()
    {
        if(currentHealth <= 0)
        {
            state = EnemyState.Dead;
            anim.SetTrigger("dead");
            GetComponent<Collider>().enabled = false;
        }
    }

    void CheckHurt()
    {
        if (countHurt)
        {
            hurtTimer += Time.deltaTime;

            if (accruedHealth >= hurtThreshold)
            {
                state = EnemyState.Hurt;
                anim.SetTrigger("hurt");

                hurtTimer = 0;
                countHurt = false;
                accruedHealth = 0;
            }

            if (hurtTimer >= timeTilHurt)
            {
                hurtTimer = 0;
                countHurt = false;
                accruedHealth = 0;
            }
        }
    }

    protected virtual void Move() { }

    protected virtual void CheckAggro() { }

    public void ChangeState(string _state)
    {
        state = (EnemyState)System.Enum.Parse(typeof(EnemyState), _state);
    }

    public void Hurt(float damage)
    {
        currentHealth -= damage;
    }

    //used to send projectiles. Triggered by animation event
    public virtual void Shoot() { }
}
