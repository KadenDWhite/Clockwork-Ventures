using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    private bool playerInPortal = false;

    void Update()
    {
        if (playerInPortal && Input.GetKeyUp(KeyCode.E))
        {
            SceneManager.LoadScene("Level 1");
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