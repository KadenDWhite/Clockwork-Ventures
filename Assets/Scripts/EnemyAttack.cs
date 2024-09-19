using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDMG = 20;

    public float attackRate = 2f;
    public float attackDelay = 1f;

    // Reference to the player and player's health
    public GameObject player;
    private PlayerHP playerHP;

    // Delay for starting the attack after reaching the player
    private float nextAttackTime = 0f;
    private float passedTime = 0f;
    private bool isAttacking = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHP = player.GetComponent<PlayerHP>();
        }
    }

    void Update()
    {
        if (playerHP == null || playerHP.currentHP <= 0)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // Only attack after the delay and when attack rate allows
        if (distance <= attackRange)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);

            if (passedTime >= attackDelay && Time.time >= nextAttackTime)
            {
                // Trigger attack after the delay
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;
                Attack();
            }
        }
        else
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }

        if (isAttacking)
        {
            passedTime += Time.deltaTime;
        }
        else
        {
            passedTime = 0f;
        }
    }

    // Attack logic
    public void Attack()
    {
        // Detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Deal damage to all enemies hit
        foreach (Collider2D enemy in hitEnemies)
        {
            PlayerHP playerHP = enemy.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDMG(attackDMG);
            }
        }

        // End the attack and reset isAttacking
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    // Visualize attack range in editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}