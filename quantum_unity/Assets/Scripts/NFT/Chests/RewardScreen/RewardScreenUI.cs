using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class RewardScreenUI : MonoBehaviour
{
    public Image displayImage; // Make sure to assign this in the Inspector
    public Button tapTarget; // The button on the image for fake tap

    public UnityEvent onActivateRewardScreen = new UnityEvent();

    private void Awake()
    {
        onActivateRewardScreen.AddListener(ActivateRewardScreen);
        tapTarget.onClick.AddListener(HandleTapToOpen);
    }

    private void ActivateRewardScreen()
    {
        gameObject.SetActive(true);
        tapTarget.gameObject.SetActive(true);
    }

    public void SetImage(Sprite chestSprite)
    {
        if (displayImage != null)
        {
            Debug.Log("Setting new sprite on reward screen.");
            displayImage.sprite = chestSprite;
        }
        else
        {
            Debug.LogError("Display Image is not assigned.");
        }
    }


    private void HandleTapToOpen()
    {
        // Show loading screen and await chest opening confirmation
        LoadingPanel.Instance.ActiveLoadingPanel();
        // Add logic to wait for the chest to actually open if necessary.
        StartCoroutine(DeactivateAfterDelay(1f));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        // Wait for the specified delay duration
        yield return new WaitForSeconds(delay);

        // Deactivate the game object here
        Image tapTargetImage = tapTarget.GetComponent<Image>();
        if (tapTargetImage != null)
        {
            tapTargetImage.color = new Color(tapTargetImage.color.r, tapTargetImage.color.g, tapTargetImage.color.b, 1f);
            tapTargetImage.rectTransform.localScale = new Vector3(1, 1, 1);
            tapTargetImage.rectTransform.offsetMin = new Vector2(0, 0);
            tapTargetImage.rectTransform.offsetMax = new Vector2(0, 0);
        }
        tapTarget.gameObject.SetActive(false);
    }

    public void CloseRewardScreen()
    {
        gameObject.SetActive(false);
    }

    public void OnChestOpenedSuccessfully()
    {
        LoadingPanel.Instance.DesactiveLoadingPanel();
        gameObject.SetActive(false);
    }
}
