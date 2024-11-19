using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace for text handling

public class Backpack : MonoBehaviour
{
    private HashSet<string> items = new HashSet<string>();

    public TMPro.TextMeshProUGUI backpackStatusText;
    public GameObject backpackStatusPanel;
    public float statusDisplayDuration = 5f;

    // Singleton pattern for easy global access
    public static Backpack Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the backpack object across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates of the backpack
        }
    }

    private void Start()
    {
        LoadBackpack();

        if (backpackStatusPanel != null)
        {
            backpackStatusPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBackpackStatus();
        }
    }

    public void AddItem(string itemName)
    {
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
            Debug.Log(itemName + " added to backpack.");
            SaveBackpack();
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    private void SaveBackpack()
    {
        foreach (var item in items)
        {
            PlayerPrefs.SetInt(item, 1); // Save item in PlayerPrefs
        }
        PlayerPrefs.Save(); // Ensure data is saved
    }

    private void LoadBackpack()
    {
        if (PlayerPrefs.HasKey("Key"))
        {
            items.Add("Key");
            Debug.Log("Key loaded from backpack.");
        }
    }

    private void ToggleBackpackStatus()
    {
        if (backpackStatusPanel != null)
        {
            if (HasItem("Key"))
            {
                backpackStatusText.text = "You have the Key in your backpack!";
            }
            else
            {
                backpackStatusText.text = "You don't have the Key in your backpack.";
            }

            backpackStatusPanel.SetActive(true);
            StartCoroutine(HideBackpackStatusAfterDelay(statusDisplayDuration));
        }
    }

    private IEnumerator HideBackpackStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        backpackStatusPanel.SetActive(false);
    }
}