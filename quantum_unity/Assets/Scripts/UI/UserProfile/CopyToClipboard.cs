using UnityEngine;
using TMPro;

public class CopyToClipboard : MonoBehaviour
{
    public NotificationManager notificationManager;

    public void CopyText()
    {
        // Directly fetch the full wallet ID from GlobalGameData or similar
        string fullWalletId = GlobalGameData.Instance.GetUserData().WalletId;
        
        GUIUtility.systemCopyBuffer = fullWalletId; // Copy the full Wallet ID to clipboard
        Debug.Log("Full Wallet ID copied to clipboard: " + fullWalletId);

        // Show notification
        if (notificationManager != null)
        {
            notificationManager.ShowNotification("Principal ID copied to clipboard!");
        }
        else
        {
            Debug.LogWarning("NotificationManager reference not set in CopyToClipboard script.");
        }
    }
}
