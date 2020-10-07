using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//General enemy animation event reciever
public class E_AnimEventR : MonoBehaviour
{
    public BoxCollider hurtbox;
    public TrailRenderer trail;

    void ToggleWeaponHurt(int flag)
    {
        if (flag == 0)
        {
            hurtbox.enabled = false;
            if(trail != null)
                trail.enabled = false;
        }
        else if (flag == 1)
        {
            hurtbox.enabled = true;
            if (trail != null)
                trail.enabled = true;
        }
    }

    void ChangeState(string state)
    {
        transform.parent.GetComponent<Enemy>().ChangeState(state);
    }

    void Shoot()
    {
        transform.parent.GetComponent<Enemy>().Shoot();
    }

    void PlaySound(int soundIndex) //the index in the enemy's "allSounds" list of the correct sound effect.
    {
        transform.parent.GetComponent<Enemy>().allSounds[soundIndex].PlaySound();
    }

    void StopSound(int soundIndex) //the index in the enemy's "allSounds" list of the correct sound effect.
    {
        transform.parent.GetComponent<Enemy>().allSounds[soundIndex].StopSound();
    }
}
