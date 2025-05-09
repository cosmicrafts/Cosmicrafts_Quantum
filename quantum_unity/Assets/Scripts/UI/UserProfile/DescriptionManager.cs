using UnityEngine;
using TMPro;
using Cosmicrafts.Managers;
using System.Threading.Tasks;
using Candid;

public class DescriptionManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField descriptionInputField; // Reference to the input field for description
    private const string defaultPlaceholderText = "Write down something about yourself.";
    private string initialDescription;
    private bool descriptionChanged = false; // Flag to track if description has been changed

    private void Start()
    {
        if (descriptionInputField == null)
        {
            Debug.LogError("[DescriptionManager] InputField not assigned.");
            return;
        }

        // Load the current description into the input field
        initialDescription = GameDataManager.Instance.playerData.Description;

        if (string.IsNullOrEmpty(initialDescription))
        {
            descriptionInputField.placeholder.GetComponent<TMP_Text>().text = defaultPlaceholderText;
        }
        else
        {
            descriptionInputField.text = initialDescription;
        }

        // Add listener to update local description
        descriptionInputField.onEndEdit.AddListener(UpdateLocalDescription);
    }

    private void UpdateLocalDescription(string newDescription)
    {
        GameDataManager.Instance.playerData.Description = newDescription;
        GameDataManager.Instance.SavePlayerData();
        descriptionChanged = true; // Set flag to true when description is changed
        Debug.Log($"[DescriptionManager] Local description updated to: {newDescription}");
    }

    // Public method to be called by UI button
    public async void OnSaveDescriptionButtonClicked()
    {
        if (descriptionChanged)
        {
            string newDescription = GameDataManager.Instance.playerData.Description;

            bool success = await UpdateDescriptionOnBlockchain(newDescription);

            if (success)
            {
                initialDescription = newDescription;
                descriptionChanged = false; // Reset the flag after successful update
                Debug.Log("[DescriptionManager] Description updated successfully on the blockchain.");
            }
        }
    }

    private async Task<bool> UpdateDescriptionOnBlockchain(string newDescription)
    {
        var response = await CandidApiManager.Instance.MainCanister.UpdateDescription(newDescription);
        if (response.ReturnArg0)
        {
            Debug.Log($"Description updated to: {newDescription} on the blockchain.");
            return true;
        }
        else
        {
            Debug.LogError($"Failed to update description. Reason: {response.ReturnArg1}");
            return false;
        }
    }
}
