using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour {

	public float currentHealth;
	public float maxHealth;
	public float attackTimer;
	public float deathTime;

	public Animator anim;

    public bool aggroed;

    int enumCount = 0;

	public enum EnemyState
	{
		Move = 0,
		Hit = 1,
		Attacking = 2,
		Invincible = 3,
		Dead = 4
	};

	public EnemyState enemyState;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		enemyState = EnemyState.Move;
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		checkDeath ();
	}

	void checkDeath(){
		if (currentHealth <= 0 && enemyState != EnemyState.Dead) {
			StartCoroutine (Die ());
		}
	}

	IEnumerator Die(){
		enemyState = EnemyState.Dead;

        if(gameObject.GetComponent<Tosser>() != null)
        {
            SendMessage("DestroyAllProjectiles");
        }

        if(PlayerManager.Instance.currentTarget.transform.root.gameObject == gameObject)
        {
            PlayerManager.Instance.currentTarget = GameObject.Find("Player");
        }

		anim.SetInteger ("state", (int)enemyState);
		GetComponent<CharacterController> ().enabled = false;
		if (GetComponent<LookAt> () != null)
			GetComponent<LookAt> ().canLook = false;

		yield return new WaitForSeconds (deathTime);

		float timer = 0;
		while (timer < 2) {
			timer += Time.deltaTime;
			transform.position = new Vector3(transform.position.x, transform.position.y - (0.03f * Time.deltaTime), transform.position.z);
			yield return null;
		}

		GameObject.Destroy (this.gameObject.transform.root.gameObject);
	}
}
