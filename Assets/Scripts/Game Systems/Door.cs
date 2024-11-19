using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapScript : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load

    private bool playerInTrigger = false; // Whether the player is within the trigger zone

    void Update()
    {
        // If the player is within the trigger zone and presses 'E' to interact
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            LoadScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the player enters the door's trigger area
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true; // Mark player as inside trigger zone
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When the player exits the door's trigger area
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false; // Mark player as outside trigger zone
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad)) // Check if a scene name is provided
        {
            Debug.Log("Loading scene: " + sceneToLoad);

            // You can save any game data here if needed before loading the scene
            // GameManager.Instance.SaveGameStates();

            // Load the new scene
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name not specified!");
        }
    }
}