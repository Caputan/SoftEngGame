using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public Transform cameraCoords;
	public Vector3 cameraOffset;
	
	float movementSpeed = 5f;
	Vector3 movement;

	public Animator animator;

	private bool m_FacingRight = false;

	private Rigidbody2D player;

	public float jumpForce = 25f;
	public bool isGrounded;

	// Start is called before the first frame update
	void Start()
	{
		player = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		cameraCoords.position = transform.position + cameraOffset;
		
		movement.x = Input.GetAxisRaw("Horizontal");

		animator.SetFloat("Movement", movement.x);
		animator.SetFloat("Speed", movement.magnitude);

		//animator.SetBool("isGrounded", isGrounded);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger("IsJumping");
			Jump();
		}
	}

	void FixedUpdate()
	{
		Move();
	}

	void Move()
	{
		this.transform.position += movement * Time.deltaTime * movementSpeed;
		if (movement.x > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (movement.x < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		transform.Rotate(0f, 180f, 0f);
	}

	void Jump()
	{
		if (isGrounded == true)
		{
			player.velocity += Vector2.up * jumpForce;
		}
	}
}
