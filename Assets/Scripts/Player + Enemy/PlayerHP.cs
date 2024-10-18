using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Animator animator;
    public Image healthBarImage; // Use Image instead of SpriteRenderer for UI health bar
    public TextMeshProUGUI healthText;
    public KnockbackManager knockbackManager;

    public int maxHP = 100;
    public int currentHP;
    public GameObject death;
    public PauseMenu pauseMenuUI;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public WeaponManager weaponManager;

    public Sprite[] healthSprites; // Array for health bar sprites (6 in total)
    public SuperPupSystems.Helper.Timer timer;
    public GameObject timeText;

    public AnimationClip deathAnimationClip;
    private bool isDead = false;

    // Add reference for GameManager
    private GameManager gameManager;

    // Add AudioSource reference for background music or other sounds
    public AudioSource backgroundMusicSource;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();

        gameManager = FindObjectOfType<GameManager>();

        if (timer != null)
        {
            timer.timeout.AddListener(TimerRanOut);
        }
    }

    public void TakeDMG(int dmg, GameObject attacker)
    {
        if (isDead) return;

        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        animator.SetTrigger("Hurt");
        UpdateHealthBar();
        UpdateHealthText();

        if (knockbackManager != null && currentHP > 0)
        {
            knockbackManager.PlayFeedback(attacker);
        }

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
            healthBarImage.sprite = healthSprites[0]; // 0% health
        }
        else if (healthPercentage <= 0.2f)
        {
            healthBarImage.sprite = healthSprites[1]; // 20% health
        }
        else if (healthPercentage <= 0.4f)
        {
            healthBarImage.sprite = healthSprites[2]; // 40% health
        }
        else if (healthPercentage <= 0.6f)
        {
            healthBarImage.sprite = healthSprites[3]; // 60% health
        }
        else if (healthPercentage <= 0.8f)
        {
            healthBarImage.sprite = healthSprites[4]; // 80% health
        }
        else
        {
            healthBarImage.sprite = healthSprites[5]; // 100% health
        }
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "" + currentHP.ToString();
        }
    }

    void Die()
    {
        Debug.Log("You died!");
        isDead = true; // Set the player as dead

        // Stop the background music or audio source if it's not null
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }

        // Play the death animation directly
        if (animator != null && deathAnimationClip != null)
        {
            animator.Play(deathAnimationClip.name); // Play the animation clip
        }

        DisablePlayerComponents();
        StartCoroutine(HandleDeath());
    }

    // Method to disable PlayerMovement, PlayerAttack, WeaponManager, and KnockbackManager scripts
    void DisablePlayerComponents()
    {
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerAttack != null) playerAttack.enabled = false;
        if (weaponManager != null) weaponManager.enabled = false;
        if (knockbackManager != null) knockbackManager.enabled = false;
        if (pauseMenuUI != null) pauseMenuUI.enabled = false;
        timeText.SetActive(false);
    }

    // Coroutine to handle the delay before showing the death screen
    IEnumerator HandleDeath()
    {
        float deathAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the duration of the death animation
        yield return new WaitForSeconds(deathAnimLength + 1.0f);
        Debug.Log("Death Animation Length: " + deathAnimLength);

        Debug.Log("Activating death screen");
        death.SetActive(true);
    }

    public void TimerRanOut()
    {
        currentHP = 0;
        UpdateHealthBar();
        UpdateHealthText();

        if (!isDead)
        {
            Die();
        }
    }
}