using System;
using UnityEngine;

/// <summary>
/// Скрипт для управления действиями противника.
/// </summary>
public class Enemy : MonoBehaviour
{
	/// <summary>
	/// Ссылка на объект игрока, для взаимодействия с ним
	/// </summary>
	public Transform player;
	
	private Rigidbody2D _enemy;
	
	/// <summary>
	/// Граница патруля слева от изначального места противника
	/// </summary>
	public float patrolLeftBorderX;
	/// <summary>
	/// Граница патруля справа от изначального места противнка
	/// </summary>
	public float patrolRightBorderX;
	
	/// <summary>
	/// Ссылка на анимации.
	/// </summary>
	public Animator animator;

	/// <summary>
	/// Максимальное здоровье противника
	/// </summary>
	public int maxHealth;
	private int currentHealth;

	/// <summary>
	/// Время задержки при достижении границы патруля  
	/// </summary>
	public float waitTime;
	/// <summary>
	/// Скорость передвижения противника
	/// </summary>
	public float movementSpeed;
	
	private bool _isWalking;
	private bool _facesRight;
	private float _currentWaitTime;
	
	/// <summary>
	/// Радиус обнаружения игрока противником
	/// </summary>
	public float detectDistance;
	/// <summary>
	/// Допускаемое растояние для покидания границ патруля в случае обнаружения игрока
	/// </summary>
	public float allowedWalkAwayDistance;
	
	private bool _isHunting;

	/// <summary>
	/// Урон, который наносит противник
	/// </summary>
	public int enemyDamage;
	/// <summary>
	/// Время задержки между атаками.
	/// </summary>
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
		if (!player.gameObject.GetComponent<Player>().isInvisible)
		{
			HuntPlayer();
		}
		animator.SetFloat("Speed", _enemy.velocity.magnitude);
	}

    /// <summary>
    /// Метод получение урона.
    /// </summary>
    /// <param name="damage">
    /// Количество получаемого урона.
    /// </param>
    public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		animator.SetTrigger("Hurt");

		if(currentHealth <= 0)
			Die();
	}

    /// <summary>
    /// Метод смерти. Вызывается, если закончились очки здоровья.
    /// </summary>
	void Die()
	{
		animator.SetBool("isDead", true);

		GetComponent<Collider2D>().enabled = false;
		this.enabled = false;
	}
	
    /// <summary>
    /// Метод поворота модельки при смене направления движения.
    /// </summary>
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_facesRight = !_facesRight;

		transform.Rotate(0f, 180f, 0f);
	}

    /// <summary>
    /// Метод атаки, срабатывающий при нахождении рядом с игроком.
    /// </summary>
	private void Attack()
	{
		animator.SetTrigger("Attack");

		player.GetComponent<Player>().TakeDamage(enemyDamage);
	}
	
    /// <summary>
    /// Бесконечно повторяющийся метод патрулирования области на предмет наличия в ней игрока.
    /// </summary>
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
	
	/// <summary>
	/// Метод погони за игроком при обнаружении.
	/// </summary>
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
