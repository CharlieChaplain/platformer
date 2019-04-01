using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBreak : MonoBehaviour {

    protected bool thrown;

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
}
