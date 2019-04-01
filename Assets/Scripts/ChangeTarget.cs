using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTarget : MonoBehaviour {

    Camera sceneCam;

    public float doubleClickTimer;
    private bool clicked;
    private bool held;

	// Use this for initialization
	void Start () {
        sceneCam = Camera.main;
        clicked = false;
        held = false;
        doubleClickTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //won't fire unless weapon is ranged. Otherwise target is reset to player
        if (PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().ranged)
        {
            // will send out a ray to lock on to enemy you're looking at
            if (Input.GetAxis("LockOn") > 0 && doubleClickTimer == 0)
            {
                SendThickRay();
                clicked = true;
                held = true;
            }

            if (Input.GetAxis("LockOn") <= 0)
                held = false;

            // checks for double click. If so, resets the target to the player.
            if (clicked)
            {
                if (Input.GetAxis("LockOn") > 0 && doubleClickTimer <= 0.5f && doubleClickTimer >= 0.05f && !held)
                {
                    ResetTarget();
                }
                if (doubleClickTimer > 0.5f)
                {
                    clicked = false;
                    doubleClickTimer = 0.0f;
                }
            }


            if (clicked)
            {
                doubleClickTimer += Time.deltaTime;
            }

            //will reset target if player is out of range
            if (PlayerManager.Instance.currentTarget != gameObject &&
                Vector3.Magnitude(PlayerManager.Instance.currentTarget.transform.position - transform.position) > PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().range)
            {
                ResetTarget();
            }
        }
        else
        {
            PlayerManager.Instance.currentTarget = gameObject;
        }

        
	}

    void SendThickRay()
    {
        RaycastHit hit;

        LayerMask mask = LayerMask.GetMask("Enemy");

        if(Physics.SphereCast(transform.position, 5.0f, transform.position - sceneCam.transform.position, out hit,
            PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().range, mask, 0))
        {
            if (hit.transform.root.Find("LookAtMe").gameObject != null)
                PlayerManager.Instance.currentTarget = hit.transform.root.Find("LookAtMe").gameObject;
            else
                PlayerManager.Instance.currentTarget = hit.transform.gameObject;

        }
    }

    void ResetTarget()
    {
        PlayerManager.Instance.currentTarget = gameObject;
    }
}
