using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_FieldOfView : MonoBehaviour
{
    public float fovAngle = 110f;
    public float fovRange = 5f;
    public float timeToAggro = 0; //the amount of time target needs to be within fov to aggro enemy

    private bool aggro = false;
    private GameObject target;

    private float aggroTimer = 0;

    void Awake()
    {
        target = GetComponent<Enemy>().target;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckFOV()
    {
        aggro = false;
        Vector3 dirToTarget = target.transform.position - transform.position;

        if (dirToTarget.magnitude <= fovRange)
        {
            float angle = Vector3.Angle(dirToTarget, transform.forward);
            if (angle < fovAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + (transform.up * 0.5f), dirToTarget.normalized, out hit, fovRange))
                {
                    if (hit.collider.gameObject == target)
                    {
                        aggroTimer += Time.deltaTime;
                        if(aggroTimer >= timeToAggro)
                        {
                            aggro = true;
                            aggroTimer = 0;
                        }
                    }
                }
            }
        }
        else
        {
            aggroTimer = 0;
        }

        return aggro;
    }
}
