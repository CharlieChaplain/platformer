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

    public Queue<GameObject> homeQueue;

    private void Awake()
    {
        active = false;
        hurtBox.enabled = active;
        meshRender.enabled = active;
        if (trail != null)
            trail.enabled = active;
        if(gravityAffected)
            rb.useGravity = active;
        else
            rb.useGravity = false;
    }

    //used to turn the projectile on and off
    public void ToggleActive(bool activity)
    {
        active = activity;
        hurtBox.enabled = activity;
        meshRender.enabled = activity;

        //if the projectile has a trail, toggle it. If it needs disabling, wait a bit for the trail to deteriorate
        if(trail != null)
        {
            if (active)
            {
                trail.Clear();
                trail.enabled = true;
            }
            else
                StartCoroutine("DisableTrail");
        }
            

        if (!active)
        {
            rb.useGravity = false; //if projectile is inactive, don't let it fall forever.
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.useGravity = gravityAffected; //if projectile uses gravity when active, set it so when proj becomes active
        }
    }

    IEnumerator EnableTrail()
    {
        //waits for a number of frames before enabling the trail
        for(int i = 0; i < 3; i++)
            yield return null;
        trail.Clear();
        trail.enabled = true;
    }

    IEnumerator DisableTrail()
    {
        yield return new WaitForSeconds(0.8f);
        trail.enabled = false;
    }
}
