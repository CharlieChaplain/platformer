using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAttack
{
    public string animTrigger; //the exact name of the animation trigger. Should be descriptive of the attack as well
    public float damage; //the amount of damage the attack does
    public PlayerManager.StatusEffect status; //the status effect that the attack imparts
    public float statusChance; //the chance the specific status effect is applied

}
