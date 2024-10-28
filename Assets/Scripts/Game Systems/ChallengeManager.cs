using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChallengeManager : MonoBehaviour
{
    public GameObject portal;         
    public TextMeshProUGUI killText;  
    public SuperPupSystems.Helper.Timer timer;

    private int totalEnemies = 1;     
    private int enemyKilledCount = 0; 

    void Start()
    {
        portal.SetActive(false);  
        UpdateKillText();         
    }

    // Method to be called by the EnemyHP script when an enemy is killed
    public void EnemyKilled()
    {
        enemyKilledCount++;      
        UpdateKillText();        

        if (enemyKilledCount >= totalEnemies)
        {
            OnChallengeComplete();  
        }
    }

    void UpdateKillText()
    {
        killText.text = enemyKilledCount + "/" + totalEnemies + " killed";  
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
        portal.SetActive(true);
    }
}