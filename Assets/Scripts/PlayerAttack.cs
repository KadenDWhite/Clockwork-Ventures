using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;
    private Animator animator;

    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (weaponManager.IsWeaponDrawn() && Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }
}