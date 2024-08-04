using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Candid;
using System.Threading.Tasks;

public class SeedRecovery : MonoBehaviour
{
    public TMP_InputField seedPhraseInputField;
    public Button recoverButton;
    public CandidApiManager candidApiManager;
    public NotificationManager notificationManager;

    private const string SeedPhraseKey = "SeedPhrase";

    private void Start()
    {
        recoverButton.onClick.AddListener(RecoverSeedPhrase);
    }

    private async void RecoverSeedPhrase()
    {
        string seedPhrase = seedPhraseInputField.text;

        if (string.IsNullOrWhiteSpace(seedPhrase))
        {
            Debug.LogWarning("Seed phrase is empty.");
            notificationManager.ShowNotification("Please enter a seed phrase.");
            return;
        }

        candidApiManager.SetTestSeedPhrase(seedPhrase);
        Debug.Log("Seed phrase recovered and set in CandidApiManager.");

        // Save the seed phrase using AsyncLocalStorage
        await SaveSeedPhraseAsync(seedPhrase);
        Debug.Log("Seed phrase saved using AsyncLocalStorage.");

        notificationManager.ShowNotification("Wallet recovered successfully.");

        // Call StartLoginRandom after successfully saving the seed phrase
        LoginManager.Instance.StartLoginRandom();
    }

    private async Task SaveSeedPhraseAsync(string seedPhrase)
    {
        await AsyncLocalStorage.SaveDataAsync(SeedPhraseKey, seedPhrase);
    }

    public static async Task<string> LoadSeedPhraseAsync()
    {
        return await AsyncLocalStorage.LoadDataAsync(SeedPhraseKey);
    }
}
