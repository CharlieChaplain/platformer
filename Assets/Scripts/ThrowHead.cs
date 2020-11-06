using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHead : MonoBehaviour
{
    public Animator anim;

    private Transform waist;
    public GameObject headStartpoint; //rotate this gameobject when aiming, it determines the angle the head is thrown

    // Start is called before the first frame update
    void Start()
    {
        waist = GetComponent<LookAtP>().joint;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("StartHeadAim");
            PlayerManager.Instance.currentWep.SetActive(false);
            MovementManager.Instance.canMove = false;
        }
        if (Input.GetMouseButton(1))
        {
            Aim();
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetTrigger("ThrowHead");
        }
    }

    void Aim()
    {

    }
}
