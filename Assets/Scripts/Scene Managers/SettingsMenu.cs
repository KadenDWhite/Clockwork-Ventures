using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public GameObject confirmationPanel; // The confirmation dialog panel
    public Button confirmButton;
    public Button cancelButton;

    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;
    private int pendingResolutionIndex = 0;
    private bool awaitingConfirmation = false;
    private Coroutine confirmationRoutine;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Hide confirmation panel initially
        confirmationPanel.SetActive(false);
    }

    public void SetResolution(int resolutionIndex)
    {
        pendingResolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];

        // Set the resolution temporarily
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Show confirmation panel
        confirmationPanel.SetActive(true);

        // Start the timer to confirm or revert the resolution
        awaitingConfirmation = true;
        if (confirmationRoutine != null)
        {
            StopCoroutine(confirmationRoutine);
        }
        confirmationRoutine = StartCoroutine(ConfirmationCountdown(10)); // 10 seconds to confirm
    }

    IEnumerator ConfirmationCountdown(float time)
    {
        yield return new WaitForSeconds(time);
        if (awaitingConfirmation)
        {
            CancelResolutionChange(); // If time runs out, cancel the change
        }
    }

    public void ConfirmResolutionChange()
    {
        currentResolutionIndex = pendingResolutionIndex; // Save the new resolution
        awaitingConfirmation = false;
        confirmationPanel.SetActive(false);
        if (confirmationRoutine != null)
        {
            StopCoroutine(confirmationRoutine);
        }
    }

    public void CancelResolutionChange()
    {
        // Revert to the previous resolution
        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        awaitingConfirmation = false;
        confirmationPanel.SetActive(false);
        if (confirmationRoutine != null)
        {
            StopCoroutine(confirmationRoutine);
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}