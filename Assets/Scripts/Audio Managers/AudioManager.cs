using System.Collections;
using UnityEngine;
using UnityEngine.Audio;  // For AudioMixer support
using UnityEngine.SceneManagement;  // For scene management

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip portalIn;
    public AudioClip portalOut;

    [Header("---------- Audio Mixer ----------")]
    [SerializeField] private AudioMixer audioMixer;  // Reference to the AudioMixer
    [SerializeField] private string musicVolumeParameter = "MusicVolume"; // The exposed parameter in the AudioMixer for music
    [SerializeField] private string sfxVolumeParameter = "SFXVolume"; // The exposed parameter in the AudioMixer for SFX

    private bool isPaused = false;

    // Define the volume levels (low, medium, max)
    private float lowVolume = -80f;   // Low volume (mute)
    private float mediumVolume = -20f; // Medium volume (half volume)
    private float maxVolume = 0f;     // Max volume (full volume)

    // Store the current volume state
    private enum VolumeState { Low, Medium, Max }
    private VolumeState currentVolumeState = VolumeState.Max; // Start with max volume

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of AudioManager persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
        SetVolume(); // Set the initial volume based on the starting state (max)

        // Ensure the audio manager is aware of the player's death
        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP != null)
        {
            playerHP.OnPlayerDeath += HandlePlayerDeath;
        }

        // Check the current scene to pause music if it's the Main Menu
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            PauseMusic();
        }
    }

    private void Update()
    {
        // Detect V key press for volume cycle functionality
        if (Input.GetKeyDown(KeyCode.V))
        {
            CycleVolume();
        }

        // Detect Escape key press for pause/unpause functionality
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMusicPause();
        }
    }

    private void CycleVolume()
    {
        // Cycle through volume levels when V is pressed
        switch (currentVolumeState)
        {
            case VolumeState.Max:
                currentVolumeState = VolumeState.Medium;
                break;
            case VolumeState.Medium:
                currentVolumeState = VolumeState.Low;
                break;
            case VolumeState.Low:
                currentVolumeState = VolumeState.Max;
                break;
        }

        SetVolume(); // Apply the new volume setting
    }

    private void SetVolume()
    {
        // Set the audio volume based on the current state (low, medium, or max)
        float volumeToSet = 0f;
        switch (currentVolumeState)
        {
            case VolumeState.Low:
                volumeToSet = lowVolume;
                break;
            case VolumeState.Medium:
                volumeToSet = mediumVolume;
                break;
            case VolumeState.Max:
                volumeToSet = maxVolume;
                break;
        }

        // Set the volume for music and SFX using the AudioMixer
        audioMixer.SetFloat(musicVolumeParameter, volumeToSet);
        audioMixer.SetFloat(sfxVolumeParameter, volumeToSet);
    }

    private void ToggleMusicPause()
    {
        if (isPaused)
        {
            musicSource.UnPause();
            isPaused = false;
        }
        else
        {
            musicSource.Pause();
            isPaused = true;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Pause music when on Main Menu scene
    public void PauseMusic()
    {
        musicSource.Pause();
        isPaused = true;
    }

    // Resume music when leaving Main Menu scene
    public void ResumeMusic()
    {
        musicSource.UnPause();
        isPaused = false;
    }

    // Fading out music when the player dies
    public void FadeOutMusic(float fadeDuration)
    {
        StartCoroutine(FadeOutCoroutine(fadeDuration));
    }

    private IEnumerator FadeOutCoroutine(float fadeDuration)
    {
        float startVolume = musicSource.volume;

        // Fade out the music volume over the given duration
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Reset to the original volume after stopping
    }

    public void SetMusicVolume(float volume)
    {
        // Set the volume for the music using the AudioMixer
        audioMixer.SetFloat(musicVolumeParameter, volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Set the volume for SFX using the AudioMixer
        audioMixer.SetFloat(sfxVolumeParameter, volume);
    }

    public void SetMasterVolume(float volume)
    {
        // Set the master volume (if you have a master volume parameter)
        audioMixer.SetFloat("MasterVolume", volume);
    }

    // Event handler to stop the background music when the player dies
    private void HandlePlayerDeath()
    {
        // Stop the background music when the player dies
        if (musicSource != null)
        {
            FadeOutMusic(1f);  // Optionally, you can adjust the fade duration here
        }
    }

    // Listen to scene changes and pause/resume music accordingly
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Pause music if the new scene is "Main Menu", otherwise resume it
        if (scene.name == "Main Menu" || scene.name == "Survey")
        {
            PauseMusic();
        }
        else
        {
            ResumeMusic();
        }
    }
}