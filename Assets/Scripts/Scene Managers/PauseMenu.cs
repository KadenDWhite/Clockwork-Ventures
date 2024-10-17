using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public PlayerMovement playerMovement;
    public PlayerAttack playerAttack;
    public WeaponManager weaponManager;

    // Add an AudioSource reference for the background music
    public AudioSource backgroundMusicSource;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Resume the audio source if it's not null
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.UnPause();  // Resume the music from where it left off
        }

        // Enable player movement, attack, and weapon manager scripts
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerAttack != null) playerAttack.enabled = true;
        if (weaponManager != null) weaponManager.enabled = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;

        // Pause the audio source if it's not null
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Pause();  // Pause the music but don't stop it entirely
        }

        // Disable player movement, attack, and weapon manager scripts
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerAttack != null) playerAttack.enabled = false;
        if (weaponManager != null) weaponManager.enabled = false;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}