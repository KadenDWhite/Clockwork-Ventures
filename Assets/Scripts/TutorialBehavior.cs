using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBehavior : MonoBehaviour
{
    private PlayerHP playerHP; // Reference to the player's health script

    void Start()
    {
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHP>();
    }

    void Update()
    {
        // Check if the enemy's health is 0 or below (implement your own health check)
        if (GetComponent<EnemyHP>().currentHP <= 0)
        {
            HandleDefeat();
        }
    }

    private void HandleDefeat()
    {
        // Handle what happens when the enemy is defeated
        // For example, notify the TutorialShopNPC or allow the player to continue
        Debug.Log("Tutorial Enemy Defeated!");

        // Notify the TutorialShopNPC or game manager
        GameManager.Instance.AllowContinue(); // Implement this method in your GameManager

        // Optionally destroy the enemy
        Destroy(gameObject);
    }
}

