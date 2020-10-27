using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add this script to the actual physical item in question.
public class ItemAnchor : MonoBehaviour
{
    public ItemTemplate item;

    public GameObject hideThis; //which hierarchy to hide when collected so the game object can be destroyed a couple seconds later

    //called by InventoryAnchor when item is picked up
    public void Collected()
    {
        ParticleSystem particles = transform.Find("particle_grabbed").GetComponent<ParticleSystem>();
        AudioSource sound = transform.Find("audio_source").GetComponent<AudioSource>();

        particles.Play();
        sound.Play();
        GetComponent<Collider>().enabled = false;
        hideThis.SetActive(false);
        StartCoroutine("disappear");
    }

    IEnumerator disappear()
    {
        Debug.Log("hello");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
