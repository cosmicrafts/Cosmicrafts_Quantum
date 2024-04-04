using UnityEngine;
using TMPro;

public class CopyToClipboard : MonoBehaviour
{
    public TMP_Text textToCopy;
    public NotificationManager notificationManager;

    public void CopyText()
    {
        Debug.Log("Copying text: " + textToCopy.text);
        GUIUtility.systemCopyBuffer = textToCopy.text;

        // Show notification
        if(notificationManager != null)
        {
            notificationManager.ShowNotification("Principal copied to clipboard!");
        }
        else
        {
            Debug.LogWarning("NotificationManager reference not set in CopyToClipboard script.");
        }
    }
}
