using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveRB : MonoBehaviour {

    public Rigidbody rb;
    public Collider col;
    public Animator anim;
    private Vector3 netForce; //this only deals with movement on xz plane
    private Vector3 acceleration;
    private GameObject gameManager;

    public bool grounded = false;
    private bool jumped = false;
    public float maxJumpVel = 1f;
    public float minJumpVel = 0.1f;
    public float fallSpeedMult = 2.5f;
    public float smallJumpMult = 1.0f;
    public float maxSpeed = 0.5f;
    public float maxForce = 10f;
    public float force = 2.0f;
    public float mu = 0.9f;
    public GameObject text;
    public float turnSpeed = 1.0f;

    public enum States {
        Standing,
        Jumping,
        Moving,
        Swinging
    }

    public States currentState = States.Standing;
    public States prevState = States.Standing;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        //anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        
        move();
        //handleAnimation();
        handleState();
        prevState = currentState;

	}

    void FixedUpdate()
    { //jumping code from https://www.youtube.com/watch?v=acBCegN60kw and related vids
        if (jumped)
        {
            jump(); //has physics calculations in it
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeedMult - 1);
        }
        else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (smallJumpMult - 1);
        }
    }

    void move()
    {
        checkGround();
        handleInput();
        applyFriction();
        netForce = Vector3.ClampMagnitude(netForce, maxForce);
        acceleration = Vector3.ClampMagnitude(netForce / rb.mass * Time.deltaTime, maxSpeed);
        text.GetComponent<TextMesh>().text = netForce.ToString();
        rb.MovePosition(rb.position + acceleration);
    }

    void checkGround()
    {
        grounded = false;
        int layerMask = 1 << 8;

        

        //an array of the bottom 4 corners of the collider
        Vector3[] points =
        {
            new Vector3(col.bounds.max.x, col.bounds.center.y, col.bounds.max.z),
            new Vector3(col.bounds.min.x, col.bounds.center.y, col.bounds.max.z),
            new Vector3(col.bounds.max.x, col.bounds.center.y, col.bounds.min.z),
            new Vector3(col.bounds.min.x, col.bounds.center.y, col.bounds.min.z)
        };

        for (int i = 0; i < points.Length; i++)
        {
            
            Debug.DrawRay(points[i], Vector3.down * (col.bounds.extents.y + 0.5f), Color.red, 0, false);
            if (Physics.Raycast(points[i], Vector3.down, col.bounds.extents.y + 0.5f, layerMask, QueryTriggerInteraction.Collide))
                grounded = true;
        }

        
    }

    void applyFriction()
    {
        Vector3 friction = netForce.normalized * -1;
        float normal = 1;
        friction *= normal * mu;

        addForce(friction);

        if (netForce.sqrMagnitude <= Mathf.Pow(0.6f, 2.0f))
        {
            netForce = new Vector3(0, 0, 0);
        }
    }

    void handleInput()
    {
        if (Input.GetKey(KeyCode.W))
            addForce(new Vector3(-force, 0, 0));
        if (Input.GetKey(KeyCode.S))
            addForce(new Vector3(force, 0, 0));
        if (Input.GetKey(KeyCode.A))
            addForce(new Vector3(0, 0, -force));
        if (Input.GetKey(KeyCode.D))
            addForce(new Vector3(0, 0, force));
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            jumped = true;

        Vector3 intendedForward = Vector3.Normalize(new Vector3(netForce.x, 0, netForce.z));
        if(intendedForward != Vector3.zero)
        {
            float step = turnSpeed * Time.deltaTime;
            transform.forward = Vector3.RotateTowards(transform.forward, intendedForward, step, 0.0f);
        }
    }

    void addForce(Vector3 force)
    {
        netForce += force;
    }

    void jump()
    {
        jumped = false;
        rb.velocity = new Vector3(rb.velocity.x, maxJumpVel, rb.velocity.z);

        /* Bad code?
        if (jumping)
        {
            currentState = States.Jumping;
            anim.SetTrigger("Jump");
        }
        if (currentState == States.Jumping && prevState != States.Jumping)
        {
            jumpVelocity = maxJumpForce;
            rb.useGravity = false;
        }

        if (!Input.GetKey(KeyCode.Space) && jumpVelocity > minJumpForce)
            jumpVelocity = minJumpForce;
        */
    }

    void handleAnimation()
    {
        switch (currentState)
        {
            case States.Moving:
                anim.SetTrigger("Run");
                break;
            case States.Standing:
                anim.SetTrigger("Stay");
                break;
            case States.Jumping:
                anim.SetTrigger("Jump");
                break;
        }
    }

    void handleState()
    {
        if (grounded)
        {
            if (netForce == Vector3.zero)
            {
                currentState = States.Standing;
                //anim.SetTrigger("Stay");
            }
            else if (netForce != Vector3.zero)
            {
                currentState = States.Moving;
                //anim.SetTrigger("Run");
            }
        }

        if(rb.velocity.y > 0)
        {
            currentState = States.Jumping;
            //anim.SetTrigger("Jump");
        }
    }
    /*bad code?
    void fixedJump()
    {
        Vector3 position = rb.position;

        if(jumpVelocity != 0)
        {
            position.y += jumpVelocity;
            jumpVelocity -= jumpDegrade;
            if(jumpVelocity <= 0)
            {
                rb.useGravity = true;
                jumpVelocity = 0;
            }
        }

        rb.MovePosition(position);
    }
    */
}
