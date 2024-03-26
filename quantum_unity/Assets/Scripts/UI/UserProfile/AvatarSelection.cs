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
        GlobalGameData.Instance.SetAvatarId(id);
        Debug.Log($"Avatar selected: {id}");
    }
}
