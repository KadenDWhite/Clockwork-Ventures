using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TypewriterEffect2 typewriterEffect; // Reference to the TypewriterEffect2 script
    [SerializeField] private GameObject deathScreen; // Reference to the Death Screen GameObject (parent)

    private void Start()
    {
        deathScreen.SetActive(false); // Ensure the Death Screen is inactive at the start
    }

    public void OnPlayerDeath()
    {
        deathScreen.SetActive(true);  // Activate the entire Death Screen (which includes Death Text)
        typewriterEffect.Run();  // Run the typewriter effect on the Death Text
    }
}
