using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AvatarSelection : MonoBehaviour
{
    [SerializeField] private GameObject avatarButtonPrefab;
    [SerializeField] private Transform scrollContent;

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

        Image buttonImage = buttonObj.GetComponent<Image>();
        Button button = buttonObj.GetComponent<Button>();

        if (buttonImage != null) buttonImage.sprite = sprite;

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
