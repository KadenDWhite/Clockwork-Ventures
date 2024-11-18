using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [Tooltip("Speed of the platform movement.")]
    [SerializeField] private float speed = 2f;

    [Tooltip("Distance the platform travels from its starting position.")]
    [SerializeField] private float moveDistance = 5f;

    private Vector3 startPosition;
    private int direction = 1; // 1 for right, -1 for left

    void Start()
    {
        // Store the initial position of the platform
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the platform left and right
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // Reverse direction when the platform reaches its move distance
        if (Mathf.Abs(transform.position.x - startPosition.x) >= moveDistance)
        {
            direction *= -1;
        }
    }

    // Detect when the player lands on the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Make the player a child of the platform
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove the player as a child of the platform
            collision.transform.SetParent(null);
        }
    }
}