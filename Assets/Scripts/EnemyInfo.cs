using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour {

	public float currentHealth;
	public float maxHealth;

	public enum EnemyState
	{
		Move,
		Hit,
		Attacking,
		Invincible
	};

	public EnemyState enemyState;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		enemyState = EnemyState.Move;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		checkDeath ();
	}

	void checkDeath(){
		if (currentHealth <= 0) {
			StartCoroutine (Die ());
		}
	}

	IEnumerator Die(){
		this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer> ()[0].enabled = false;
		this.gameObject.GetComponentsInChildren<MeshRenderer> ()[0].enabled = false;
		this.gameObject.GetComponent<CapsuleCollider> ().enabled = false;
		yield return new WaitForSeconds (1f);
		GameObject.Destroy (this.gameObject);
	}
}
