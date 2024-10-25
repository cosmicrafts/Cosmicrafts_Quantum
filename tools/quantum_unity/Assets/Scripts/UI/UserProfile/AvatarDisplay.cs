using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Cosmicrafts.Managers;


public class AvatarDisplay : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    [SerializeField] private string avatarsFolderPath = "Avatars";

    private void Awake()
    {
        UpdateAvatarImage(GameDataManager.Instance.playerData.AvatarID);
        GameDataManager.Instance.playerData.OnAvatarIdChanged += UpdateAvatarImage;
    }

    private void OnDestroy()
    {
        GameDataManager.Instance.playerData.OnAvatarIdChanged -= UpdateAvatarImage;
    }

    private void UpdateAvatarImage(int avatarId)
    {
        string formattedId = avatarId < 10 ? $"0{avatarId}" : avatarId.ToString();
        string imagePath = Path.Combine(avatarsFolderPath, $"Avatar_{formattedId}");
        Sprite avatarSprite = Resources.Load<Sprite>(imagePath);

        if (avatarSprite != null)
        {
            avatarImage.sprite = avatarSprite;
            // Debug.Log($"Displaying avatar ID {avatarId} with sprite {avatarSprite.name}");
        }
        else
        {
            Debug.LogError($"Avatar image not found for ID {avatarId}, looked for {imagePath}");
        }
    }
}
