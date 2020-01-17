using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventReceiverP : MonoBehaviour
{
    private BoxCollider hurtbox;
    private TrailRenderer trail;

    //triggered by animation events during melee swinging animations
    void ToggleWeaponHurt(int flag)
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

    void EndAttack()
    {
        PlayerManager.Instance.canAttack = true;
    }

    void ShootSlingshot()
    {
        if (GameManager.Instance.InactivePellets.Count > 0) //will only fire if there are pellets left in the queue
        {
            GameObject currentWep = PlayerManager.Instance.currentWep;
            Vector3 force;
            if (PlayerManager.Instance.currentTarget != PlayerManager.Instance.player)
                force = (PlayerManager.Instance.currentTarget.transform.position - currentWep.transform.position).normalized * currentWep.GetComponent<WeaponInfo>().projectileSpeed;
            else
                force = PlayerManager.Instance.faceDir.normalized * currentWep.GetComponent<WeaponInfo>().projectileSpeed;

            currentWep.GetComponent<PlayerFire>().Launch(force);
        }
    }
}
