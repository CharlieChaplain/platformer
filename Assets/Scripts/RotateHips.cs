using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script just allows other scripts to rotate punkin at the hips. Mainly the throw head and the locked on ranged weapon scripts.
public class RotateHips : MonoBehaviour
{
    public bool rotate;
    public Quaternion Rotation;

    public GameObject hips;

    // Start is called before the first frame update
    void Start()
    {
        rotate = false;
        Rotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
        if(rotate)
            hips.transform.rotation = Rotation;
    }
}
