using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : MonoBehaviour {

    public float knockbackMult;
    public float damage;
    public GameObject origin; //the gameobject that spawned the projectile
    public bool active = false;
    public bool gravityAffected; //is this projectile affected by gravity?
    public Collider hurtBox;
    public TrailRenderer trail;
    public MeshRenderer meshRender;
    public Rigidbody rb;

    private void Awake()
    {
        active = false;
        hurtBox.enabled = false;
        meshRender.enabled = false;
        if (trail != null)
            trail.enabled = false;
        rb.useGravity = false;
    }

    //used to turn the projectile on and off, and to add them back to 
	public void ToggleActive()
    {
        active = !active;
        hurtBox.enabled = !hurtBox.enabled;
        meshRender.enabled = !meshRender.enabled;

        //if the projectile has a trail, toggle it
        if(trail != null)
        {
            trail.Clear();
            trail.enabled = !trail.enabled;
        }
            

        if (!active)
        {
            GameManager.Instance.InactivePellets.Enqueue(gameObject);
            rb.useGravity = false; //if projectile is inactive, don't let it fall forever.
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.useGravity = gravityAffected; //if projectile uses gravity when active, set it so when proj becomes active
        }
    }
}
