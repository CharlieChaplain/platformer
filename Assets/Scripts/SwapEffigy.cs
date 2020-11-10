using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapEffigy : MonoBehaviour
{
    public GameObject punkin;
    public GameObject head;

    public GameObject noheadPunkin; //leaves this behind when punkin throws their head. Used to leap back up onto body

    private GameObject thisNoHeadPunkin;

    public float tossPower;

    public void PunkinToHead()
    {
        //gets copies of initial pos and forward vectors.
        Transform startTrans = punkin.GetComponent<ThrowHead>().headStartpoint.transform;
        Vector3 headStartpoint = new Vector3(startTrans.position.x, startTrans.position.y, startTrans.position.z);
        Vector3 headDir = punkin.GetComponent<ThrowHead>().aimDirection;

        //disables normal punkin
        punkin.SetActive(false);
        StartCoroutine("WaitThenTPToOrigin", punkin);

        //enables head punkin
        head.SetActive(true);
        Rigidbody headRB = head.GetComponent<Rigidbody>();
        RollerMovementTest headRMT = head.GetComponent<RollerMovementTest>();
        headRB.position = headStartpoint;
        headRB.useGravity = true;
        headRMT.speedLimit = false;
        headRMT.justThrown = true;

        Vector3 facing = new Vector3(headDir.x, 0, headDir.z).normalized;

        head.transform.forward = facing;
        headRB.isKinematic = false;
        headRB.AddForce(headDir * tossPower);

        //changes camera
        CameraManager.ChangeCamera("JustHeadCamera");
        float angle = Mathf.Atan2(facing.x, facing.z) * Mathf.Rad2Deg;
        CameraManager.Instance.CurrentCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value = angle;

        //puts down a nohead punkin
        thisNoHeadPunkin = GameObject.Instantiate(noheadPunkin, punkin.transform.position, punkin.transform.rotation);
    }

    public void HeadToPunkin()
    {
        //gets starting position and current forward
        Transform startTrans = head.transform;
        Vector3 startForward = thisNoHeadPunkin.transform.forward;

        //disables head punkin
        head.SetActive(false);
        Rigidbody headRB = head.GetComponent<Rigidbody>();
        headRB.isKinematic = true;
        headRB.useGravity = false;
        StartCoroutine("WaitThenTPToOrigin", head);

        //enables normal punkin
        punkin.transform.position = startTrans.position;
        punkin.transform.forward = startForward;
        punkin.SetActive(true);

        //changes camera
        Vector3 facing = new Vector3(startForward.x, 0, startForward.z).normalized;
        CameraManager.ChangeCamera("PunkinCamera");
        float angle = Mathf.Atan2(facing.x, facing.z) * Mathf.Rad2Deg;
        CameraManager.Instance.CurrentCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value = angle;

        //deletes nohead punkin
        GameObject.Destroy(thisNoHeadPunkin);

        MovementManager.Instance.canMove = true;
    }

    IEnumerator WaitThenTPToOrigin(GameObject objToTP)
    {
        yield return new WaitForSeconds(1.2f);
        objToTP.transform.position = Vector3.zero;
    }
}
