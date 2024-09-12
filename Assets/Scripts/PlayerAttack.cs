using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;

    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    public bool IsAttacking()
    {
        if (weaponManager.IsWeaponDrawn() && Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }
}