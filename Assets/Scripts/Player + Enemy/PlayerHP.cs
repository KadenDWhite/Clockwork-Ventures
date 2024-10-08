using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlayerHP : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public TextMeshProUGUI healthText; // Reference to the TextMeshProUGUI component

    public int maxHP = 100;
    public int currentHP;

    public Sprite[] healthSprites; // Array for health bar sprites (6 in total)

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar(); // Initialize the correct sprite based on starting health
    }

    public void TakeDMG(int dmg)
    {
        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // Ensure HP doesn't go below 0 or above maxHP

        animator.SetTrigger("Hurt");
        UpdateHealthBar(); // Update the health bar sprite after taking damage
        UpdateHealthText(); // Update health text display

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        // Calculate the percentage of health remaining
        float healthPercentage = (float)currentHP / maxHP;

        // Choose the correct sprite based on the health percentage
        if (healthPercentage <= 0)
        {
            spriteRenderer.sprite = healthSprites[0]; // 0% health
        }
        else if (healthPercentage <= 0.2f)
        {
            spriteRenderer.sprite = healthSprites[1]; // 20% health
        }
        else if (healthPercentage <= 0.4f)
        {
            spriteRenderer.sprite = healthSprites[2]; // 40% health
        }
        else if (healthPercentage <= 0.6f)
        {
            spriteRenderer.sprite = healthSprites[3]; // 60% health
        }
        else if (healthPercentage <= 0.8f)
        {
            spriteRenderer.sprite = healthSprites[4]; // 80% health
        }
        else
        {
            spriteRenderer.sprite = healthSprites[5]; // 100% health
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "" + currentHP.ToString(); // Update the text display with current HP
        }
    }

    void Die()
    {
        Debug.Log("You died!");

        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;

        // Disable the entire player object when the player dies
        gameObject.SetActive(false); // Disable the player game object
    }
}