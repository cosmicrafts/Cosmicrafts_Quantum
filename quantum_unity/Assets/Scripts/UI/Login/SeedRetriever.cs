using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using Cosmicrafts.Data;

public class SeedRetriever : MonoBehaviour
{
    public TMP_InputField seedPhraseInputField; // Assign in the inspector
    public Button copyButton; // Assign in the inspector
    public NotificationManager notificationManager;

    private string currentSeedPhrase = "";

    async void Start()
    {
        // Attach button listener
        copyButton.onClick.AddListener(CopySeedPhraseToClipboard);

        // Load and display the saved seed phrase
        await LoadAndDisplaySeedPhraseAsync();
    }

    private async Task LoadAndDisplaySeedPhraseAsync()
    {
        // Load the saved seed phrase
        currentSeedPhrase = await SeedRecovery.LoadSeedPhraseAsync();
        
        if (!string.IsNullOrEmpty(currentSeedPhrase))
        {
            // Display the seed phrase
            Debug.Log($"Loaded Seed Phrase: {currentSeedPhrase}");
            seedPhraseInputField.text = currentSeedPhrase;
        }
        else
        {
            // Handle case where no seed phrase is found
            Debug.LogWarning("No saved seed phrase found.");
            notificationManager.ShowNotification("No saved seed phrase found.");
        }
    }

    private void CopySeedPhraseToClipboard()
    {
        if (!string.IsNullOrEmpty(currentSeedPhrase))
        {
            // Copy the current seed phrase to the clipboard
            GUIUtility.systemCopyBuffer = currentSeedPhrase;
            Debug.Log("Seed Phrase copied to clipboard.");
            
            // Show a notification to the user
            notificationManager.ShowNotification("Seed Phrase copied to clipboard.");
        }
        else
        {
            Debug.LogWarning("No seed phrase to copy.");
            notificationManager.ShowNotification("No seed phrase to copy.");
        }
    }
}
