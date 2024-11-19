using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    private bool playerInPortal = false;
    private bool portalActivated = false; // Flag to track if the portal can be used

    // Public variable to specify which scene to load
    public string sceneToLoad;

    void Update()
    {
        // If the player is near the portal and presses 'B', activate the portal
        if (playerInPortal && Input.GetKeyDown(KeyCode.B))
        {
            portalActivated = true;
            Debug.Log("Portal is now active. Press 'E' to enter.");
        }

        // If the player is in the portal and presses 'E' to enter, load the specified scene
        if (playerInPortal && portalActivated && Input.GetKeyDown(KeyCode.E))
        {
            if (!string.IsNullOrEmpty(sceneToLoad)) // Check if a scene name is set
            {
                SceneManager.LoadScene(sceneToLoad); // Load the specified scene
            }
            else
            {
                Debug.LogWarning("Scene name not specified!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the player enters the portal trigger zone
        if (other.CompareTag("Player"))
        {
            playerInPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player exits the portal trigger zone
        if (other.CompareTag("Player"))
        {
            playerInPortal = false;
            portalActivated = false; // Deactivate the portal if the player leaves the trigger zone
        }
    }
}