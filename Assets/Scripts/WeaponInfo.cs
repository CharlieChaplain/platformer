using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour {

	public float damage;
	public float swingTime;
	public bool twoHand;
    public bool ranged;
	public int attackState;
    public float projectileSpeed; //only used if weapon has projectiles
    public float range; //only used if weapon has projectiles
    public float timeWhenShoot; //only used if weapon has projectiles

    public BoxCollider hurtbox; //the hurtbox of the weapon. Used by animation events to turn it on/off
    public TrailRenderer trail; //the trail the weapon leaves when swung. Used by animation events to turn it on/off

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
