using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour {

    private float poisonTimer = 0;
    private float poisonPainTimer = 0; //how often poison will hurt the player

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerManager.Instance.currentStatus == PlayerManager.StatusEffect.poisoned)
        {
            poisonTimer -= Time.deltaTime;
            if (poisonTimer <= 0)
            {
                //Player is no longer poisoned
                PlayerManager.Instance.currentStatus = PlayerManager.StatusEffect.none;
                PlayerManager.Instance.playerTex.SetColor("_Tint", new Color(1f, 1f, 1f, 1f));
                PlayerManager.Instance.playerTexTrans.SetColor("_Tint", new Color(1f, 1f, 1f, 1f));
                PlayerManager.Instance.poisonBubbles.Stop();
            }
            Poisoned();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "HurtBox")
        {
            if(other.gameObject.layer == 10) //if hurtbox is on enemy layer
            {
                Enemy enemy = other.gameObject.transform.root.GetComponentInChildren<Enemy>();
                float damage = enemy.currentAttack.damage;
                HurtPlayer(damage);

                //will poison the player for a while if the attack poisons
                if(enemy.currentAttack.status == PlayerManager.StatusEffect.poisoned)
                {
                    if(Random.Range(0, 1f) <= enemy.currentAttack.statusChance)
                    {
                        //Player becomes poisoned
                        PlayerManager.Instance.currentStatus = PlayerManager.StatusEffect.poisoned;
                        poisonTimer = 10.0f;
                        poisonPainTimer = 0;

                        PlayerManager.Instance.playerTex.SetColor("_Tint", new Color(0, 1f, 0.75f, 1f));
                        PlayerManager.Instance.playerTexTrans.SetColor("_Tint", new Color(0, 1f, 0.75f, 1f));
                        PlayerManager.Instance.poisonBubbles.Play();
                    }
                }
            }
            else if (other.gameObject.layer == 11) //if hurtbox is on enemyProjectile layer
            {
                ProjectileInfo projectile = other.GetComponentInParent<ProjectileInfo>();
                HurtPlayer(projectile.damage);
            }
        }
    }

    public void HurtPlayer(float amount)
    {
        PlayerManager.Instance.currentHealth -= amount;
    }

    private void Poisoned()
    {
        if(poisonPainTimer <= 0)
        {
            HurtPlayer(1.0f);
            poisonPainTimer = 0.1f;
        }

        poisonPainTimer -= Time.deltaTime;
    }
}
