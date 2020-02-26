using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

	public Animator animator;

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
}
