using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBreak : MonoBehaviour {

    protected bool thrown;
    protected float timer = 0;
    protected float lifeLimit; //how long the projectile can survive before being reset

    public bool getThrown() { return thrown; }
    public void setThrown(bool p_thrown) {
        thrown = p_thrown;
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    public virtual void StartBreak()
    {

    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (thrown)
            StartBreak();
    }

    protected void LifeTimer()
    {
        timer += Time.deltaTime;
        if(timer >= lifeLimit)
        {
            StartBreak();
            timer = 0;
        }
    }
}
