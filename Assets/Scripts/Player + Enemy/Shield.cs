using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Tooltip("Time in seconds that the shield will stay active")]
    public float shieldDuration = 5.0f;

    [Tooltip("Time in seconds before the shield can be used again")]
    public float shieldCooldown = 10.0f;

    [Tooltip("Speed of the shield's rotation (adjust as needed)")]
    public float rotationSpeed = 100.0f;

    [Tooltip("Visual effect object for the shield that will rotate when active")]
    public GameObject shieldEffect;

    private bool isShieldActive = false;
    private bool isCooldown = false;

    void Update()
    {
        // Activates shield if not on cooldown and player presses the shield button (e.g., 'F' key)
        if (Input.GetKeyDown(KeyCode.F) && !isCooldown)
        {
            StartCoroutine(ActivateShield());
        }

        // Rotate shield effect if it's active
        if (isShieldActive && shieldEffect != null)
        {
            shieldEffect.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator ActivateShield()
    {
        isShieldActive = true;        // Set shield to active
        isCooldown = true;            // Start cooldown
        shieldEffect.SetActive(true); // Activate shield effect

        // The shield stays active for `shieldDuration` seconds
        yield return new WaitForSeconds(shieldDuration);

        // Disable shield effect after duration
        isShieldActive = false;
        shieldEffect.SetActive(false);

        // Wait for cooldown period before shield can be reactivated
        yield return new WaitForSeconds(shieldCooldown);
        isCooldown = false;
    }

    [Tooltip("Returns true if the shield is currently active (for checking in other scripts)")]
    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}