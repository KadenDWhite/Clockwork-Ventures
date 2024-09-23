using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNPC : MonoBehaviour
{
    public GameObject tutorialEnemy; // Assign the goblin GameObject in the inspector
    private string[] dialog;
    private int currentDialogIndex = 0;

    void Start()
    {
        dialog = new string[] {
            "Welcome to the tutorial!",
            "Here, you'll learn how to fight.",
            "Press 'E' to attack.",
            "Watch out for enemies!",
            "Here's a goblin to practice on!" // Element 4
        };

        tutorialEnemy.SetActive(false); // Ensure goblin is inactive at the start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowDialog();
        }
    }

    private void ShowDialog()
    {
        if (currentDialogIndex < dialog.Length)
        {
            Debug.Log(dialog[currentDialogIndex]); // Replace with your dialog display logic

            if (currentDialogIndex == 3) // When reaching element 4 (index 3)
            {
                tutorialEnemy.SetActive(true); // Activate the goblin
            }

            currentDialogIndex++;
        }
        else
        {
            // Dialog is finished, implement further logic if needed
            currentDialogIndex = 0; // Reset or disable the NPC interaction
        }
    }
}