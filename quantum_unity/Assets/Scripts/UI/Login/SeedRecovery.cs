using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Candid;

public class SeedRecovery : MonoBehaviour
{
    public TMP_InputField seedPhraseInputField;
    public Button recoverButton;
    public CandidApiManager candidApiManager;
    public NotificationManager notificationManager;

    private void Start()
    {
        recoverButton.onClick.AddListener(RecoverSeedPhrase);
    }

    private void RecoverSeedPhrase()
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

        notificationManager.ShowNotification("Seed phrase recovered successfully.");
    }
}
