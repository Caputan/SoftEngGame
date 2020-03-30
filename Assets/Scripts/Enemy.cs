using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Transform player; // TODO: use GetComponent or smth
	private Rigidbody2D _enemy;
	public float patrolLeftBorderX;
	public float patrolRightBorderX;
	
	public Animator animator;

	public int maxHealth = 0;
	private int currentHealth = 100;

	public float waitTime;
	public float movementSpeed;
	private bool _isWalking;
	private bool _facesRight;
	private float _currentWaitTime;
	
	public float detectDistance;
	public float allowedWalkAwayDistance;
	private bool _isHunting;
	
	// Start is called before the first frame update
    void Start()
    {
		maxHealth = currentHealth;
		_facesRight = true;
		_enemy = GetComponent<Rigidbody2D>();
		_currentWaitTime = waitTime;
		_isWalking = true;
		_isHunting = false;
    }

    private void Update()
    {
	    Patrol();
	    HuntPlayer();
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
		
		if (distance <= detectDistance)
		{
			_isHunting = true;

			if (playerOnTheLeft)
			{
				if (_facesRight)
				{
					Flip();
				}
				_enemy.velocity = new Vector2(-movementSpeed * 2, 0);
			}
			else
			{
				if (!_facesRight)
				{
					Flip();
				}
				_enemy.velocity = new Vector2(movementSpeed * 2, 0);
			}
		}
	}
}
