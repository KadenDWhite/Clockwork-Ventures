using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public Animator animator;
    public int maxHP = 100;
    public int currentHP;
    public KnockbackManager knockbackManager;

    public ChallengeManager challengeManager; // Reference to ChallengeManager

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDMG(int dmg, GameObject attacker)
    {
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
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Notify the ChallengeManager that this enemy died
        if (challengeManager != null)
        {
            challengeManager.EnemyKilled(); // Call the EnemyKilled method in ChallengeManager
        }
    }
}