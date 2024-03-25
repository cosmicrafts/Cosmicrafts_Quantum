using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField] private GameObject avatarButtonPrefab;
    [SerializeField] private Transform scrollContent;
    [SerializeField] private Image avatarImagePrefab;

    private void Start()
    {
        PopulateAvatarButtons();
    }

    private void PopulateAvatarButtons()
    {
        Object[] avatarObjects = Resources.LoadAll("Avatars", typeof(Sprite));
        Debug.Log($"Loaded {avatarObjects.Length} avatars.");

        for (int i = 0; i < avatarObjects.Length; i++)
        {
            Sprite avatarSprite = (Sprite)avatarObjects[i];
            Debug.Log($"Avatar {i + 1}: {avatarSprite.name}");
            CreateAvatarButton(avatarSprite, i + 1);
        }
    }

    private void CreateAvatarButton(Sprite sprite, int avatarId)
    {
        GameObject buttonObj = Instantiate(avatarButtonPrefab, scrollContent);
        buttonObj.SetActive(true);

        if (avatarImagePrefab != null)
        {
            avatarImagePrefab.sprite = sprite;
            Debug.Log($"Assigning sprite {sprite.name} to button with Avatar ID: {avatarId}");
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => SelectAvatar(avatarId));
        }
    }

    private void SelectAvatar(int id)
    {
        GlobalGameData.Instance.SetAvatarId(id);
        Debug.Log($"Avatar selected: {id}");
    }
}
