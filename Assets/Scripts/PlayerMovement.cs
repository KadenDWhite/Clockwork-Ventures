using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public List<AnimationClip> movementAnimations; // Idle and Run animations
    public List<AnimationClip> attackAnimations;   // Attack, Draw, Sheathe animations
    public float speed;
    public float runSpeed;
    public float jump;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    private float currentSpeed;
    private bool isRunning;
    private bool isAttacking;
    private bool isWeaponDrawn;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize SpriteRenderer
    }

    void Update()
    {
        HandleMovement();
        HandleWeaponState();
        HandleAttack();
        FlipSprite(); // Call to flip the sprite based on movement direction
    }

    void HandleMovement()
    {
        float move = Input.GetAxis("Horizontal");

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

        // Switch animations based on state
        if (isAttacking)
        {
            PlayAnimation("Attack");
        }
        else if (isRunning)
        {
            PlayAnimation("Run");
        }
        else
        {
            PlayAnimation("Idle");
        }
    }

    void HandleWeaponState()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isWeaponDrawn = !isWeaponDrawn;
            if (isWeaponDrawn)
            {
                PlayAnimation("Draw");
            }
            else
            {
                PlayAnimation("Sheathe");
            }
        }
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0) && isWeaponDrawn)
        {
            isAttacking = true;
            PlayAnimation("Attack");
        }
        else
        {
            isAttacking = false;
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }

        // Check and play animation based on the name
        foreach (var clip in movementAnimations)
        {
            if (clip.name == animationName)
            {
                animator.Play(animationName);
                return;
            }
        }

        foreach (var clip in attackAnimations)
        {
            if (clip.name == animationName)
            {
                animator.Play(animationName);
                return;
            }
        }

        Debug.LogWarning("Animation not found: " + animationName);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, groundLayer);
    }

    private void FlipSprite()
    {
        float move = Input.GetAxis("Horizontal");

        if (move > 0 && !isFacingRight || move < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f; // Flips the sprite by inverting the x scale
            transform.localScale = ls;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * currentSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}