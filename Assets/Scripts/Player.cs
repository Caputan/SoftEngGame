using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> 
/// Скрипт для управления игровым персонажем 
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// Ссылка на координаты камеры
    /// </summary>
    public Vector3 cameraOffset;
    private Transform _cameraCoords;

    /// <summary>
    /// Скорость игрока.
    /// </summary>
    public float movementSpeed;
    private Vector3 _movement;

    /// <summary>
    /// Ссылка на анимации.
    /// </summary>
    public Animator animator;

    private bool _facesRight;

    private Rigidbody2D _player;

    /// <summary>
    /// Сила прыжка
    /// </summary>
    public float jumpForce;

    /// <summary>
    /// Нахождение игрока на поверхности. Значение меняется скриптом GroundCheck
    /// </summary>
    public bool isGrounded;
    
    public Transform attackPosition;
    /// <summary>
    /// Радиус атаки.
    /// </summary>
    public float attackRange;
    /// <summary>
    /// Задержка при атаке.
    /// </summary>
    public float attackRate;
    /// <summary>
    /// Ссылка на противников в радиусе атаки
    /// </summary>
    public LayerMask enemyLayers;
    /// <summary>
    /// Урон, наносимый игроком
    /// </summary>
    public int playerDamage;
    private float _nextTimeAttack;

    /// <summary>
    /// Задержка перед следующим использованием невидимости
    /// </summary>
    public float nextInvisibilityTime;
    /// <summary>
    /// Максимальное время нахождения в режиме невидимости 
    /// </summary>
    public float invisibilityTime;
    /// <summary>
    /// Время, оставшееся до окончания невидимости
    /// </summary>
    public float invisibilityTimeLeft;
    /// <summary>
    /// ПНахождение игрока в режиме невидимости.
    /// </summary>
    public bool isInvisible;

	/// <summary>
	/// Задержка перед следующим использованием неуязвимости 
	/// </summary>
	public float _nextInvinсibilityTime;
    /// <summary>
    /// Максимальное время нахождения в режиме неуязвимости
    /// </summary>
	public float invinсibilityTime;
    /// <summary>
    /// Время, оставшееся до окончания неуязвимости
    /// </summary>
	public float invincibilityTimeLeft;
    private bool _isInvincible;

    /// <summary>
    /// Значение возможности забраться на лестницу
    /// </summary>
    public bool canClimb;

    /// <summary>
    /// Текущее здоровье игрока
    /// </summary>
	public int currentHealth;
    private int maxHeatlh;
    private HealthBar _healthBar;

    public int activeSaveIndex;
    public string nickname;

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

        canClimb = false;

        maxHeatlh = 100;
		currentHealth = maxHeatlh;
        _healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();

        _healthBar.SetMaxHealth(maxHeatlh);

        isInvisible = false;
        _isInvincible = false;
		nextInvisibilityTime = 0f;
		_nextInvinсibilityTime = 0f;
        invinсibilityTime = 1f;
        invisibilityTime = 5f;

        activeSaveIndex = Slot.activeIndex;

        nickname = Slot.nickname;

        LoadProgress();
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
            isInvisible = true;
        }
        else if (invisibilityTimeLeft <= 0f)
        {
            invisibilityTimeLeft = 0f;
            GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            isInvisible = false;
        }
		if(invincibilityTimeLeft <= 0f)
		{
			invincibilityTimeLeft = 0f;
			_isInvincible = false;
		}

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= nextInvisibilityTime)
            {
                invisibilityTimeLeft = invisibilityTime;
				invincibilityTimeLeft = invinсibilityTime;
                Invisibility();
                nextInvisibilityTime = Time.time + invisibilityTime * 3;
            }
        }

    }

    /// <summary> 
    /// Метод атаки противников игроком
    /// </summary>
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


    /// <summary> 
    /// Обработка нажатий кнопок с клавиатуры для управления движением персонажа
    /// </summary>
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

        if (canClimb)
        {
            _player.gravityScale = 0f;
            Climb();
            //isGrounded = false;
        } else
        {
            _player.gravityScale = 10f;
            //isGrounded = true;
        }
    }


    /// <summary> 
    /// Подъем/спуск по лестнице
    /// </summary>
    void Climb()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        _player.velocity = new Vector2(0f, vertical * movementSpeed);
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
    /// Описание способа прыжока игрового персонажа 
    /// </summary>
    void Jump()
    {
        if (isGrounded == true)
        {
            animator.SetTrigger("IsJumping");
            _player.velocity += Vector2.up * jumpForce;
  
        }
    }
    
    /// <summary> 
    /// Переключение режима "невидимости" персонажа 
    /// </summary>
    void Invisibility()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
    }

    /// <summary> 
    /// Получение урона игровым персонажем
    /// </summary>
    /// <param name="damage">
    /// Значение получаемого урона
    /// </param>
	public void TakeDamage(int damage)
	{
		if (!_isInvincible)
		{
			currentHealth -= damage;

            _healthBar.SetHealth(currentHealth);

			invisibilityTimeLeft = 0f;

			animator.SetTrigger("Hurt");

			if (currentHealth <= 0)
			{
				Die();
			}
		}
	}


    /// <summary> 
    /// Смерть игрового персонажа 
    /// </summary>
    void Die()
	{
        animator.SetBool("isDead", true);

        Dialogue deadDialogue = new Dialogue();
        deadDialogue.name = "Oh no!";
        deadDialogue.sentences = new string[1];
        deadDialogue.sentences[0] = "You are dead!";

        FindObjectOfType<DialogueManager>().StartDialogue(deadDialogue);


        GetComponent<Collider2D>().enabled = false;
		enabled = false;

        //SceneManager.LoadScene(0);

        SaveSystem.ClearData();
    }

    /// <summary> 
    /// Сохранение прогресса персонажа
    /// </summary>
    public void SaveProgress()
	{
		SaveSystem.SavePlayer(this);
	}

    /// <summary> 
    /// Загрузка прогресса персонажа 
    /// </summary>
    public void LoadProgress()
	{
		DataToSave data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            currentHealth = data.playerHealth;
            _healthBar.SetHealth(currentHealth);
        }
    }
}