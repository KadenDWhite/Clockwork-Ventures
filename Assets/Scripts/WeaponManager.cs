using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public bool isDrawn = false;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleWeaponState();
        }
    }

    public void ToggleWeaponState()
    {
        isDrawn = !isDrawn;
        playerMovement.SetWeaponState(isDrawn); // Update player movement animator
    }

    public bool IsWeaponDrawn()
    {
        return isDrawn;
    }
}