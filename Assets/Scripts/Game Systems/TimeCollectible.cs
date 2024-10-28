using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCollectible : MonoBehaviour
{
    public float timeToAdd = 10.0f;
    public SuperPupSystems.Helper.Timer timer;
    public AudioClip pickupSound;

    private AudioSource audioSource;
    private bool isCollected = false; // To ensure pickup happens only once

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = pickupSound;
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true; // Prevent multiple pickups

            if (timer != null)
            {
                timer.AddTime(timeToAdd);
                Debug.Log("Time increased by: " + timeToAdd);
            }

            if (pickupSound != null)
            {
                audioSource.Play();
            }

            // Make the collectible invisible and disable the collider immediately
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Destroy the collectible after the sound finishes playing
            Destroy(gameObject, pickupSound.length);
        }
    }
}