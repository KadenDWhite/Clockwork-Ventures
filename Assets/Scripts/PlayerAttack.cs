using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Handle attack input
        if (Input.GetButtonDown("Fire1")) // Replace with your attack button
        {
            if (!isAttacking)
            {
                StartAttack();
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack"); // Use a trigger for attack animation
    }

    // This function should be called from an animation event or elsewhere to reset attacking state
    public void EndAttack()
    {
        isAttacking = false;
    }
}
