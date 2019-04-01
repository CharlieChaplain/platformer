using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMovement : MonoBehaviour {

    public float radius;
    public float halfAmplitude;
    public float vertSpeed = 1;
    public float horzSpeed = 1;

    public float moveSpeed = 5.0f;

    public Transform target;

    private Transform fairy;
    ParticleSystem[] particles;
    TrailRenderer[] trails;

    GameObject weaponLastFrame;

    // Use this for initialization
    void Start () {
        fairy = transform.GetChild(0);
        particles = GetComponentsInChildren<ParticleSystem>();
        trails = GetComponentsInChildren<TrailRenderer>();
        weaponLastFrame = PlayerManager.Instance.currentWep;

        //initializes fairy to not show
        foreach (ParticleSystem part in particles)
            part.Stop();
        foreach (TrailRenderer trail in trails)
            trail.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        target = PlayerManager.Instance.currentTarget.transform;

        CheckVisibility();

        float y = Mathf.Sin(Time.time * vertSpeed) * halfAmplitude;
        float x = Mathf.Sin(Time.time * horzSpeed) * radius;
        float z = Mathf.Cos(Time.time * horzSpeed) * radius;
        fairy.localPosition = new Vector3(x, y, z);

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);

        weaponLastFrame = PlayerManager.Instance.currentWep;
    }

    //checks if current weapon is a ranged weapon. If so, show the fairy
    void CheckVisibility()
    {
        if (PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().ranged && !weaponLastFrame.GetComponent<WeaponInfo>().ranged)
        {
            foreach(ParticleSystem part in particles)
                part.Play();
            foreach (TrailRenderer trail in trails)
                trail.enabled = true;
        } else if (!PlayerManager.Instance.currentWep.GetComponent<WeaponInfo>().ranged && weaponLastFrame.GetComponent<WeaponInfo>().ranged)
        {
            foreach (ParticleSystem part in particles)
                part.Stop();
            foreach (TrailRenderer trail in trails)
                trail.enabled = false;
        }


    }
}
