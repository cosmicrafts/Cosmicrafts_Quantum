using UnityEngine;
using TMPro;

public class ActivateInputField : MonoBehaviour
{
    public TMP_InputField inputField;

    private void Start()
    {
        // Check if the input field is not null
        if (inputField != null)
        {
            // Activate the input field
            inputField.ActivateInputField();
        }
        else
        {
            Debug.LogWarning("Input field is not assigned in the script.");
        }
    }
}
