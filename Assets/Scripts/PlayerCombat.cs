using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

	public Animator animator;

	float nextInvisTime = 0f;
	public float invisTime = 5f;
	public float invisTimeLeft = 0f;

	public Transform attackPos;
	public float attackRange = 0.5f;
	public float attackRate = 2f;
	float nextAttackTime = 0f;
	public LayerMask enemyLayers;

	public int playerDamage = 20;


    void Update()
    {
		if (Time.time >= nextAttackTime)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Attack();
				nextAttackTime = Time.time + 1f / attackRate;
			}
		}
		
		if(invisTimeLeft > 0f)
		{
			invisTimeLeft -= Time.deltaTime;
		} else if (invisTimeLeft <= 0f)
		{
			invisTimeLeft = 0f;
			GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{ 
			if (Time.time >= nextInvisTime)
			{
				invisTimeLeft = invisTime;
				Invisibility();
				nextInvisTime = Time.time + invisTime * 3;
			}
		}
	}

	void Attack()
	{
		animator.SetTrigger("Attack");

		Collider2D[] enemiesHit =  Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemyLayers);

		foreach(Collider2D enemy in enemiesHit)
		{
			enemy.GetComponent<Enemy>().TakeDamage(playerDamage);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (attackPos == null)
			return;

		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}

	void Invisibility()
	{
		GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
	}
}
