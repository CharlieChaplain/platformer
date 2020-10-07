using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Pursue : MonoBehaviour
{
    public Vector3 Pursue(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        return dir;
    }
}
