using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCollectible : MonoBehaviour
{
    public float timeToAdd = 10.0f; // Amount of time to add when collected
    public SuperPupSystems.Helper.Timer timer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add time to the timer
            if (timer != null)
            {
                timer.AddTime(timeToAdd);
                Debug.Log("Time increased by: " + timeToAdd);
            }

            // Destroy the collectible object after being collected
            Destroy(gameObject);
        }
    }
}