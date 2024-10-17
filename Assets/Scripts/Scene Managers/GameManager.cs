using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TypewriterEffect2 typewriterEffect; // Reference to the TypewriterEffect2 script
    [SerializeField] private GameObject deathText; // Reference to the Death Text GameObject

    private void Start()
    {
        deathText.SetActive(false);
    }

    public void OnPlayerDeath()
    {
        deathText.SetActive(true);
        typewriterEffect.Run();
    }
}