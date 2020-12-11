using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarseBehavior : MonoBehaviour
{
    private float speed = 50f;

    public Transform parent;
    public Transform bottle;
    public Transform pop;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Float();
    }

    //Handles all movement of the pop bottle
    void Float()
    {
        float time = Time.time;
        float sine = Mathf.Sin(time);
        float doubleSine = Mathf.Sin(time * 2f);
        parent.transform.rotation = Quaternion.Euler(new Vector3(0, 0, doubleSine * 15f));
        parent.transform.position = transform.position + Vector3.up * (0.5f + (.25f * sine));

        bottle.transform.Rotate(0, Time.deltaTime * speed, 0);
        pop.transform.Rotate(0, Time.deltaTime * speed * -2f, 0);
    }
}
