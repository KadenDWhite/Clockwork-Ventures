using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TypewriterEffect2 typewriterEffect;
    [SerializeField] private TypewriterEffect2 typewriterEffect2;
    [SerializeField] private GameObject deathScreen; 

    private void Start()
    {
        deathScreen.SetActive(false);
    }

    public void OnPlayerDeath()
    {
        deathScreen.SetActive(true);
        typewriterEffect.Run();
        typewriterEffect2.Run();
    }
}
