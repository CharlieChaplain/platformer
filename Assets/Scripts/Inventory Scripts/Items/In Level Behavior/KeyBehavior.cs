using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    public GameObject model;

    Vector3 initPos;

    private void Start()
    {
        initPos = model.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = initPos;
        pos.y = initPos.y + (Mathf.Sin(Time.time * 2f) * .1f);

        model.transform.position = pos;
        model.transform.rotation = Quaternion.Euler(0, Time.time * 50f, 0);
    }
}
