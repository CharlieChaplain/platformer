using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMoveController2 : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundMask;
    public Animator anim;

    private bool grounded;
    private bool doubleJumpPossible = true;

    public float jumpHeight;

    public float maxSpeed = 10f;
    private float speed = 0;
    public float accel = 1f;

    private Vector3 velocity; //used for jumping/falling, only y value will change
    public float gravityMult = 0.25f;
    public float upGravMult = 0.4f;
    public float downGravMult = 0.2f;
    private float terminalVelocity = -25.0f;

    public float turnSpeed = 0.1f;
    private float turnSmoothing;
    private float targetAngle = 0;

    private float hurtTimer;

    //used for attacking
    private GameObject currentWep; //the weapon currently being wielded. Taken from playermanager
    private bool twoHand; //is the current weapon two handed? Is passed into the animator as bool to change idle anim
    private int stateNum; //the type of animation the weapon uses. Is passed into the animator as an integer

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            CheckGrounded();

            if (PlayerManager.Instance.canMove)
            {
                Move();
                Jump();
            }

            ApplyGravity();

            if (PlayerManager.Instance.canAttack)
                CheckAttack();
        }
    }

    void Move()
    {
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horz, 0f, vert);

        if (direction.magnitude >= 0.1f)
        {
            //accelerates player
            speed += accel * direction.magnitude;
            speed = Mathf.Clamp(speed, 0, maxSpeed * direction.magnitude);

            //smooths player rotation and faces the player the correct direction based off of camera position
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothing, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
            //decelerates player
            speed -= accel * 6f;
            speed = Mathf.Clamp(speed, 0, maxSpeed);
        }



        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir * speed * Time.deltaTime);

        //animation variable connection
        anim.SetFloat("Speed", speed);

        //set face direction
        PlayerManager.Instance.faceDir = transform.forward;
    }

    void ApplyGravity()
    {
        float finalGravMult = gravityMult;
        if (velocity.y > 0)
            finalGravMult *= upGravMult;
        else if (velocity.y < 0)
            finalGravMult *= downGravMult;

        velocity += Physics.gravity * Time.deltaTime * finalGravMult;

        //prevents player from falling faster than terminal velocity
        if (velocity.y < terminalVelocity)
            velocity.y = terminalVelocity;

        controller.Move(velocity * Time.deltaTime);

        //animation variable connection
        anim.SetFloat("YSpeed", controller.velocity.y);
    }

    void CheckGrounded()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -1f;
            doubleJumpPossible = true;
        }

        //animation variable connection
        anim.SetBool("Grounded", grounded);
    }

    void Jump()
    {
        if (grounded || doubleJumpPossible)
        {
            if (Input.GetButtonDown("Jump"))
            {
                float mult = 1.0f;

                if (!grounded)
                {
                    doubleJumpPossible = false;
                    mult = 0.6f;
                }
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y * gravityMult) * mult;
            }
        }
    }

    //checks if player attacks. If so, it sends the current weapon type and a trigger to the animator
    void CheckAttack()
    {
        currentWep = PlayerManager.Instance.currentWep;

        //sends over the handedness of the current weapon to the animator
        twoHand = currentWep.GetComponent<WeaponInfo>().twoHand;
        anim.SetBool("TwoHand", twoHand);

        //triggers the attack animation if player can attack (not currently attacking and also possibly some other conditions)
        if (Input.GetAxis("Attack") > 0 && PlayerManager.Instance.canAttack)
        {
            //sends over the weapon attack state stored in the weaponinfo of the prefab to the animator
            stateNum = currentWep.GetComponent<WeaponInfo>().attackState;
            anim.SetInteger("AttackState", stateNum);

            //sends over trigger
            anim.SetTrigger("AttackTrigger");

            PlayerManager.Instance.canAttack = false;
        }
    }

    /* MESSY CODE, NEEDS REWORKING FOR THIS SYSTEM
    //initiates knocking the player backwards towards a specific direction
    void PlayerKnockback(object[] array)
    {
        hurtTimer = (float)array[1];
        Vector3 dir = (Vector3)array[0];
        Vector3 directionXZ = new Vector3(dir.x, 0.0f, dir.z);
        transform.forward = -directionXZ;
        moveDirection = direction;
        PlayerManager.Instance.canMove = false;
    }

    void checkKnockbackTimer()
    {
        if (hurtTimer > 0)
            hurtTimer -= Time.deltaTime;
        if (hurtTimer <= 0)
            PlayerManager.Instance.canMove = true;
    }
    */

}
