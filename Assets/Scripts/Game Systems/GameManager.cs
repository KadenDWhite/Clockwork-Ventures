using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TypewriterEffect2 typewriterEffect;
    [SerializeField] private TypewriterEffect2 typewriterEffect2;
    [SerializeField] private GameObject deathScreen;

    public static GameManager Instance;

    private Dictionary<string, bool> portalStates = new Dictionary<string, bool>();

    // Store the key's collection state (this could be expanded to more items if needed)
    private bool hasKey = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate GameManagers
        }
    }

    private void Start()
    {
        deathScreen.SetActive(false);

        // Load saved data (key state and portal states)
        LoadGameState();
    }

    public void OnPlayerDeath()
    {
        deathScreen.SetActive(true);
        typewriterEffect.Run();
        typewriterEffect2.Run();
    }

    // Set the state of a portal
    public void SetPortalState(string portalKey, bool isActive)
    {
        if (portalStates.ContainsKey(portalKey))
        {
            portalStates[portalKey] = isActive;
        }
        else
        {
            portalStates.Add(portalKey, isActive);
        }
    }

    // Get the state of a portal
    public bool GetPortalState(string portalKey)
    {
        return portalStates.ContainsKey(portalKey) && portalStates[portalKey];
    }

    // Set and get the key state
    public void SetKeyState(bool hasTheKey)
    {
        hasKey = hasTheKey;
        SaveGameState(); // Save the key state when it's changed
    }

    public bool GetKeyState()
    {
        return hasKey;
    }

    // Save game state (portal and key states)
    public void SaveGameState()
    {
        // Saving portal states
        foreach (var portalState in portalStates)
        {
            PlayerPrefs.SetInt(portalState.Key, portalState.Value ? 1 : 0);
        }

        // Save key state
        PlayerPrefs.SetInt("HasKey", hasKey ? 1 : 0);
    }

    // Load game state (portal and key states)
    public void LoadGameState()
    {
        // Load portal states
        foreach (var portalKey in portalStates.Keys)
        {
            int state = PlayerPrefs.GetInt(portalKey, 0);
            portalStates[portalKey] = state == 1;
        }

        // Load key state
        hasKey = PlayerPrefs.GetInt("HasKey", 0) == 1;
    }
}