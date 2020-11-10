using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerMovementTest : MonoBehaviour
{
    public bool grounded;
    public bool speedLimit; //whether or not to apply a top speed
    public bool justThrown; //whether punkin was just thrown or not
    public float jumpForce;//300
    public float speedMult;//30 //what the forces will be multiplied by before being added to rigidbody
    public float slowDown;//0.8
    public float maxSpeed;//4

    public LayerMask groundLayer;

    private bool jumping;

    public Transform cameraTrans; //the transform of the camera. Used to get forwards and rights

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        jumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        if(MovementManager.Instance.canMove)
            Move();
    }

    void Move()
    {
        Vector3 moveDirection;

        Vector3 previousVelocity = rigidbody.velocity;

        Vector3 cameraForward = cameraTrans.forward;
        Vector3 cameraRight = cameraTrans.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraForward * Input.GetAxis("Vertical") * speedMult) + (cameraRight * Input.GetAxis("Horizontal") * speedMult);
        rigidbody.AddForce(moveDirection);

        //clamps to max speed
        if (speedLimit && rigidbody.velocity.sqrMagnitude > maxSpeed * maxSpeed)
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);

        //slows down rigidbody if no input is being made
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0 && grounded)
            rigidbody.velocity *= slowDown;

        //will preserve the y velocity so it isn't affected by the clamping/slowdown
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, previousVelocity.y, rigidbody.velocity.z);

        //jump
        if(grounded && !jumping && Input.GetAxis("Jump") > 0)
        {
            jumping = true;
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
            StartCoroutine("toggleJumping");
        }
    }

    void CheckGround() //this check ground only works with the rigidbody and a sphere collider, won't work with character controller and pill collider
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.20f, groundLayer, QueryTriggerInteraction.Collide))
        {
            grounded = true;

            //first time hitting the ground after thrown
            if (justThrown)
            {
                justThrown = false;
                speedLimit = true;
                MovementManager.Instance.canMove = true;
            }
        }  
        else
            grounded = false;
    }

    //this coroutine waits a small amount of time to turn jumping back off, just to make sure we don't "double jump"
    IEnumerator toggleJumping()
    {
        yield return new WaitForSeconds(.1f);
        jumping = false;
        speedLimit = true;
    }
}
