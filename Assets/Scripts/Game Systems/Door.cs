using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Door : MonoBehaviour
{
    [Tooltip("The required item to open the door. Leave empty if no item is required.")]
    [SerializeField] private string requiredItem = ""; // The required item to open the door (empty for no item requirement)

    [Tooltip("The closed door sprite.")]
    [SerializeField] private Sprite closedDoorSprite; // The closed door sprite

    [Tooltip("The open door sprite.")]
    [SerializeField] private Sprite openDoorSprite;   // The open door sprite

    [Tooltip("The scene to load when the door is opened.")]
    [SerializeField] private string sceneToLoad;      // The scene to load when the door is opened

    [Tooltip("Reference to the UI text to show interaction message.")]
    [SerializeField] private TextMeshProUGUI interactionText; // Reference to the UI text to show interaction message

    [Tooltip("Duration to display the interaction message.")]
    [SerializeField] private float messageDuration = 2f; // Duration to display the interaction message

    private bool playerInTrigger = false; 
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        UpdateDoorSprite();
    }

    void Update()
    {
        // Check if the player is in range and presses 'E' to interact with the door
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            TryToOpenDoor(); 
        }

        UpdateDoorSprite();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Trigger when the player enters the door's trigger zone
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            ShowMessage("Press E to interact.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Trigger when the player exits the door's trigger zone
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            HideMessage(); 
        }
    }

    private void TryToOpenDoor()
    {
        if (!string.IsNullOrEmpty(requiredItem))
        {
            // Check if the player has the required item (adapted to use the existing key)
            if (GameManager.Instance.GetKeyState())
            {
                GameManager.Instance.SetKeyState(false); // Consume the key
                ShowMessage($"{requiredItem} used to open the door!"); // Display message confirming item use
                LoadScene(); // Load the scene upon key use
            }
            else
            {
                ShowMessage($"You need a {requiredItem} to open this door.");
            }
        }
        else if (GameManager.Instance.GetKeyState()) // If no item is required, use the key directly
        {
            GameManager.Instance.SetKeyState(false); // Consume the key
            ShowMessage("Key used to open the door!"); // Display key used message
            LoadScene(); // Load the scene upon key use
        }
        else
        {
            ShowMessage("You need a key to open this door.");
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            GameManager.Instance.SaveGameState();
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void UpdateDoorSprite()
    {
        if (GameManager.Instance.GetKeyState())
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