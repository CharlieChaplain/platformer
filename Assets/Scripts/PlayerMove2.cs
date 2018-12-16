using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2 : MonoBehaviour {

    public Collider col;
    //public Animator anim;
    private Vector3 netForce; //this only deals with movement on xz plane
    private Vector3 velocity;
    private Vector3 acceleration;
    public GameObject gameManager;
    public GameObject playerObj;

    public bool grounded = false;
    public bool jumped = false;
    public float maxJumpVel = 1f;
    public float minJumpVel = 0.1f;
    public float fallSpeedMult = 2.5f;
    public float smallJumpMult = 1.0f;
    public float maxSpeed = 0.5f;
    public float maxForce = 10f;
    public float force = 2.0f;
    public float mu = 0.9f;
    public float mass = 1.0f;
    public GameObject text;
    public float turnSpeed = 1.0f;
    public float heightOffGround = 0.5f;

    public enum States
    {
        Standing,
        Jumping,
        Moving,
        Swinging
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        move();
	}

    void FixedUpdate()
    { //jumping code from https://www.youtube.com/watch?v=acBCegN60kw and related vids
        if (jumped)
        {
            jump(); //has physics calculations in it
        }

        if (velocity.y < 0) //determines if player is falling
        {
            applyGravity(fallSpeedMult - 1);
            //velocity += Vector3.up * Physics.gravity.y * (fallSpeedMult - 1);
        }
        else if (velocity.y > 0 && !Input.GetKey(KeyCode.Space)) //determines if player let go of jump button (tiny hop)
        {
            netForce = new Vector3(netForce.x, minJumpVel, netForce.z);
            //velocity += Vector3.up * Physics.gravity.y * (smallJumpMult - 1);
        }
    }

    //handles all movement
    void move()
    {
        applyGravity(1.0f);
        checkGround();
        handleInput();
        applyFriction();
        netForce = Vector3.ClampMagnitude(netForce, maxForce);
        acceleration = Vector3.ClampMagnitude(netForce / mass * Time.deltaTime, maxSpeed);
        velocity = acceleration * Time.deltaTime;
        
        playerObj.transform.position += acceleration;
        text.GetComponent<TextMesh>().text = playerObj.transform.position.y.ToString();
    }

    //handles collisions
    void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("collided");
        //ground layer. puts the player at ground level if collided with it
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("collidedGround");
            ContactPoint contact = collision.contacts[0];
            Vector3 conPos = contact.point;
            transform.position = new Vector3(transform.position.x, conPos.y + col.bounds.extents.y + heightOffGround, transform.position.z);
        }
        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collided");
    }

    //checks the 4 corners of the rigid body to determine if the player is on the ground
    void checkGround()
    {
        grounded = false;
        int layerMask = 1 << 8;

        //an array of the bottom 4 corners of the collider
        Vector3[] points =
        {
            new Vector3(col.bounds.max.x, col.bounds.extents.y, col.bounds.max.z),
            new Vector3(col.bounds.min.x, col.bounds.extents.y, col.bounds.max.z),
            new Vector3(col.bounds.max.x, col.bounds.extents.y, col.bounds.min.z),
            new Vector3(col.bounds.min.x, col.bounds.extents.y, col.bounds.min.z)
        };

        for (int i = 0; i < points.Length; i++)
        {
            Debug.DrawRay(points[i], Vector3.down, Color.red, 0, false);
            //collision code
            if (Physics.Raycast(points[i], Vector3.down, col.bounds.max.y + 0.5f, layerMask, QueryTriggerInteraction.Collide))
            {
                grounded = true;
            }
        }
    }

    //applies friction when the player is moving on the xz plane
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

    //applies gravity to player
    void applyGravity(float multiplier)
    {
        addForce(new Vector3(0, multiplier * gameManager.GetComponent<GameManager>().gravity, 0));
    }

    //handles input for force related math
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
        if (intendedForward != Vector3.zero)
        {
            float step = turnSpeed * Time.deltaTime;
            transform.forward = Vector3.RotateTowards(transform.forward, intendedForward, step, 0.0f);
        }
    }

    //helper function to add a force to the net force
    void addForce(Vector3 force)
    {
        netForce += force;
    }

    //jump function. Called in FixedUpdate
    void jump()
    {
        jumped = false;
        addForce(new Vector3(0, maxJumpVel, 0));
    }
}
