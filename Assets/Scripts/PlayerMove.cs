using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    private Vector3 position;
    private Vector3 netForce;
    private Vector3 acceleration;
    private Vector3 size;
    private GameObject gameManager;
    public float mass;
    public float maxSpeed;
    public float maxForce;
    public float force;
    public float mu;
    public GameObject text;

	// Use this for initialization
	void Start () {
        position = transform.position;
        gameManager = GameObject.Find("GameManager");
        size = GetComponent<BoxCollider>().size;
        text.GetComponent<TextMesh>().text = "bluh";
    }
	
	// Update is called once per frame
	void Update () {
        handleInput();
        move();
	}

    void move()
    {
        applyFriction(mu);
        applyGravity();
        netForce = Vector3.ClampMagnitude(netForce, maxForce);
        acceleration = netForce / mass * Time.deltaTime;
        position += Vector3.ClampMagnitude(acceleration, maxSpeed);
        transform.position = position;
    }

    void addForce(Vector3 force)
    {
        netForce += force;
    }

    void applyFriction(float u)
    {
        Vector3 friction = netForce.normalized * -1;
        float normal = 1;
        friction *= normal * u;

        addForce(friction);

        if (netForce.sqrMagnitude <= Mathf.Pow(0.3f, 2.0f))
        {
            netForce = new Vector3(0, 0, 0);
        }
    }

    void applyGravity()
    {
        addForce(new Vector3(0, gameManager.GetComponent<GameManager>().gravity, 0));
    }

    void handleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            addForce(new Vector3(-force, 0, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            addForce(new Vector3(force, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            addForce(new Vector3(0, 0, -force));
        }
        if (Input.GetKey(KeyCode.D))
        {
            addForce(new Vector3(0, 0, force));
        }
    }

    void OnCollisionEnter(Collision col)
    {
        text.GetComponent<TextMesh>().text = "collided";
        if(col.gameObject.layer == 8)
        {
            text.GetComponent<TextMesh>().text = "worked";
            float getThisY = GetComponent<Collider>().bounds.extents.y;
            float newY = col.collider.bounds.center.y + (col.collider.bounds.size.y / 2.0f) + (getThisY) + 0.3f;
            position = new Vector3(position.x, newY, position.z);
        }
    }
}
