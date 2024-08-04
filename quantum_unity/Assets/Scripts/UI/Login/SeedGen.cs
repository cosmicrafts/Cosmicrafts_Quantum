using UnityEngine;
using NBitcoin;
using TMPro; // Ensure this is included for TMP_InputField
using UnityEngine.UI;
using Candid;

public class SeedGen : MonoBehaviour
{
    public TMP_InputField seedPhraseInputField; // Adjust to use TMP_InputField
    public Button generateButton; // Assign in the inspector
    public Button copyButton; // Assign in the inspector
    private string currentSeedPhrase = "";
    public NotificationManager notificationManager;
    public CandidApiManager candidApiManager;

    void Start()
    {
        // Attach button listeners
        generateButton.onClick.AddListener(GenerateAndDisplaySeedPhrase);
        copyButton.onClick.AddListener(CopySeedPhraseToClipboard);

        // Generate initial seed phrase
        GenerateAndDisplaySeedPhrase();
    }

    void GenerateAndDisplaySeedPhrase()
    {
        // Generate a new mnemonic (seed phrase)
        Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
        currentSeedPhrase = mnemonic.ToString();

        // Display the seed phrase
        Debug.Log($"Seed Phrase: {currentSeedPhrase}");
        seedPhraseInputField.text = currentSeedPhrase; // Use the input field for display

        // Save the seed phrase
        SaveSeedPhrase(currentSeedPhrase);

        candidApiManager.SetTestSeedPhrase(currentSeedPhrase);
    }

    void CopySeedPhraseToClipboard()
    {
        // Copy the current seed phrase to the clipboard
        GUIUtility.systemCopyBuffer = currentSeedPhrase;
        Debug.Log("Seed Phrase copied to clipboard.");
        
        // Show a notification to the user
        notificationManager.ShowNotification("Seed Phrase copied to clipboard.");
    }

    async void SaveSeedPhrase(string seedPhrase)
    {
        await AsyncLocalStorage.SaveDataAsync("SeedPhrase", seedPhrase);
    }
}
