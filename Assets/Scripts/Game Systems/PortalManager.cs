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

    // Reference to the Backpack script to check if the player has the key
    private Backpack backpack;

    void Start()
    {
        // Find the Backpack object (ensure it is available in the scene)
        backpack = Backpack.Instance;
    }

    void Update()
    {
        // If the player is near the portal and presses 'B', check if the portal can be activated
        if (playerInPortal && Input.GetKeyDown(KeyCode.B))
        {
            if (backpack != null && backpack.HasItem("Key"))
            {
                portalActivated = true;
                Debug.Log("Portal is now active. Press 'E' to enter.");
            }
            else
            {
                portalActivated = false;
                Debug.Log("You need the Key to activate the portal.");
            }
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