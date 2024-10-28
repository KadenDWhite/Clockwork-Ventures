using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public Animator animator; // Animator component reference
    public int maxHP = 100; // Maximum health points
    public int currentHP; // Current health points
    public KnockbackManager knockbackManager; // Reference to KnockbackManager
    public ChallengeManager challengeManager; // Reference to ChallengeManager

    [SerializeField] private string deathAnimName; // Name of the death animation

    private bool isDead = false; // Flag to check if the enemy is dead

    void Start()
    {
        currentHP = maxHP; // Initialize current health to maximum
    }

    public void TakeDMG(int dmg, GameObject attacker)
    {
        if (isDead) return; // Prevent damage if already dead

        currentHP -= dmg; // Reduce current health by damage taken
        animator.SetTrigger("Hurt"); // Trigger hurt animation

        // Handle knockback effect
        if (knockbackManager != null)
        {
            knockbackManager.PlayFeedback(attacker);
        }

        // Check if the enemy has died
        if (currentHP <= 0)
        {
            Die(); // Call Die method
        }
    }

    public void Die()
    {
        if (isDead) return; // Prevent multiple calls to Die

        isDead = true; // Set dead flag to true
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true); // Trigger death animation

        // Disable enemy components
        DisableEnemyComponents();

        StartCoroutine(HandleEnemyDeath()); // Start the death coroutine

        // Notify challenge manager about the enemy's death
        if (challengeManager != null)
        {
            challengeManager.EnemyKilled();
        }
    }

    private void DisableEnemyComponents()
    {
        // Disable EnemyAI
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false; // Disable AI behavior
        }

        // Disable KnockbackManager
        if (knockbackManager != null)
        {
            knockbackManager.enabled = false; // Disable knockback manager
        }

        // Disable physics interactions
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false; // Disable physics interactions
        }

        // Disable collisions
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false; // Disable collisions
        }
    }

    private IEnumerator HandleEnemyDeath()
    {
        // Wait until the animator starts playing any animation tagged as "Death"
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"));

        // Get the length of the current death animation
        float deathAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the animation to complete
        yield return new WaitForSeconds(deathAnimLength);

        gameObject.SetActive(false); // Disable the enemy object
    }
}