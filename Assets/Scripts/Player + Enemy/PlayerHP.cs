using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [Header("UI Components")]
    public Image healthBarImage;
    public TextMeshProUGUI healthText;

    [Header("Player Components")]
    public Animator animator;
    public KnockbackManager knockbackManager;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public WeaponManager weaponManager;
    public GameObject timeText;
    public PauseMenu pauseMenuUI;

    [Header("Health and Shield")]
    public int maxHP = 100;
    public int currentHP;
    public Sprite[] healthSprites;
    public Shield shield;

    [Header("Audio and Effects")]
    public AudioSource backgroundMusicSource;
    public GameObject death;

    [Header("Gameplay References")]
    public SuperPupSystems.Helper.Timer timer;
    private GameManager gameManager;

    private bool isDead = false;

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

    public bool IsDead() => isDead;

    public void TakeDMG(int dmg, GameObject attacker)
    {
        // Early exit if player is already dead or shield is active
        if (isDead || (shield != null && shield.IsShieldActive())) return;

        // Apply damage and clamp health
        currentHP = Mathf.Clamp(currentHP - dmg, 0, maxHP);
        UpdateHealthBar();
        UpdateHealthText();

        // Trigger hurt animation if player is still alive
        if (currentHP > 0)
        {
            animator.SetTrigger("Hurt");

            // Apply knockback effect if applicable
            knockbackManager?.PlayFeedback(attacker);
        }

        // Check if health has reached zero to trigger death
        if (currentHP <= 0) Die();
    }

    void UpdateHealthBar()
    {
        float healthPercentage = (float)currentHP / maxHP;
        int spriteIndex = Mathf.Clamp((int)(healthPercentage * healthSprites.Length), 0, healthSprites.Length - 1);
        healthBarImage.sprite = healthSprites[spriteIndex];
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = currentHP.ToString();
        }
    }

    void Die()
    {
        if (isDead) return;

        Debug.Log("You died!");
        isDead = true;

        animator.SetBool("IsDead", true);
        animator.CrossFade("Death", 0.1f);
        backgroundMusicSource?.Stop();

        DisablePlayerComponents();

        if (death != null)
        {
            death.SetActive(true);
        }

        StartCoroutine(HandleDeath());
    }

    void DisablePlayerComponents()
    {
        if (knockbackManager != null) knockbackManager.enabled = false;
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerAttack != null) playerAttack.enabled = false;
        if (weaponManager != null) weaponManager.enabled = false;
        if (pauseMenuUI != null) pauseMenuUI.enabled = false;

        if (timeText != null)
        {
            timeText.SetActive(false);
        }
    }

    private IEnumerator HandleDeath()
    {
        // Ensure the death screen is inactive at the start
        if (death != null)
        {
            death.SetActive(false);
        }

        // Wait until the "Death" animation starts playing
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"));

        // Get the length of the death animation
        float deathAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the animation to finish before showing the death screen
        yield return new WaitForSeconds(deathAnimLength);

        // After the death animation finishes, activate the death screen
        if (death != null)
        {
            death.SetActive(true);
        }

        // Optionally, disable the player object if desired
        gameObject.SetActive(false);
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