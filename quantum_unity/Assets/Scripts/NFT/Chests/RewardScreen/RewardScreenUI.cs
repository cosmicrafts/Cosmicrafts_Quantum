using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class RewardScreenUI : MonoBehaviour
{
    public Image displayImage; // Make sure to assign this in the Inspector
    public Button tapTarget; // The button on the image for fake tap
    public GameObject loadingScreen; // Assign in Inspector

    public UnityEvent onActivateRewardScreen = new UnityEvent();

    private void Awake()
    {
        onActivateRewardScreen.AddListener(ActivateRewardScreen);
        tapTarget.onClick.AddListener(HandleTapToOpen);
    }

    private void ActivateRewardScreen()
    {
        gameObject.SetActive(true); // Activate the reward screen
    }

    public void SetImage(Sprite chestSprite)
    {
        if (displayImage != null)
        {
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
        loadingScreen.SetActive(true);
        // Add logic to wait for the chest to actually open if necessary.
    }

    public void CloseRewardScreen()
    {
        gameObject.SetActive(false);
    }

    public void OnChestOpened()
    {
        loadingScreen.SetActive(false);
        // Optionally, show rewards here or close the reward screen
    }
}
