using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapEffigy : MonoBehaviour
{
    public GameObject punkin;
    public GameObject head;

    GameObject headStartpoint;

    public float tossPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PunkinToHead()
    {
        headStartpoint = punkin.GetComponent<ThrowHead>().headStartpoint;
        punkin.SetActive(false);
        punkin.transform.position = Vector3.zero;
        head.transform.position = headStartpoint.transform.position;
        head.SetActive(true);
        head.GetComponent<Rigidbody>().isKinematic = false;
        head.GetComponent<Rigidbody>().AddForce(headStartpoint.transform.forward * tossPower);
    }

    public void HeadToPunkin()
    {
        head.GetComponent<Rigidbody>().isKinematic = true;
        punkin.transform.position = head.transform.position; //might run into trouble accessing transform of inactive gameobject
        head.transform.position = Vector3.zero;
        head.SetActive(false);
        punkin.SetActive(true);

    }
}
