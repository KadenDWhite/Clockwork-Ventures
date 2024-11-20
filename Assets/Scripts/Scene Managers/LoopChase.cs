using System.Collections;
using UnityEngine;

public class LoopChase : MonoBehaviour
{
    public Transform player;
    public Transform goblin;

    public float screenLimitX = 10f; // Adjust this to match the width of your screen
    public float playerSpeed = 5f;
    public float goblinSpeed = 5f;
    public float chaseSwitchInterval = 3f; // Interval in seconds before chase role changes

    private bool isGoblinChasing = true;

    private AudioListener audioListener;

    public float backgroundChangeInterval = 3f; // Time interval for changing background color (in seconds)

    void Start()
    {
        StartCoroutine(SwitchChaseRole());

        // Find the AudioListener component in the scene (if there is one)
        audioListener = FindObjectOfType<AudioListener>();
        if (audioListener == null)
        {
            Debug.LogError("No AudioListener found in the scene.");
        }

        // Start periodic background color change
        StartCoroutine(ChangeBackgroundColorPeriodically());
    }

    void Update()
    {
        HandleLoopPosition(player, playerSpeed);
        HandleLoopPosition(goblin, goblinSpeed);
        HandleChaseDirection();

        // Toggle AudioListener active state when Q is pressed
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleAudioListener();
        }
    }

    void HandleLoopPosition(Transform character, float speed)
    {
        // Move character to the right
        character.Translate(Vector2.right * speed * Time.deltaTime);

        // Loop character to the left side if they go off screen to the right
        if (character.position.x > screenLimitX)
        {
            character.position = new Vector2(-screenLimitX, character.position.y);
        }
    }

    void HandleChaseDirection()
    {
        if (isGoblinChasing)
        {
            goblinSpeed = playerSpeed + 1f; // Goblin slightly faster when chasing
        }
        else
        {
            playerSpeed = goblinSpeed + 1f; // Player slightly faster when being chased
        }
    }

    IEnumerator SwitchChaseRole()
    {
        while (true)
        {
            yield return new WaitForSeconds(chaseSwitchInterval);
            isGoblinChasing = !isGoblinChasing;

            // Randomize interval for next chase switch for unpredictability
            chaseSwitchInterval = Random.Range(2f, 5f);
        }
    }

    void ToggleAudioListener()
    {
        // If an AudioListener is found
        if (audioListener != null)
        {
            // If there's already an active AudioListener, disable it
            AudioListener[] listeners = FindObjectsOfType<AudioListener>();
            foreach (AudioListener listener in listeners)
            {
                if (listener != audioListener) // Don't disable the current one
                {
                    listener.enabled = false;
                }
            }

            // Toggle the current AudioListener's state
            audioListener.enabled = !audioListener.enabled;
        }
    }

    IEnumerator ChangeBackgroundColorPeriodically()
    {
        while (true)
        {
            // Generate random RGB values (values between 0 and 1 for each color channel)
            float r = Random.value;
            float g = Random.value;
            float b = Random.value;

            // Set the camera's background color to the generated random color
            Camera.main.backgroundColor = new Color(r, g, b);

            // Wait for the specified interval before changing the color again
            yield return new WaitForSeconds(backgroundChangeInterval);
        }
    }
}