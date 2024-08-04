using UnityEngine;
using UnityEngine.UI;
using Cosmicrafts.Managers;
using Candid;
using System.Threading.Tasks;
using EdjCase.ICP.Candid.Models;
using System.Numerics;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField] private GameObject avatarButtonPrefab;
    [SerializeField] private Transform scrollContent;

    private bool isUpdatingAvatar = false; // Flag to track if an update is in progress

    private void Start()
    {
        PopulateAvatarButtons();
    }

    private void PopulateAvatarButtons()
    {
        Object[] avatarObjects = Resources.LoadAll("Avatars", typeof(Sprite));

        for (int i = 0; i < avatarObjects.Length; i++)
        {
            Sprite avatarSprite = (Sprite)avatarObjects[i];
            CreateAvatarButton(avatarSprite, i + 1);
        }
    }

    private void CreateAvatarButton(Sprite sprite, int avatarId)
    {
        GameObject buttonObj = Instantiate(avatarButtonPrefab, scrollContent);
        buttonObj.SetActive(true);

        Transform imageTransform = buttonObj.transform.Find("AvatarImage");

        if (imageTransform != null)
        {
            Image avatarImage = imageTransform.GetComponent<Image>();
            if (avatarImage != null)
            {
                avatarImage.sprite = sprite;
            }
            else
            {
                Debug.LogWarning("Avatar Image component not found on the child object.");
            }
        }
        else
        {
            Debug.LogWarning("Child object with the specific name for Avatar Image not found.");
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => SelectAvatar(avatarId));
        }
    }

    private void SelectAvatar(int id)
    {
        // Update the local player data
        GameDataManager.Instance.playerData.AvatarID = id;
        GameDataManager.Instance.SavePlayerData();

        // Optionally, you can also trigger an event or a method to refresh the UI
        Debug.Log($"Avatar selected: {id}");
    }

    public async Task<bool> UpdateAvatarOnBlockchain(int newAvatarId)
    {
        if (isUpdatingAvatar) return false; // Prevent multiple updates at the same time

        isUpdatingAvatar = true;

        try
        {
            var response = await CandidApiManager.Instance.CanisterLogin.UpdateAvatar(UnboundedUInt.FromBigInteger(new BigInteger(newAvatarId)));
            if (response.ReturnArg0)
            {
                GameDataManager.Instance.playerData.AvatarID = newAvatarId;
                GameDataManager.Instance.SavePlayerData();
                Debug.Log($"Avatar updated to ID: {newAvatarId}");
                return true;
            }
            else
            {
                Debug.LogError($"Failed to update avatar. Reason: {response.ReturnArg1}");
                return false;
            }
        }
        finally
        {
            isUpdatingAvatar = false;
        }
    }

    // Public method to be called by UI button
    public void OnUpdateAvatarButtonClicked()
    {
        if (!isUpdatingAvatar)
        {
            int avatarId = GameDataManager.Instance.playerData.AvatarID;
            _ = UpdateAvatarOnBlockchain(avatarId);
        }
        else
        {
            Debug.LogWarning("Avatar update is already in progress.");
        }
    }
}
