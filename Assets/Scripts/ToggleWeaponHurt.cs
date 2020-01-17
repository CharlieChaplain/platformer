using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWeaponHurt : MonoBehaviour
{
    private BoxCollider hurtbox;
    private TrailRenderer trail;

    void toggleWeaponHurt(int flag) //triggered by animation events during swinging animations
    {
        //grabs the hitbox and trail that are manually assigned to the current weapon's prefab via the inspector
        //WeaponInfo is a component of the current weapon. Current Weapon is a field in Player Manager.
        hurtbox = PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().hurtbox;
        trail = PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().trail;

        if (flag == 0)
        {
            hurtbox.enabled = false;
            trail.enabled = false;
        }
        else if (flag == 1)
        {
            hurtbox.enabled = true;
            trail.enabled = true;
        }
    }
}
