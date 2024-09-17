using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float distanceBetween;
    public Animator animator;
    public SpriteRenderer spriteRenderer; // Add reference to SpriteRenderer

    private float distance;
    private bool isFacingRight = false; // Track which direction the enemy is facing

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>(); // Ensure SpriteRenderer is assigned
        }
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        distance = Vector2.Distance(transform.position, player.transform.position);

        // Calculate the direction towards the player and normalize it
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        // If the player is within the specified distance, the enemy moves towards the player
        if (distance < distanceBetween)
        {
            // Move the enemy towards the player
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

            // Set the "isRunning" parameter to true to trigger the run animation
            animator.SetBool("isRunning", true);

            // Flip the sprite based on movement direction
            FlipSprite(direction);
        }
        else
        {
            // If the enemy is not moving, set "isRunning" to false to trigger the idle animation
            animator.SetBool("isRunning", false);
        }

        // Set the "xVelocity" and "yVelocity" to control smooth transitions (optional)
        animator.SetFloat("xVelocity", Mathf.Abs(direction.x));
        animator.SetFloat("yVelocity", direction.y);
    }

    private void FlipSprite(Vector2 direction)
    {
        // Check if the enemy is moving left or right and flip sprite accordingly
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Invert the x scale to flip the sprite
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f; // Flip the sprite by inverting the x scale
        transform.localScale = ls;
    }
}