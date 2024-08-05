using UnityEngine;
using UnityEngine.SceneManagement;
using Candid;
using Cosmicrafts.Managers;

public class Logout : MonoBehaviour
{
    public GameObject dashboard; // Assign this in the inspector
    public GameObject login; // Assign this in the inspector

    public void CallLogout()
    {
        // Find the CandidApiManager instance in the scene
        CandidApiManager candidApiManager = FindObjectOfType<CandidApiManager>();

        // Check if the CandidApiManager instance exists
        if (candidApiManager != null)
        {
            // Call the logout method of the CandidApiManager
            candidApiManager.LogOut();
            
            // Clear the API if there is a specific method to do so
            // Example: candidApiManager.ClearAPI();
        }
        else
        {
            Debug.LogError("CandidApiManager instance not found in the scene!");
        }

        // Set IsLoggedIn to false
        if (GameDataManager.Instance != null && GameDataManager.Instance.playerData != null)
        {
            GameDataManager.Instance.playerData.IsLoggedIn = false;
            GameDataManager.Instance.SavePlayerData();
        }
        else
        {
            Debug.LogError("GameDataManager or PlayerData instance not found!");
        }

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
