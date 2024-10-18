using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour // Rename class if necessary
{
    public Animator animator;
    public int maxHP = 100;
    public int currentHP;
    public KnockbackManager knockbackManager;

    public ChallengeManager challengeManager;

    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDMG(int dmg, GameObject attacker)
    {
        if (isDead) return;

        currentHP -= dmg;

        animator.SetTrigger("Hurt");

        if (knockbackManager != null)
        {
            knockbackManager.PlayFeedback(attacker);
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);
        
        DisableEnemyComponents();
        
        StartCoroutine(HandleEnemyDeath());

        if (challengeManager != null)
        {
            challengeManager.EnemyKilled(); 
        }
    }

    private void DisableEnemyComponents()
    {
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false; // Disable physics interactions
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false; // Disable collisions
        }
    }

    private IEnumerator HandleEnemyDeath()
    {
        
        float deathAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;

       
        yield return new WaitForSeconds(deathAnimLength + 1.0f); 

       
        gameObject.SetActive(false);
    }
}