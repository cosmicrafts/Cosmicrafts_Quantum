using UnityEngine;
using TMPro; // Include the TextMeshPro namespace

public class NumberFormatter : MonoBehaviour
{
    private TMP_Text numberText; // No need to assign in the Inspector

    private void Awake()
    {
        // Automatically get the TMP_Text component on the same GameObject
        numberText = GetComponent<TMP_Text>();
        if (numberText == null)
        {
            Debug.LogError("No TMP_Text component found on the GameObject.");
        }
    }

    public void SetNumber(float number)
    {
        // Format the number with thousands separators
        if (numberText != null)
        {
            numberText.text = string.Format("{0:n0}", number);
        }
    }
}
