using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationPanel;
    public TMP_Text notificationText;
    public float displayDuration = 3f;
    private SimpleDeactivate simpleDeactivate;

    private void Awake()
    {
        simpleDeactivate = notificationPanel.GetComponent<SimpleDeactivate>();
        if (simpleDeactivate == null)
        {
            Debug.LogWarning("SimpleDeactivate component not found on notificationPanel GameObject.");
        }
        // Ensure the panel is initially inactive without causing a visible flicker
        notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);
        CancelInvoke("HideNotification"); // Cancel any previous hide invocation
        Invoke("HideNotification", displayDuration);
    }

    void HideNotification()
    {
        if (simpleDeactivate != null)
        {
            simpleDeactivate.StartDeactivation();
        }
        else
        {
            notificationPanel.SetActive(false);
        }
    }
}
