using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float speed = 2f;
    public float chaseDistanceThreshold = 3f;
    public float attackDistanceThreshold = 0.8f;
    public float attackDelay = 1f;

    private float attackTimer;
    private Animator animator;
    private bool isFacingRight = false;
    private PlayerHP playerHP;

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackTimer = attackDelay;
        playerHP = player.GetComponent<PlayerHP>();
    }

    private void Update()
    {
        if (player == null || playerHP.currentHP <= 0)
        {
            StopMovement();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < chaseDistanceThreshold)
        {
            if (distance <= attackDistanceThreshold)
            {
                if (attackTimer >= attackDelay)
                {
                    Attack();
                    attackTimer = 0;
                }
                else
                {
                    StopMovement();
                }
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            StopMovement();
        }

        attackTimer += Time.deltaTime;
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        FlipSprite(direction);

        animator.SetFloat("xVelocity", Mathf.Abs(direction.x));
        animator.SetFloat("yVelocity", direction.y);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void StopMovement()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    private void Attack()
    {
        Debug.Log("Attacking");  // Log to check if Attack() is being called
        animator.SetBool("isAttacking", true);
        // Call additional attack functionality here if needed
    }

    private void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0 && !isFacingRight || direction.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
}