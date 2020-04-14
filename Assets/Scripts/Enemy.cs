﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Transform player;
	private Rigidbody2D _enemy;
	public float patrolLeftBorderX;
	public float patrolRightBorderX;
	
	public Animator animator;

	public int maxHealth;
	private int currentHealth;

	public float waitTime;
	public float movementSpeed;
	private bool _isWalking;
	private bool _facesRight;
	private float _currentWaitTime;
	
	public float detectDistance;
	public float allowedWalkAwayDistance;
	private bool _isHunting;

	public int enemyDamage;
	public float attackDelay;
	private bool _playerInAttackRange;
	private float _currentAttackDelay;

	// Start is called before the first frame update
    void Start()
    {
	    // Default values
	    patrolLeftBorderX = 2.0f;
	    patrolRightBorderX = 2.0f;
	    maxHealth = 100;
	    waitTime = 2;
	    movementSpeed = 3.0f;
	    detectDistance = 4.0f;
	    allowedWalkAwayDistance = 3.0f;
	    enemyDamage = 20;
	    attackDelay = 2.0f;
	    
	    patrolLeftBorderX = transform.position.x - patrolLeftBorderX;
	    patrolRightBorderX += transform.position.x;
	    currentHealth = maxHealth;
		player = GameObject.Find("Player").GetComponent<Transform>();
		_facesRight = true;
		_enemy = GetComponent<Rigidbody2D>();
		_currentWaitTime = waitTime;
		_isWalking = true;
		_isHunting = false;
		_playerInAttackRange = false;
		_currentAttackDelay = 0;
		movementSpeed = 5f;
    }

    private void Update()
    {
	    Debug.Log(patrolLeftBorderX);
	    Patrol();
	    HuntPlayer();
		animator.SetFloat("Speed", _enemy.velocity.magnitude);
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
		enabled = false;
	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_facesRight = !_facesRight;

		transform.Rotate(0f, 180f, 0f);
	}

	private void Attack()
	{
		// animator.SetTrigger("Attack");

		player.GetComponent<Player>().TakeDamage(enemyDamage);
	}
	
	private void Patrol()
	{
		if (_isHunting)
		{
			return;
		}
		
		if (_isWalking)
		{
			if (_facesRight)
			{
				if (transform.position.x < patrolRightBorderX) // Goes right
				{
					_enemy.velocity = new Vector2(movementSpeed, 0);
				}
				else
				{
					_isWalking = false;
				}
			}
			else
			{
				if (transform.position.x > patrolLeftBorderX) // Goes left
				{
					_enemy.velocity = new Vector2(-movementSpeed, 0);
				}
				else
				{
					_isWalking = false;
				}
			}
		}
		else
		{
			_currentWaitTime -= Time.deltaTime;

			if (_currentWaitTime <= 0)
			{
				_currentWaitTime = waitTime;
				Flip();
				_isWalking = true;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == "Player")
		{
			_currentAttackDelay = 0;
			_playerInAttackRange = true;
		}
	}
	
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.tag == "Player")
		{
			_playerInAttackRange = false;
		}
	}
	
	private void HuntPlayer()
	{
		var enemyPos = _enemy.position.x;
		var playerPos = player.position.x;
		var allowedLeftBorder = patrolLeftBorderX - allowedWalkAwayDistance;
		var allowedRightBorder = patrolRightBorderX + allowedWalkAwayDistance;

		if (playerPos < allowedLeftBorder || playerPos > allowedRightBorder)
		{
			_isHunting = false;
			return;
		}

		var playerOnTheLeft = enemyPos - playerPos > 0;
		var distance = Math.Sqrt(Math.Pow(enemyPos - playerPos, 2));

		if (_playerInAttackRange)
		{
			_currentAttackDelay -= Time.deltaTime;

			if (_currentAttackDelay <= 0)
			{
				_currentAttackDelay = attackDelay;
				Attack();
			}
			return;
		}
		
		if (distance <= detectDistance)
		{
			_isHunting = true;

			if (playerOnTheLeft)
			{
				if (_facesRight)
				{
					Flip();
				}
				_enemy.velocity = new Vector2(-movementSpeed * 1.2f, 0);
			}
			else
			{
				if (!_facesRight)
				{
					Flip();
				}
				_enemy.velocity = new Vector2(movementSpeed * 1.2f, 0);
			}
		}
	}
}
