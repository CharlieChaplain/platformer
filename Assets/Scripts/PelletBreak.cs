using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletBreak : ProjectileBreak {

	// Use this for initialization
	void Start () {
        thrown = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Break()
    {
        
        GetComponentInChildren<MeshRenderer>().enabled = false;
        Collider[] colliders = GetComponents<Collider>();
        foreach(Collider col in colliders)
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(3);
        GameObject.Destroy(gameObject);
    }

    public override void StartBreak()
    {
        StartCoroutine(Break());
    }
}
