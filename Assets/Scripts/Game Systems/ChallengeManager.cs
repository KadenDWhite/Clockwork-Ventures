using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeManager : MonoBehaviour
{
    public string portalKey = "Portal1"; // Unique identifier for this portal
    public GameObject portal;

    public TextMeshProUGUI killText; // Displays enemy kills
    public TextMeshProUGUI pickupText; // Displays pickups

    public SuperPupSystems.Helper.Timer timer;

    public int totalEnemies = 1;
    public int totalPickups = 0;
    public string keyName = "key"; // The name of the key item

    private int enemyKilledCount = 0;
    private int pickupCount = 0;

    public bool requirePickups = false;

    private Backpack playerBackpack; // Reference to the player's backpack

    void Start()
    {
        // Find and reference the player's Backpack
        playerBackpack = FindObjectOfType<Backpack>();

        if (portal != null)
        {
            // Set the portal active based on GameManager state
            portal.SetActive(GameManager.Instance.GetPortalState(portalKey));
        }

        UpdateKillText();
        UpdatePickupText();
    }

    public void EnemyKilled()
    {
        enemyKilledCount++;
        UpdateKillText();
        CheckChallengeComplete();
    }

    public void PickupCollected(string itemName)
    {
        // If the item is the key, we track it specifically
        if (itemName == keyName)
        {
            if (playerBackpack != null && !playerBackpack.HasItem(keyName))
            {
                playerBackpack.AddItem(keyName);
            }
        }

        pickupCount++;
        UpdatePickupText();
        CheckChallengeComplete();
    }

    void UpdateKillText()
    {
        if (killText != null)
        {
            killText.text = $"{enemyKilledCount}/{totalEnemies} Enemies Killed";
        }
    }

    void UpdatePickupText()
    {
        if (pickupText != null)
        {
            pickupText.text = $"{pickupCount}/{totalPickups}";
        }
    }

    void CheckChallengeComplete()
    {
        bool enemiesComplete = enemyKilledCount >= totalEnemies;
        bool pickupsComplete = !requirePickups || pickupCount >= totalPickups;

        // Additionally check if the key is in the backpack if it's required for challenge completion
        bool keyComplete = playerBackpack != null && playerBackpack.HasItem(keyName);

        // Challenge is complete if all conditions are met
        if (enemiesComplete && pickupsComplete && keyComplete)
        {
            OnChallengeComplete();
        }
    }

    void OnChallengeComplete()
    {
        ActivatePortal();
        if (timer != null)
        {
            timer.StopTimer();
            Debug.Log("Challenge complete! Timer stopped.");
        }
        else
        {
            Debug.Log("Challenge complete! No timer to stop.");
        }
    }

    void ActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);
        }

        // Save portal state in GameManager
        GameManager.Instance.SetPortalState(portalKey, true);
    }
}