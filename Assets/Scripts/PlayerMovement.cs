using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator animator;
    private WeaponManager weaponManager;
    private SpriteRenderer spriteRenderer;

	private float move;
	private bool isFacingRight = true; // Assuming the character starts facing right

	public float speed; // Regular speed movement
	public float jump;
	public float runSpeed; // Speed when holding Shift

	public Vector2 boxSize;
	public float castDistance;
	public LayerMask groundLayer;

	private float currentSpeed;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>(); // Initializes SpriteRenderer
        weaponManager = GetComponent<WeaponManager>(); // Reference to WeaponManager
    }

	void Update()
	{
		// Gets horizontal input
		move = Input.GetAxis("Horizontal");

		// Determines current speed
		if (Input.GetKey(KeyCode.LeftShift))
		{
			currentSpeed = runSpeed;
		}
		else
		{
			currentSpeed = speed;
		}

        // Regular movement animations (idle, run)
        if (!weaponManager.isDrawn)
        {
            animator.SetFloat("xVelocity", Mathf.Abs(move) * currentSpeed);
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
        // Handles jumping
        if (Input.GetButtonDown("Jump") && IsGrounded())
		{
			rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
			animator.SetBool("isJumping", true);
		}
		else
		{
			// Updates jumping status
			animator.SetBool("isJumping", !IsGrounded());
		}

        // Flipping the sprite based on movement direction
        FlipSprite();

		// Applying movement
		rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);
    }

	private bool IsGrounded()
	{
		// Checking if the player is grounded
		return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, groundLayer);
	}

	private void FlipSprite()
	{
		if (move > 0 && !isFacingRight || move < 0 && isFacingRight)
		{
			Flip(); // Called as a helper method to flip the sprite
		}
	}

	private void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 ls = transform.localScale;
		ls.x *= -1f; // Flips the sprite by inverting the x scale
		transform.localScale = ls;
	}

	private void FixedUpdate()
	{
		// Applies the movement with the current speed
		rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);
		animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
		animator.SetFloat("yVelocity", rb.velocity.y);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
	}
}