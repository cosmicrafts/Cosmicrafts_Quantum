using UnityEngine;
using NBitcoin;
using Candid;

public class GuestLogin : MonoBehaviour
{
    private string currentSeedPhrase = "";
    public CandidApiManager candidApiManager;
    public NotificationManager notificationManager;

    void Start()
    {
        // The logic is now triggered by a button, so we don't need to do anything in Start.
    }

    public void StartGuestLoginProcess()
    {
        // Generate and handle the seed phrase
        GenerateAndHandleSeedPhrase();
    }

    void GenerateAndHandleSeedPhrase()
    {
        // Generate a new mnemonic (seed phrase)
        Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
        currentSeedPhrase = mnemonic.ToString();

        // Display the seed phrase
        Debug.Log($"Seed Phrase: {currentSeedPhrase}");

        // Save the seed phrase
        SaveSeedPhrase(currentSeedPhrase);

        // Set the seed phrase in CandidApiManager
        candidApiManager.SetTestSeedPhrase(currentSeedPhrase);
        Debug.Log("Seed phrase set in CandidApiManager.");

        // Notify the user
        notificationManager.ShowNotification("Guest Login initialized");

        // Initiate the random login process
        LoginManager.Instance.StartLoginRandom();
    }

    void SaveSeedPhrase(string seedPhrase)
    {
        GlobalGameData.Instance.SaveSeedPhrase(seedPhrase);
        PlayerPrefs.SetString("SeedPhrase", seedPhrase);
        PlayerPrefs.Save();
        Debug.Log("Seed phrase saved to GlobalGameData and PlayerPrefs.");
    }
}
