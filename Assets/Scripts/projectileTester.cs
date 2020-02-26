using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileTester : MonoBehaviour
{
    private Vector3 home;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        home = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = home + new Vector3(Mathf.Sin(Time.time * speed), 0, Mathf.Cos(Time.time * speed));
        transform.position = pos;
    }
}
