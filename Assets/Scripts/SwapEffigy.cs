using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapEffigy : MonoBehaviour
{
    public GameObject punkin;
    public GameObject head;
    public GameObject bigPunkin;
    public GameObject dollPunkin;

    public GameObject noheadPunkin; //leaves these behind when punkin throws their head. Used to leap back up onto body
    public GameObject noheadBigPunkin;
    public GameObject noheadDollPunkin;

    private GameObject thisNoHeadPunkin;

    public float tossPower;

    void EnableHead(Vector3 headStartpoint, Vector3 headDir)
    {
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
        headRB.AddForce(headDir);

        //changes reference in playermanager
        PlayerManager.Instance.currentType = PlayerManager.PunkinType.headPunkin;
        PlayerManager.Instance.player = head;

        //changes camera
        CameraManager.ChangeCamera("JustHeadCamera");
        float angle = Mathf.Atan2(facing.x, facing.z) * Mathf.Rad2Deg;
        CameraManager.Instance.ChangeCurrentAngle(angle, true);
    }

    void DisableHead()
    {
        //disables head punkin
        head.SetActive(false);
        Rigidbody headRB = head.GetComponent<Rigidbody>();
        headRB.isKinematic = true;
        headRB.useGravity = false;
        StartCoroutine(WaitThenTPToOrigin(head, 0.8f));
    }

    public void PunkinToHead()
    {
        //gets copies of initial pos and forward vectors.
        Transform startTrans = punkin.GetComponent<ThrowHead>().headStartpoint.transform;
        Vector3 headStartpoint = new Vector3(startTrans.position.x, startTrans.position.y, startTrans.position.z);
        Vector3 headDir = punkin.GetComponent<ThrowHead>().aimDirection * tossPower;

        //disables normal punkin
        punkin.SetActive(false);
        StartCoroutine(WaitThenTPToOrigin(punkin, 1.2f));
        punkin.GetComponent<RotateHips>().rotate = false;

        //enables head
        EnableHead(headStartpoint, headDir);

        //puts down a nohead punkin
        thisNoHeadPunkin = GameObject.Instantiate(noheadPunkin, punkin.transform.position, punkin.transform.rotation);
    }

    public void HeadToPunkin(GameObject effigyToDelete)
    {
        //gets starting position and current forward
        Vector3 startPos = new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z);
        Vector3 startForward = effigyToDelete.transform.forward;

        //disables head punkin
        DisableHead();

        //enables normal punkin
        punkin.transform.position = startPos;
        punkin.transform.forward = startForward;
        punkin.SetActive(true);
        punkin.GetComponentInChildren<Animator>().Play("Base Layer.Punk_Standup_E");
        PlayerManager.Instance.SpawnWeapon();
        PlayerManager.Instance.canAttack = true;

        //changes reference in playermanager
        PlayerManager.Instance.currentType = PlayerManager.PunkinType.basePunkin;
        PlayerManager.Instance.player = punkin;

        //changes camera
        Vector3 facing = new Vector3(startForward.x, 0, startForward.z).normalized;
        CameraManager.ChangeCamera("PunkinCamera");
        float angle = Mathf.Atan2(facing.x, facing.z) * Mathf.Rad2Deg;
        CameraManager.Instance.CurrentCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value = angle;

        //deletes nohead punkin
        GameObject.Destroy(effigyToDelete);

        MovementManager.Instance.canMove = true;
    }

    public void BigPunkToHead()
    {
        //gets copies of initial pos and forward vectors.
        Transform startTrans = bigPunkin.GetComponent<DropHead>().headStartpoint.transform;
        Vector3 headStartpoint = new Vector3(startTrans.position.x, startTrans.position.y, startTrans.position.z);
        Vector3 headDir = bigPunkin.transform.forward * 50.0f;

        //disables big punkin
        bigPunkin.SetActive(false);
        StartCoroutine(WaitThenTPToOrigin(bigPunkin, 1.2f));

        //enables head
        EnableHead(headStartpoint, headDir);

        //puts down a nohead big punkin
        thisNoHeadPunkin = GameObject.Instantiate(noheadBigPunkin, bigPunkin.transform.position, bigPunkin.transform.rotation);
    }

    public void HeadToBigPunk(GameObject effigyToDelete)
    {
        //gets starting position and current forward
        Vector3 startPos = new Vector3(effigyToDelete.transform.position.x, effigyToDelete.transform.position.y, effigyToDelete.transform.position.z);
        Vector3 startForward = effigyToDelete.transform.forward;

        //disables head punkin
        DisableHead();

        //enables big punkin
        bigPunkin.transform.position = startPos;
        bigPunkin.transform.forward = startForward;
        bigPunkin.SetActive(true);
        bigPunkin.GetComponentInChildren<Animator>().Play("Base Layer.BigPunk_Standup_E");
        PlayerManager.Instance.canAttack = true;

        //changes reference in playermanager
        PlayerManager.Instance.currentType = PlayerManager.PunkinType.bigPunkin;
        PlayerManager.Instance.player = bigPunkin;

        //changes camera
        Vector3 facing = new Vector3(startForward.x, 0, startForward.z).normalized;
        CameraManager.ChangeCamera("BigPunkinCamera");
        float angle = Mathf.Atan2(facing.x, facing.z) * Mathf.Rad2Deg;
        CameraManager.Instance.CurrentCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value = angle;

        //deletes nohead punkin
        GameObject.Destroy(effigyToDelete);

        MovementManager.Instance.canMove = true;
    }
    public void DollPunkToHead()
    {
        //gets copies of initial pos and forward vectors.
        Transform startTrans = dollPunkin.GetComponent<DropHead>().headStartpoint.transform;
        Vector3 headStartpoint = new Vector3(startTrans.position.x, startTrans.position.y, startTrans.position.z);
        Vector3 headDir = dollPunkin.transform.forward * 50.0f;

        //disables doll punkin
        dollPunkin.SetActive(false);
        StartCoroutine(WaitThenTPToOrigin(dollPunkin, 1.2f));

        //enables head
        EnableHead(headStartpoint, headDir);

        //puts down a nohead big punkin
        thisNoHeadPunkin = GameObject.Instantiate(noheadDollPunkin, dollPunkin.transform.position, dollPunkin.transform.rotation);
    }

    public void HeadToDollPunk(GameObject effigyToDelete)
    {
        //gets starting position and current forward
        Vector3 startPos = new Vector3(effigyToDelete.transform.position.x, effigyToDelete.transform.position.y, effigyToDelete.transform.position.z);
        Vector3 startForward = effigyToDelete.transform.forward;

        //disables head punkin
        DisableHead();

        //enables big punkin
        dollPunkin.transform.position = startPos;
        dollPunkin.transform.forward = startForward;
        dollPunkin.SetActive(true);
        dollPunkin.GetComponentInChildren<Animator>().Play("Base Layer.DollPunk_Getup_E");
        PlayerManager.Instance.canAttack = true;

        //changes reference in playermanager
        PlayerManager.Instance.currentType = PlayerManager.PunkinType.dollyPunkin;
        PlayerManager.Instance.player = dollPunkin;

        //changes camera
        Vector3 facing = new Vector3(startForward.x, 0, startForward.z).normalized;
        CameraManager.ChangeCamera("DollPunkinCamera");
        float angle = Mathf.Atan2(facing.x, facing.z) * Mathf.Rad2Deg;
        CameraManager.Instance.CurrentCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value = angle;

        //deletes nohead punkin
        GameObject.Destroy(effigyToDelete);

        MovementManager.Instance.canMove = true;
    }

    IEnumerator WaitThenTPToOrigin(GameObject objToTP, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        objToTP.transform.position = Vector3.zero;
    }
}
