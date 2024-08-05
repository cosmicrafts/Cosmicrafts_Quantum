using UnityEngine;
using Candid;
using Cosmicrafts.Managers;
using System.Collections;
namespace TowerRush
{
    public class Logout : MonoBehaviour
    {
        public GameObject dashboard; // Assign this in the inspector
        public GameObject login; // Assign this in the inspector

        public void CallLogout()
        {
            StartCoroutine(LogoutAndRestart());
        }

        private IEnumerator LogoutAndRestart()
        {
            // Show the loading scene
            Game.Instance.StartFinishAndRestartCoroutine();

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

            // Wait for the loading scene to be shown and the logout process to complete
            yield return new WaitForSeconds(2);

            // Restart the game (with the loading scene still active)
            Game.Instance.Restart();

            // Disable the dashboard and enable the login object
            if (dashboard != null && login != null)
            {
                dashboard.SetActive(false);
                login.SetActive(true);
            }
            else
            {
                Debug.LogError("Dashboard or Login object is not assigned!");
            }

            // Hide the loading scene after restarting
            yield return Game.Instance.HideLoadingScene_Coroutine();
        }
    }
}