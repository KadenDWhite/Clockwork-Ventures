using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private WeaponManager weaponManager; //Reference to WeaponManager

    
    void Start()
    {
        animator = GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManager>(); // Assuming WeaponManager is on the same GameObject
    }

    
    void Update()
    {
        if (weaponManager.IsWeaponDrawn())
        {
            Attack();
        }
    }

    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("isAttacking", true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
