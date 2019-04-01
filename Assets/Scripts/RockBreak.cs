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

        GetComponentInChildren<ParticleSystem>().Emit(6);

        yield return new WaitForSeconds(3);
        GameObject.Destroy(gameObject);
    }

    public override void StartBreak()
    {
        StartCoroutine(Break());
    }
}
