using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletBreak : ProjectileBreak {

	// Use this for initialization
	void Start () {
        thrown = true;
        lifeLimit = 3;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<ProjectileInfo>().active)
            LifeTimer();
	}

    IEnumerator Break()
    {
        GetComponent<ProjectileInfo>().ToggleActive();
        yield return new WaitForSeconds(1);
        transform.position = GameManager.Instance.gameObject.transform.position;
    }

    public override void StartBreak()
    {
        StartCoroutine(Break());
    }
}
