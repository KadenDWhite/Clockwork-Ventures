using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect2 : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 50f; // Speed of the typewriter effect
    public bool IsRunning { get; private set; } // Indicates if the typewriter effect is currently running

    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>() { '.', '!', '?' }, 0.6f), // Punctuation with longer wait time
        new Punctuation(new HashSet<char>() { ',', ';', ':' }, 0.3f)  // Punctuation with shorter wait time
    };

    private Coroutine typingCoroutine; // Coroutine reference for controlling typing

    // Reference to the TMP_Text object
    [SerializeField] private TMP_Text textLabel;

    // Public method to start the typewriter effect
    public void Run()
    {
        if (IsRunning)
        {
            Stop(); // Stop any existing typing coroutine before starting a new one
        }

        // Use the text from the TMP_Text component
        string textToType = textLabel.text;
        typingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    // Public method to stop the typewriter effect
    public void Stop()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop the coroutine if it exists
            typingCoroutine = null; // Reset coroutine reference
        }
        IsRunning = false;
    }

    // Coroutine that handles the typewriting effect
    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        IsRunning = true;
        textLabel.text = string.Empty; // Clear the text label

        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;

            t += Time.deltaTime * typewriterSpeed; // Update the timer

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for (int i = lastCharIndex; i < charIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;

                textLabel.text = textToType.Substring(0, i + 1); // Update displayed text

                // Check if the current character is punctuation
                if (IsPunctuation(textToType[i], out float waitTime) && !isLast &&
                    !IsPunctuation(textToType[i + 1], out waitTime))
                {
                    yield return new WaitForSeconds(waitTime); // Wait for the specified time if punctuation
                }
            }

            yield return null; // Wait until the next frame
        }

        IsRunning = false; // Mark as not running when done
    }

    // Method to check if a character is punctuation and get the corresponding wait time
    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach (Punctuation punctuationCategory in punctuations)
        {
            if (punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default; // No wait time for non-punctuation
        return false;
    }

    // Struct to represent punctuation characters and their wait times
    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations; // Set of punctuation characters
        public readonly float WaitTime; // Wait time for these punctuation characters

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}