using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Door : MonoBehaviour
{
    [Tooltip("The ChallengeManager responsible for tracking key or challenge state.")]
    [SerializeField] private ChallengeManager challengeManager;

    [Tooltip("The closed door sprite.")]
    [SerializeField] private Sprite closedDoorSprite;

    [Tooltip("The open door sprite.")]
    [SerializeField] private Sprite openDoorSprite;

    [Tooltip("The scene to load when the door is opened.")]
    [SerializeField] private string sceneToLoad;

    [Tooltip("Reference to the UI text to show interaction message.")]
    [SerializeField] private TextMeshProUGUI interactionText;

    [Tooltip("Duration to display the interaction message.")]
    [SerializeField] private float messageDuration = 2f;

    [Tooltip("The minimum number of enemies killed to unlock the door (set to 0 to ignore).")]
    [SerializeField] private int requiredEnemyKills = 0;

    private bool playerInTrigger = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateDoorSprite();
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            TryToOpenDoor();
        }

        UpdateDoorSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            ShowMessage("Press E to interact.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            HideMessage();
        }
    }

    private void TryToOpenDoor()
    {
        if (challengeManager != null)
        {
            bool enemiesConditionMet = challengeManager.totalEnemies > 0 && challengeManager.enemyKilledCount >= requiredEnemyKills;
            bool pickupsConditionMet = challengeManager.requirePickups && challengeManager.totalPickups > 0 && challengeManager.totalPickups == challengeManager.pickupCount;

            if (enemiesConditionMet || pickupsConditionMet)
            {
                ShowMessage("Conditions met! Door unlocked.");
                LoadScene();
            }
            else
            {
                ShowMessage($"Kill {requiredEnemyKills - challengeManager.enemyKilledCount} more enemies or collect key(s) to open this door.");
            }
        }
        else
        {
            ShowMessage("Challenge Manager not assigned.");
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void UpdateDoorSprite()
    {
        if (challengeManager != null)
        {
            bool enemiesConditionMet = challengeManager.totalEnemies > 0 && challengeManager.enemyKilledCount >= requiredEnemyKills;
            bool pickupsConditionMet = challengeManager.requirePickups && challengeManager.totalPickups > 0 && challengeManager.totalPickups == challengeManager.pickupCount;

            if (enemiesConditionMet || pickupsConditionMet)
            {
                if (spriteRenderer.sprite != openDoorSprite)
                {
                    spriteRenderer.sprite = openDoorSprite;
                }
            }
            else
            {
                if (spriteRenderer.sprite != closedDoorSprite)
                {
                    spriteRenderer.sprite = closedDoorSprite;
                }
            }
        }
    }

    private void ShowMessage(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = message;
            interactionText.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay());
        }
    }

    private void HideMessage()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        HideMessage();
    }
}