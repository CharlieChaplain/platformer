using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for all effigies except base punkin
public class DropHead : MonoBehaviour
{
    public GameObject headStartpoint; //rotate this gameobject when aiming, it determines the angle the head is thrown

    void Update()
    {
        if (MovementManager.Instance.canMove && Input.GetMouseButtonDown(1))
        {
            MovementManager.Instance.canMove = false;
            GameObject empty = new GameObject();
            switch (PlayerManager.Instance.currentType)
            {
                case PlayerManager.PunkinType.bigPunkin:
                    PlayerManager.Instance.SwapEffigy(4, empty);
                    break;
                case PlayerManager.PunkinType.dollyPunkin:
                    PlayerManager.Instance.SwapEffigy(5, empty);
                    break;
                default:
                    break;
            }
        }
    }
}
