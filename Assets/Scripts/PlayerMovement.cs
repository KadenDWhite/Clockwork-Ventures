using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public float speed; // Regular speed movement
    public float jump;
    public float runSpeed; // Speed when holding Shift

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    private float currentSpeed;
    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");

        // Run logic
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            isRunning = true;
        }
        else
        {
            currentSpeed = speed;
            isRunning = false;
        }

        rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);

        // Animator parameters
        animator.SetFloat("xVelocity", Mathf.Abs(move) * currentSpeed);
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", !IsGrounded());
        }

        // Handle flipping
        FlipSprite(move);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, groundLayer);
    }

    private void FlipSprite(float move)
    {
        bool isFacingRight = transform.localScale.x > 0;
        if (move > 0 && !isFacingRight || move < 0 && isFacingRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}