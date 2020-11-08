using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapEffigy : MonoBehaviour
{
    public GameObject punkin;
    public GameObject head;

    public GameObject noheadPunkin; //leaves this behind when punkin throws their head. Used to leap back up onto body

    Vector3 headStartpoint;
    Vector3 headDir;

    public float tossPower;

    public void PunkinToHead()
    {
        //gets copies of initial pos and forward vectors.
        Transform startTrans = punkin.GetComponent<ThrowHead>().headStartpoint.transform;
        headStartpoint = new Vector3(startTrans.position.x, startTrans.position.y, startTrans.position.z);
        headDir = punkin.GetComponent<ThrowHead>().aimDirection;

        //disables normal punkin
        punkin.SetActive(false);
        StartCoroutine("WaitThenTPToOrigin", punkin);

        //enables head punkin
        head.SetActive(true);
        Rigidbody headRB = head.GetComponent<Rigidbody>();
        headRB.position = headStartpoint;
        head.GetComponent<RollerMovementTest>().speedLimit = false;

        Vector3 facing = new Vector3(headDir.x, 0, headDir.z).normalized;

        head.transform.forward = facing;
        headRB.isKinematic = false;
        headRB.AddForce(headDir * tossPower);

        //changes camera
        CameraManager.ChangeCamera("JustHeadCamera");
        float angle = Mathf.Atan2(facing.x, facing.z) * Mathf.Rad2Deg;
        CameraManager.Instance.CurrentCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value = angle;

        //puts down a nohead punkin
        GameObject.Instantiate(noheadPunkin, punkin.transform.position, punkin.transform.rotation);
    }

    public void HeadToPunkin()
    {
        head.GetComponent<Rigidbody>().isKinematic = true;
        punkin.transform.position = head.transform.position; //might run into trouble accessing transform of inactive gameobject
        head.transform.position = Vector3.zero;
        head.SetActive(false);
        punkin.SetActive(true);

    }

    IEnumerator WaitThenTPToOrigin(GameObject objToTP)
    {
        yield return new WaitForSeconds(1.2f);
        objToTP.transform.position = Vector3.zero;
    }
}
