using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor; // Allows using SceneAsset in the Editor

public class MainMenu : MonoBehaviour
{
    public List<SceneAsset> scenesToLoad; // Drag your scenes here in the Inspector

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // Load a scene based on its index in the list
    public void LoadLevelByIndex(int index)
    {
        if (index >= 0 && index < scenesToLoad.Count)
        {
            // Load the scene using the name of the SceneAsset
            string sceneName = scenesToLoad[index].name;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Invalid scene index.");
        }
    }

    // Load a scene based on its name
    public void LoadLevelByName(string sceneName)
    {
        foreach (SceneAsset scene in scenesToLoad)
        {
            if (scene.name == sceneName)
            {
                SceneManager.LoadScene(scene.name);
                return;
            }
        }
        Debug.LogWarning("Scene not found in the list.");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}