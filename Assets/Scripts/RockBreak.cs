using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBreak : ProjectileBreak {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    IEnumerator Break()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        GetComponentInChildren<ParticleSystem>().Emit(600);

        

        yield return new WaitForSeconds(1);

        GetComponent<ProjectileInfo>().origin.GetComponent<Tosser>().allProjectiles.Remove(gameObject); //this removes the projectile from the origin's list so it doesn't reference dead projectiles

        GameObject.Destroy(gameObject);
    }

    public override void StartBreak()
    {
        StartCoroutine(Break());
    }
}
