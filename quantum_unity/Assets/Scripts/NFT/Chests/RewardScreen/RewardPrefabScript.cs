using UnityEngine;
using TMPro;

public class RewardPrefabScript : MonoBehaviour
{
    public TMP_Text amountText; // Display amount
    public string tokenType; // Type of token, e.g., "Shards" or "Flux"
    public System.Action<string, GameObject> OnRewardClicked; // Action to invoke on click
    public NotificationManager notificationManager;

    public void SetAmount(int amount)
    {
        if (amountText != null)
            amountText.text = amount.ToString();
    }

    // Setup callback from RewardScreenUI
    public void Initialize(System.Action<string, GameObject> rewardClickedCallback)
    {
        OnRewardClicked = rewardClickedCallback;
    }

    // Method called via onClick from each prefab's button
    public void RewardClicked()
    {
        OnRewardClicked?.Invoke(tokenType, gameObject);

        if (notificationManager == null)
        {
            Debug.Log("Notification Manager is null");
            return;
        }

        string notificationText = $"Received {amountText.text} {tokenType}";
        Debug.Log($"Showing notification: {notificationText}");
        notificationManager.ShowNotification(notificationText);
    }
}
