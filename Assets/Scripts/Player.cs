using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform _cameraCoords;
    public Vector3 cameraOffset;

    public float movementSpeed;
    private Vector3 _movement;

    public Animator animator;

    private bool _facesRight;

    private Rigidbody2D _player;

    public float jumpForce;
    public bool isGrounded;

    public Transform attackPosition;
    public float attackRange;
    public float attackRate;
    public LayerMask enemyLayers;
    public int playerDamage;
    private float _nextTimeAttack;

    private float _nextInvisibilityTime;
    public float invisibilityTime;
    public float invisibilityTimeLeft;

	private bool _isInvincible = false;
	private float _nextInvinсibilityTime;
	public float invinсibilityTime;
	public float invincibilityTimeLeft;


	public int currentHealth = 0;
	private int maxHeatlh = 100;


	void Start()
    {
        movementSpeed = 5f;
        _facesRight = false;
        _player = GetComponent<Rigidbody2D>();
        jumpForce = 25f;

        attackRange = 0.5f;
        attackRate = 2f;
        _nextTimeAttack = 0f;
        playerDamage = 20;

		_cameraCoords = Camera.main.transform;

		currentHealth = maxHeatlh;

		_nextInvisibilityTime = 0f;
		_nextInvinсibilityTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
		_cameraCoords.position = new Vector3(transform.position.x + cameraOffset.x, transform.position.y + cameraOffset.y, -10);

        _movement.x = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Movement", _movement.x);
        animator.SetFloat("Speed", _movement.magnitude);

        //animator.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

		if(Input.GetKeyDown(KeyCode.E))
		{
			LoadProgress();
		}

        if (Time.time >= _nextTimeAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                _nextTimeAttack = Time.time + 1f / attackRate;
            }
        }
        
        if(invisibilityTimeLeft > 0f)
        {
            invisibilityTimeLeft -= Time.deltaTime;
			invincibilityTimeLeft -= Time.deltaTime;
			_isInvincible = true;
        }
        else if (invisibilityTimeLeft <= 0f)
        {
            invisibilityTimeLeft = 0f;
            GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
		if(invincibilityTimeLeft <= 0f)
		{
			invincibilityTimeLeft = 0f;
			_isInvincible = false;
		}

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= _nextInvisibilityTime)
            {
                invisibilityTimeLeft = invisibilityTime;
				invincibilityTimeLeft = invinсibilityTime;
                Invisibility();
                _nextInvisibilityTime = Time.time + invisibilityTime * 3;
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.GetComponent<Enemy>().TakeDamage(playerDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPosition == null)
            return;

        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        this.transform.position += _movement * Time.deltaTime * movementSpeed;
        if (_movement.x > 0 && !_facesRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (_movement.x < 0 && _facesRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facesRight = !_facesRight;

        transform.Rotate(0f, 180f, 0f);
    }

    void Jump()
    {
        if (isGrounded == true)
        {
            animator.SetTrigger("IsJumping");
            _player.velocity += Vector2.up * jumpForce;
        }
    }
    
    void Invisibility()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
    }

	void TakeDamage(int damage)
	{
		if (!_isInvincible)
		{
			currentHealth -= damage;
			invisibilityTimeLeft = 0f;

			animator.SetTrigger("Hurt");

			if (currentHealth <= 0)
			{
				Die();
			}
		}
	}

	void Die()
	{

	}

	public void SaveProgress()
	{
		SaveSystem.SavePlayer(this);
	}

	public void LoadProgress()
	{
		DataToSave data = SaveSystem.LoadPlayer();

		currentHealth = data.playerHealth;

		Vector2 position;
		position.x = data.playerPosition[0];
		position.y = data.playerPosition[1];

		transform.position = position;
	}
}