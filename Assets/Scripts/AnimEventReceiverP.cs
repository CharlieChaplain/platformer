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
        //Will determine if base punkin (and therefore all weapons) or a different effigy should be accessed.
        switch (PlayerManager.Instance.currentType)
        {
            case PlayerManager.PunkinType.basePunkin:
                hurtbox = PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().hurtbox;
                trail = PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().trail;
                break;
            case PlayerManager.PunkinType.bigPunkin:
                hurtbox = PlayerManager.Instance.bigPunkWep.GetComponent<WeaponInfo>().hurtbox;
                trail = PlayerManager.Instance.bigPunkWep.GetComponent<WeaponInfo>().trail;
                break;
            default:
                break;
        }

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

    void PlayFootstepSound()
    {
        SoundManager.Instance.PlayPlayerFootstep();
    }

    void PlayWeaponSound(int index)
    {
        float pitch = 1.0f;
        //randomizes the pitch of the whoosh
        if (index == 0)
            pitch = Random.Range(0.5f, 1.25f);


        SoundManager.Instance.PlayWeaponSound(index, pitch);
    }

    void SwapEffigy(int index) //this is only used when the head attaches to an effigy so it can delete the headless effigy object.
    {
        GameObject empty = new GameObject();
        PlayerManager.Instance.SwapEffigy(index, empty);
    }

    void ToggleMove(int flag)
    {
        if (flag == 0)
            MovementManager.Instance.canMove = false;
        else if (flag == 1)
            MovementManager.Instance.canMove = true;
    }
}
