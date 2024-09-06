using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    Animator animator;

    private float Move;
    bool isFacingRight = false;

    public float speed; // Regular speed movement
	public float jump;
    public float runSpeed; // Speed when holding Shift

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<animator>();
    }

    // Update is called once per frame
    void Update()
    {
		{
			float move = Input.GetAxisRaw("Horizontal");

            // Default to normal speed
            float currentSpeed = speed;

            FlipSprite();

            // Check if the Shift key is held down
            if (Input.GetKey(KeyCode.LeftShift))
			{
				currentSpeed = runSpeed;
            }


            if (Input.GetButtonDown("Jump") && isGrounded())
            {
                rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
                animator.SetBool("isJumping", !isGrounded);
            }
            else
            {
				animator.SetBool("isJumping", isGrounded);
			}
        }
    }

    private void FixedUpdate()
    {
		// Apply the movement with the current speed
		rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
		animator.SetFloat("yVelocity", rb.velocity.y);
	}

    void FlipSprite()
    {
        if (isFacingRight && Move < 0f || !isFacingRight && Move > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    public bool isGrounded()
    {
        if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
		}
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
	}
}
