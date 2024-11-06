using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Animator animator;
    public Image healthBarImage;
    public TextMeshProUGUI healthText;
    public KnockbackManager knockbackManager;
    public int maxHP = 100;
    public int currentHP;
    public GameObject death;
    public PauseMenu pauseMenuUI;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public WeaponManager weaponManager;
    public Sprite[] healthSprites;
    public SuperPupSystems.Helper.Timer timer;
    public GameObject timeText;
    public AnimationClip deathAnimationClip;
    private bool isDead = false;
    private GameManager gameManager;
    public AudioSource backgroundMusicSource;
    public Shield shield;

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

    public bool IsDead()
    {
        return isDead;
    }

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

    void UpdateHealthBar()
    {
        float healthPercentage = (float)currentHP / maxHP;

        if (healthPercentage <= 0)
        {
            healthBarImage.sprite = healthSprites[0];
        }
        else if (healthPercentage <= 0.2f)
        {
            healthBarImage.sprite = healthSprites[1];
        }
        else if (healthPercentage <= 0.4f)
        {
            healthBarImage.sprite = healthSprites[2];
        }
        else if (healthPercentage <= 0.6f)
        {
            healthBarImage.sprite = healthSprites[3];
        }
        else if (healthPercentage <= 0.8f)
        {
            healthBarImage.sprite = healthSprites[4];
        }
        else
        {
            healthBarImage.sprite = healthSprites[5];
        }
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