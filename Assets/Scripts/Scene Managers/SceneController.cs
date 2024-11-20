using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
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
        // If we're entering the Main Menu, pause the music
        if (scene.name == "Main Menu")
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PauseMusic();  // Pause music on Main Menu
            }
        }

        // If we're entering the Survey, stop the music
        if (scene.name == "Survey")
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PauseMusic();  // Pause music on Survey
            }
        }

        // If the scene is "Main Menu 2", "MM (Easy)", or "MM (Normal)", unpause or resume the music
        else if (scene.name == "Main Menu 2" || scene.name == "MM (Easy)" || scene.name == "MM (Normal)")
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.ResumeMusic();  // Unpause music when entering these scenes
            }
        }
        else
        {
            // For other scenes, stop the music
            if (AudioManager.instance != null)
            {
                AudioManager.instance.StopMusic();  // Stop music for other scenes
            }
        }

        // Define allowed scenes where AudioManager and SceneController should not be deleted
        string[] allowedScenes = new string[] { "Main Menu 2", "MM (Easy)", "MM (Normal)" };

        // If it's any other scene, stop the music and delete AudioManager and SceneController
        if (!Array.Exists(allowedScenes, s => s == scene.name))
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.StopMusic();  // Stop music
                Destroy(AudioManager.instance.gameObject);  // Delete AudioManager
            }

            // Destroy SceneController if it's not an allowed scene
            if (instance != null)
            {
                Destroy(instance.gameObject);  // Delete SceneController
            }
        }
    }
}