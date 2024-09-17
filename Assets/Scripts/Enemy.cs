using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int maxHP = 100;
    int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDMG(int dmg)
    {
        currentHP -= dmg;

        animator.SetTrigger("Hurt");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

   
}
