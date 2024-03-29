﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Animator animator;

	public int maxHealth = 0;
	int currentHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
		maxHealth = currentHealth;  
    }

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		animator.SetTrigger("Hurt");

		if(currentHealth <= 0)
			Die();
	}

	void Die()
	{
		animator.SetBool("Dead", true);

		GetComponent<Collider2D>().enabled = false;
		this.enabled = false;
	}
}
