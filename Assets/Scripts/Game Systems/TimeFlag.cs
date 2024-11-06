using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPupSystems.Helper; // Include the namespace for the Timer

public class TimeFlag : MonoBehaviour
{
    public Timer timer; // Reference to the Timer component

    // This method will be called when another collider enters this collider's trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player (replace "Player" with your player tag if different)
        if (other.CompareTag("Player"))
        {
            // Enable the Timer component to start the countdown
            if (timer != null && !timer.enabled)
            {
                timer.enabled = true;
                timer.StartTimer(timer.countDownTime); // Start the timer
            }
        }
    }
}