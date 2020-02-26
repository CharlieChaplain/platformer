using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBreak : MonoBehaviour {

    private ProjectileInfo info;
    private float timer = 0;
    public float lifeLimit; //how long the projectile can survive before being reset, in seconds

    void Start () {
        info = GetComponent<ProjectileInfo>();
    }
	
	void Update () {
        if (info.active)
        {
            timer += Time.deltaTime;
        }

        if (timer >= lifeLimit)
        {
            timer = 0;
            StartBreak();
        }
    }

    public virtual void StartBreak()
    {
        StartCoroutine("Break");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (info.active)
            StartBreak();
    }

    IEnumerator Break()
    {
        info.ToggleActive(false);
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(0, -20f, 0);
        info.homeQueue.Enqueue(gameObject);
    }
}
