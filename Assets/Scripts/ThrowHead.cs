using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowHead : MonoBehaviour
{
    public Animator anim;

    private Transform waist;
    public GameObject headStartpoint; //rotate this gameobject when aiming, it determines the angle the head is thrown
    public GameObject cameraAnchor; //the third person over the shoulder anchor
    public Vector3 aimDirection;

    private bool aiming;

    // Start is called before the first frame update
    void Start()
    {
        waist = GetComponent<LookAtP>().joint;
    }

    // Update is called once per frame
    void Update()
    {
        aiming = false;
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("StartHeadAim");
            PlayerManager.Instance.currentWep.SetActive(false);
            MovementManager.Instance.canMove = false;
            Vector3 OldCameraLook = CameraManager.Instance.SceneCamera.transform.forward;
            CameraManager.ChangeCamera("3rdPersonOvertheShoulderCamera");

            //will change the direction of the 3rdPersonOTS cam so it doesn't jarrigly flip around initially
            float angle = Mathf.Atan2(OldCameraLook.x, OldCameraLook.z) * Mathf.Rad2Deg;
            CameraManager.Instance.ChangeCurrentAngle(angle, true);

            PlayerManager.Instance.player.GetComponent<RotateHips>().rotate = true;
        }
        if (Input.GetMouseButton(1))
        {
            Aim();
            PlayerManager.Instance.player.GetComponent<RotateHips>().Rotation = Quaternion.LookRotation(aimDirection, Vector3.up);
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetTrigger("ThrowHead"); //this is what throws the head. An anim event activates in this anim that will do it
        }
    }

    void Aim()
    {
        aimDirection = CameraManager.Instance.SceneCamera.transform.forward;
        Vector3 Rot2D = aimDirection;
        Rot2D.y = 0;
        Rot2D.Normalize();
        cameraAnchor.transform.rotation = Quaternion.LookRotation(Rot2D);
    }
}
