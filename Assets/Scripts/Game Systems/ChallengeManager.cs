using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeManager : MonoBehaviour
{
    public GameObject portal;

    public TextMeshProUGUI killText; // Displays enemy kills
    public TextMeshProUGUI pickupText; // Displays pickups

    public SuperPupSystems.Helper.Timer timer;

    public int totalEnemies = 1;
    public int totalPickups = 0;

    public int enemyKilledCount = 0;
    public int pickupCount = 0;

    public bool requirePickups = false;

    void Start()
    {
        if (portal != null)
        {
            // Set the portal active based on GameManager state (no portalKey needed anymore)
            portal.SetActive(true); // Automatically activate the portal, assuming no key condition
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
        pickupCount++;
        UpdatePickupText();
        CheckChallengeComplete();
    }

    void UpdateKillText()
    {
        if (killText != null)
        {
            killText.text = $"{enemyKilledCount}/{totalEnemies} Killed";
        }
    }

    public void UpdatePickupText()
    {
        if (pickupText != null)
        {
            pickupText.text = $"{pickupCount}/{totalPickups}";
        }
    }

    public void CheckChallengeComplete()
    {
        bool enemiesComplete = enemyKilledCount >= totalEnemies;
        bool pickupsComplete = !requirePickups || pickupCount >= totalPickups;

        // Challenge is complete if all conditions are met
        if (enemiesComplete && pickupsComplete)
        {
            OnChallengeComplete();
        }
    }

    public void OnChallengeComplete()
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

    public void ActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);
        }
    }
}