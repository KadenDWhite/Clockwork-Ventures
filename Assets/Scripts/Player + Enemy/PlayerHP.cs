using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Tooltip("Animator component to control the player's animations.")]
    public Animator animator;

    [Tooltip("UI element representing the player's health bar.")]
    public Image healthBarImage;

    [Tooltip("Text element to display the player's current health.")]
    public TextMeshProUGUI healthText;

    [Tooltip("Reference to the KnockbackManager to apply knockback effects when damaged.")]
    public KnockbackManager knockbackManager;

    [Tooltip("The maximum health points the player can have.")]
    public int maxHP = 100;

    [Tooltip("The player's current health points.")]
    public int currentHP;

    [Tooltip("Game object representing the death screen.")]
    public GameObject death;

    [Tooltip("Reference to the pause menu UI for managing the game state when paused.")]
    public PauseMenu pauseMenuUI;

    [Tooltip("Reference to the PlayerMovement script for controlling player movement.")]
    public PlayerMovement playerMovement;

    [Tooltip("Reference to the PlayerAttack script for handling player attacks.")]
    public PlayerAttack playerAttack;

    [Tooltip("Reference to the WeaponManager script for handling weapon actions.")]
    public WeaponManager weaponManager;

    [Tooltip("Array of sprites representing different health levels for the health bar.")]
    public Sprite[] healthSprites;

    [Tooltip("Timer used to track specific time-related events in the game.")]
    public SuperPupSystems.Helper.Timer timer;

    [Tooltip("UI element for displaying time remaining.")]
    public GameObject timeText;

    [Tooltip("Animation clip to play when the player dies.")]
    public AnimationClip deathAnimationClip;

    [Tooltip("Indicates whether the player is currently dead.")]
    private bool isDead = false;

    [Tooltip("Reference to the GameManager for controlling game-related state and logic.")]
    private GameManager gameManager;

    [Tooltip("Audio source for playing background music or sound effects.")]
    public AudioSource backgroundMusicSource;

    [Tooltip("Reference to the Shield component for checking if the shield is active.")]
    public Shield shield;

    void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();

        gameManager = FindObjectOfType<GameManager>();

        // Add timer listener if timer is available
        if (timer != null)
        {
            timer.timeout.AddListener(TimerRanOut);
        }
    }

    /// <summary>
    /// Applies damage to the player. If the shield is active or the player is dead, damage is not applied.
    /// </summary>
    /// <param name="dmg">Amount of damage to apply.</param>
    /// <param name="attacker">The game object causing the damage.</param>
    public void TakeDMG(int dmg, GameObject attacker)
    {
        if (isDead || (shield != null && shield.IsShieldActive())) return;

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

    /// <summary>
    /// Updates the health bar image based on the player's current health percentage.
    /// </summary>
    void UpdateHealthBar()
    {
        float healthPercentage = (float)currentHP / maxHP;

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

    /// <summary>
    /// Updates the health text display to show the player's current health points.
    /// </summary>
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "" + currentHP.ToString();
        }
    }

    /// <summary>
    /// Handles the player's death, including animations, UI updates, and disabling certain components.
    /// </summary>
    void Die()
    {
        Debug.Log("You died!");
        isDead = true;

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }

        if (animator != null && deathAnimationClip != null)
        {
            animator.Play(deathAnimationClip.name);
        }

        DisablePlayerComponents();
        StartCoroutine(HandleDeath());
    }

    /// <summary>
    /// Disables player-related components upon death.
    /// </summary>
    void DisablePlayerComponents()
    {
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerAttack != null) playerAttack.enabled = false;
        if (weaponManager != null) weaponManager.enabled = false;
        if (knockbackManager != null) knockbackManager.enabled = false;
        if (pauseMenuUI != null) pauseMenuUI.enabled = false;

        if (timeText != null)
        {
            timeText.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine to manage the delay before activating the death screen UI.
    /// </summary>
    IEnumerator HandleDeath()
    {
        float deathAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(deathAnimLength + 1.0f);
        Debug.Log("Death Animation Length: " + deathAnimLength);

        Debug.Log("Activating death screen");
        if (death != null)
        {
            death.SetActive(true);
        }
    }

    /// <summary>
    /// Method triggered when the timer runs out, resulting in player death.
    /// </summary>
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