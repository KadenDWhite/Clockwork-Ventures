using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public ChallengeManager challengeManager;
    public AudioClip pickupSound;
    public string itemName;

    private AudioSource audioSource;
    private bool isCollected = false;  // To prevent multiple pickups

    private void Start()
    {
        // Initialize the audio source component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pickupSound;
        audioSource.playOnAwake = false;

        // Get the ChallengeManager reference if not already set
        if (challengeManager == null)
        {
            challengeManager = FindObjectOfType<ChallengeManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collides with the pickup and if it hasn't been collected yet
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Notify the ChallengeManager that the item was picked up
            if (challengeManager != null)
            {
                challengeManager.PickupCollected(itemName);  // Optionally pass the item name to track in ChallengeManager
            }

            // Play the pickup sound if available
            if (pickupSound != null)
            {
                audioSource.Play();
            }

            // Hide the item and disable its collider so it can't be collected again
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Destroy the pickup object after the sound finishes playing
            Destroy(gameObject, pickupSound.length);
        }
    }
}