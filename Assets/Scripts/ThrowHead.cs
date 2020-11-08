using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHead : MonoBehaviour
{
    public Animator anim;

    private Transform waist;
    public GameObject headStartpoint; //rotate this gameobject when aiming, it determines the angle the head is thrown
    public Vector3 aimDirection;

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
            anim.SetTrigger("ThrowHead"); //this is what throws the head. An anim event activates in this anim that will do it
        }
    }

    void Aim()
    {
        aimDirection = transform.forward;
        aimDirection.y = .6f;
        aimDirection.Normalize();
    }
}
