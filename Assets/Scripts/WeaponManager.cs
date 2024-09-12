using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public bool isDrawn = false; // Whether the sword is drawn or not
    public bool isSheathed = true; // Whether the sword is sheathed or not
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Toggle between sheathe and draw with the Q key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleWeaponState();
        }
    }

    public void ToggleWeaponState()
    {
        if (isDrawn)
        {
            // If drawn, sheath it
            animator.SetTrigger("Sheathe");
            isDrawn = false;
            isSheathed = true;
        }
        else
        {
            // If sheathed, draw it
            animator.SetTrigger("Draw");
            isDrawn = true;
            isSheathed = false;
        }
    }

    // This function can be called by other scripts to check if the weapon is drawn
    public bool IsWeaponDrawn()
    {
        return isDrawn;
    }
}