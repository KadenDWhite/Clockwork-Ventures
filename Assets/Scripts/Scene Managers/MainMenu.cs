using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public List<string> sceneNames; // List of scene names

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
        if (index >= 0 && index < sceneNames.Count)
        {
            // Load the scene using the scene name
            string sceneName = sceneNames[index];
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
        if (sceneNames.Contains(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene not found in the list.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}