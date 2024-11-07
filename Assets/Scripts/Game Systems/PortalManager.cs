using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    private bool playerInPortal = false;

    // Public variable to specify which scene to load
    public string sceneToLoad;

    void Update()
    {
        // If the player is in the portal and presses the 'E' key, load the specified scene
        if (playerInPortal && Input.GetKeyUp(KeyCode.E))
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
        if (other.CompareTag("Player"))
        {
            playerInPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInPortal = false;
        }
    }
}