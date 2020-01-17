using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatwoodsMonManager : MonoBehaviour
{
    public GameObject[] allMonsters;
    public List<GameObject> activeMonsters;

    private float timer = 0;
    private bool spawned; //determines if baddie was just spawned. Will wait a little bit to spawn another

    public int maxNumMonsters;

    public Transform spawnArea; //the position where they can spawn.

    // Start is called before the first frame update
    void Start()
    {
        maxNumMonsters = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeMonsters.Count < maxNumMonsters && !spawned)
        {
            CheckMonster(); //will start process to spawn monster if not enough are present
        }
        if(activeMonsters.Count > 0)
        {
            CheckActives(); //routinely checks if any monsters are inactive and should be removed from the active list.
            CheckMaxMonsters(); //routinely checks if any monsters are causing the active count to be over maxnum
        }

        if (spawned) //if monster just spawned, then count up to wait a bit between spawning
        {
            timer += Time.deltaTime;
            if(timer >= 0.5f)
            {
                timer = 0;
                spawned = false;
            }
        }
    }

    void CheckMonster() //checks for first available inactive monster and "spawns" it.
    {
        for (int i = 0; i < allMonsters.Length; i++)
        {
            if (!allMonsters[i].GetComponent<FlatwoodsMonster>().isActive)
            {
                SpawnMonster(allMonsters[i]);
                    
                break;
            }
        }
    }

    void SpawnMonster(GameObject monster) //"spawns" given monster, aka activates and teleports it
    {
        activeMonsters.Add(monster);

        //will find a random position around the spawn area to put the monster
        float difference = Random.Range(-5.0f, 5.0f);
        Vector3 pos = spawnArea.position + (spawnArea.right * difference);

        //casts a ray up, then down a bit, to see if it hits a "ground". if not, it will just spawn at the player's height
        RaycastHit hit;
        int layerMask = 1 << 8;
        if(Physics.Raycast(pos, Vector3.down, out hit, 40f, layerMask, QueryTriggerInteraction.Collide))
        {
            pos = hit.point;
        } else if (Physics.Raycast(pos, Vector3.up, out hit, 40f, layerMask, QueryTriggerInteraction.Collide))
        {
            pos = hit.point;
        }
        //if no ground is found, just spawn in front of player.
        monster.transform.position = pos;

        monster.GetComponent<FlatwoodsMonster>().isActive = true;

        spawned = true;
    }

    void CheckActives() //checks all monsters in active list, if they're inactive remove them
    {
        GameObject toRemove = null;
        foreach(GameObject monster in activeMonsters)
        {
            if(monster.GetComponent<FlatwoodsMonster>().isActive == false) //checks if any monsters in the "active" list are inactive
            {
                toRemove = monster;
            }
        }

        if (toRemove != null)
        {
            //will remove the last inactive monster in the list. Will remove all over a couple iterations
            RemoveMonster(toRemove);
        }
    }

    void CheckMaxMonsters() //checks for max number of monsters. If there's too many active monsters, removes them til there's not
    {
        if(activeMonsters.Count > maxNumMonsters)
        {
            RemoveMonster(activeMonsters[activeMonsters.Count - 1]);
        }
    }

    void RemoveMonster(GameObject monster) //removes a monster from the active list
    {
        //monster.transform.position = transform.position;
        StartCoroutine("Sink", monster);
    }

    IEnumerator Sink(GameObject monster)
    {
        for(float f = 0; f < 0.5f; f += Time.deltaTime)
        {
            monster.transform.position += Vector3.down * 30f * Time.deltaTime;
            yield return null;
        }
        monster.GetComponent<FlatwoodsMonster>().isActive = false;
        activeMonsters.Remove(monster);
        monster.transform.position = transform.position;
    }
}
