using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public Animator animator;
    public int maxHP = 100;
    public int currentHP;
    public KnockbackManager knockbackManager;
    public ChallengeManager challengeManager;
    [SerializeField] private string deathAnimName;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        isDead = false;
    }

    public bool IsDead()
    {
        return isDead;
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

        if (knockbackManager != null)
        {
            knockbackManager.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    private IEnumerator HandleEnemyDeath()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"));

        float deathAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deathAnimLength);

        gameObject.SetActive(false);
    }
}