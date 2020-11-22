using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAnchor : MonoBehaviour
{
    public GameObject headLocation;
    public int effigyType; //used in ReattachHead and subsequently in the SwapEffigy() method in PlayerManager

    public bool canAcceptHead;

    public ParticleSystem part1;
    public ParticleSystem part2;

    float timer;
    bool ignition;

    public void Start()
    {
        canAcceptHead = false;
        ignition = false;
        timer = 3f;
    }

    public void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(!ignition)
        {
            ignition = true;
            canAcceptHead = true;
            part1.Play();
            part2.Play();
        }
    }
}
