using UnityEngine;
using Candid;

public class Logout : MonoBehaviour
{
    public void CallLogout()
    {
        // Find the CandidApiManager instance in the scene
        CandidApiManager candidApiManager = FindObjectOfType<CandidApiManager>();

        // Check if the CandidApiManager instance exists
        if (candidApiManager != null)
        {
            // Call the logout method of the CandidApiManager
            candidApiManager.LogOut();
        }
        else
        {
            Debug.LogError("CandidApiManager instance not found in the scene!");
        }
    }
}
