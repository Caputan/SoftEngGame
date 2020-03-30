using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

	// Start is called before the first frame update
    void Start()
    {
		maxHealth = currentHealth;
		_facesRight = true;
		_enemy = GetComponent<Rigidbody2D>();
		_currentWaitTime = waitTime;
		_isWalking = true;
    }

    private void Update()
    {
	    if (_isWalking)
	    {
		    if (_facesRight)
		    {
			    if (transform.position.x < patrolRightBorderX)
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
			    if (transform.position.x > patrolLeftBorderX)
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
	
	private void Move()
	{
		
		
		// transform.position += _movement * Time.deltaTime * movementSpeed;
		//
		//
		//
		// if (_movement.x > 0 && !_facesRight)
		// {
		// 	Flip();
		// }
		// else if (_movement.x < 0 && _facesRight)
		// {
		// 	Flip();
		// }
	}
}
