using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AvatarDisplay : MonoBehaviour
{
    [SerializeField] private Image avatarImage; // Make sure to assign this in the inspector
    [SerializeField] private string avatarsFolderPath = "Avatars"; // Path inside the Resources folder

    private void Awake()
    {
        UpdateAvatarImage(GlobalGameData.Instance.avatarId); 
        GlobalGameData.Instance.OnAvatarIdChanged += UpdateAvatarImage; 
    }

    private void OnDestroy()
    {
        GlobalGameData.Instance.OnAvatarIdChanged -= UpdateAvatarImage;
    }

    private void UpdateAvatarImage(int avatarId)
    {
        string imagePath = Path.Combine(avatarsFolderPath, avatarId.ToString()); // Build the resource path
        Sprite avatarSprite = Resources.Load<Sprite>(imagePath);

        if (avatarSprite != null)
        {
            avatarImage.sprite = avatarSprite;
        }
        else
        {
            Debug.LogError($"Avatar image not found for ID {avatarId}");
        }
    }
}
