using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float move;
    private bool isFacingRight = true;

    public float speed; // Walking speed
    public float jump;
    public float runSpeed; // Speed when holding Shift

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    private float currentSpeed;
    private bool isJumping; // Tracks if the player is currently jumping
    private bool isStunned = false; // Tracks if the player is stunned
    private bool isDialogueFrozen = false; // New flag for dialogue freeze

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Skip movement logic if dialogue is active
        if (isDialogueFrozen) return;

        // Movement logic
        move = Input.GetAxis("Horizontal");

        // Determines current speed (running or walking)
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;

        // Set the "isRunning" parameter based on any movement input
        animator.SetBool("isRunning", Mathf.Abs(move) > 0);

        // Set velocity parameters for smooth transitions
        animator.SetFloat("xVelocity", Mathf.Abs(move) * currentSpeed);
        animator.SetFloat("yVelocity", rb.velocity.y);

        // Handle jumping
        HandleJumping();

        // Flipping the sprite based on movement direction
        FlipSprite();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
        }
    }

    public void FreezeMovement(bool freeze)
    {
        isDialogueFrozen = freeze; // Freeze or unfreeze player movement
    }

    private void HandleJumping()
    {
        // Check if the player is grounded
        bool isGrounded = IsGrounded();

        if (isGrounded && !isJumping)
        {
            animator.SetBool("isJumping", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
            isJumping = true;
            animator.SetBool("isJumping", true);
        }

        if (isJumping && rb.velocity.y <= 0)
        {
            isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, groundLayer);
    }

    private void FlipSprite()
    {
        if (move > 0 && !isFacingRight || move < 0 && isFacingRight)
        {
            Flip();
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
        if (!isDialogueFrozen && !isStunned)
        {
            rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);
        }
    }

    public void TakeDamage(int damage)
    {
        // Trigger hurt animation and stun effect
        animator.SetTrigger("Hurt");
        StartCoroutine(StunCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(1f); // Duration of the stun
        isStunned = false;
    }
}