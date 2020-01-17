using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaxMonsters : MonoBehaviour
{
    //this will set the max number of monsters to spawn when a specific trigger is entered.

    public FlatwoodsMonManager MonsterManager;
    public int maxNumMonsters;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            MonsterManager.maxNumMonsters = maxNumMonsters;
        }
    }
}
