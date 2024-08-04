using UnityEngine;

public class CopyToClipboard : MonoBehaviour
{
    public NotificationManager notificationManager;

    public async void CopyText()
    {
        // Fetch the PrincipalId from AsyncLocalStorage
        string fullPrincipalId = await AsyncLocalStorage.LoadDataAsync("PrincipalId");
        
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
            Debug.LogError("Failed to load Principal ID from AsyncLocalStorage.");
        }
    }
}
