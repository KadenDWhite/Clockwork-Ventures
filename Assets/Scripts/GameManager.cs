using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }
    }

    public void AllowContinue()
    {
        // Logic for allowing the player to continue the game
        Debug.Log("You can now continue the game!");
    }
}
