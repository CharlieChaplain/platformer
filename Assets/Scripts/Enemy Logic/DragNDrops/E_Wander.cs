using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Wander : MonoBehaviour
{
    public float timer = 0; //the amount of time between changes (+ a random time between 1 and 3)
    public bool random = true; //determines if walk/wait toggles or is randomly chosen
    public float waitWeight; //determines how often enemy waits vs walks if random is true;

    private float wanderTimer = 0;
    private bool standStill = true;

    public Vector3 Wander(Vector3 spawn, Vector3 currentDir)
    {
        Vector3 desiredDir = currentDir;
        if (wanderTimer <= 0)
        {
            wanderTimer = timer + Random.Range(1f, 3f);
            if ((spawn - transform.position).magnitude >= 10f)
            {
                desiredDir = spawn - transform.position;
            }
            if (!standStill)
            {
                if(!random)
                    standStill = !standStill; //just toggles back each time wanderTimer is up
                else
                {
                    float hotOrNot = Random.Range(0, 1f);
                    if(hotOrNot <= waitWeight) //toggles back to waiting
                        standStill = !standStill;
                }
                desiredDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            }
            else
            {
                if(!random)
                    standStill = !standStill; //just toggles back each time wanderTimer is up
                else
                {
                    float hotOrNot = Random.Range(0, 1f);
                    if (hotOrNot >= waitWeight) //toggles back to walking around
                        standStill = !standStill;
                }
                desiredDir = Vector3.zero;
            }
        }
        wanderTimer -= Time.deltaTime;


        return desiredDir;
    }
}