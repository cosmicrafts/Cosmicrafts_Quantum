using UnityEngine;
using Cosmicrafts.Managers;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject dashboardCanvas;
    [SerializeField] private GameObject loginCanvas;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (GameDataManager.Instance != null)
        {
            bool isLoggedIn = GameDataManager.Instance.playerData.IsLoggedIn;
            dashboardCanvas.SetActive(isLoggedIn);
            loginCanvas.SetActive(!isLoggedIn);
        }
        else
        {
            Debug.LogError("[UIManager] GameDataManager instance is null in UpdateUI.");
        }
    }
}
