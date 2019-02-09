using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour {

	public Animator anim;
	public EnemyInfo enemyInfo;

	public ParticleSystem hitParticles;

	// Use this for initialization
	void Start () {
		enemyInfo = GetComponent<EnemyInfo> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "HurtBox" && PlayerManager.Instance.attacking) {
			//Vector3 dir = new Vector3 (-transform.forward.x, 0.0f, -transform.forward.z);
			Vector3 dir = transform.position - other.transform.parent.parent.position;
			dir.Normalize ();
			SendMessage ("BounceBack", dir);
			anim.SetInteger ("State", 1);
			enemyInfo.enemyState = EnemyInfo.EnemyState.Hit;

			getHurt (other.transform.parent.gameObject.GetComponent<WeaponInfo>().damage);

			if(hitParticles != null){
				hitParticles.Emit (15);
			}

			if (GetComponent<Chase> () != null) {
				GetComponent<Chase> ().doChase = true;
			}
		}
	}

	void resetState () {
		anim.SetInteger ("State", 0);
		enemyInfo.enemyState = EnemyInfo.EnemyState.Move;
	}

	void getHurt(float damage){
		enemyInfo.currentHealth -= damage;
	}
}
