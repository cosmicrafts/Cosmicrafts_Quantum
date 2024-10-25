using UnityEngine;
using Cosmicrafts.Managers;

public class CopyToClipboard : MonoBehaviour
{
    public NotificationManager notificationManager;

    public void CopyText()
    {
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("[CopyToClipboard] GameDataManager instance is null.");
            return;
        }

        var playerData = GameDataManager.Instance.playerData;
        if (playerData == null)
        {
            Debug.LogError("Failed to load player data.");
            return;
        }

        string fullPrincipalId = playerData.PrincipalId;

        if (!string.IsNullOrEmpty(fullPrincipalId))
        {
            GUIUtility.systemCopyBuffer = fullPrincipalId; // Copy the PrincipalId to clipboard
            Debug.Log("Principal ID copied to clipboard: " + fullPrincipalId);

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
        else
        {
            Debug.LogError("Principal ID is null or empty.");
        }
    }
}
