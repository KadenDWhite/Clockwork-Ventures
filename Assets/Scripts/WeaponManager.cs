using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public bool isDrawn = false; //Whether the sword is drawn or not
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    void Update()
    {
        //Toggling between sheathe and draw with the Q key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleWeaponState();
        }
    }

    public void ToggleWeaponState()
    {
        isDrawn = !isDrawn; // Toggle weapon state

        if (isDrawn)
        {
            animator.SetBool("isDrawn", true);
            animator.SetBool("isSheathed", false);
        }
        else
        {
            animator.SetBool("isDrawn", false);
            animator.SetBool("isSheathed", true);
        }
    }

    // This function can be called by other scripts to check if the weapon is drawn
    public bool IsWeaponDrawn()
    {
        return isDrawn;
    }
}