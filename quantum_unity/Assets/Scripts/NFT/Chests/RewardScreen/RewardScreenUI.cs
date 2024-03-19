using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using TMPro;
using System;
using System.Linq;


public class RewardScreenUI : MonoBehaviour
{
    public Image displayImage; 
    public Button tapTarget;
    public UnityEvent onActivateRewardScreen = new UnityEvent();
    public Transform rewardsContainer;
    public GameObject shardsRewardPrefab;
    public GameObject fluxRewardPrefab;
    public GameObject rewardsPanel;
    public flux FluxScript;
    public shards ShardsScript;
    public ChestOpenerUI chestOpenerUI;

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
            displayImage.sprite = chestSprite;
        else
            Debug.LogError("Display Image is not assigned.");
    }

    private void HandleTapToOpen()
    {
        chestOpenerUI.UserInteracted();
        if (!chestOpenerUI.AreRewardsFetched())
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
        }
        else
        {
            ShowRewardsUI(); // Directly show rewards if already fetched
        }
        StartCoroutine(DeactivateAfterDelay(1f));
    }

    public void OnChestOpenedSuccessfully()
    {
        LoadingPanel.Instance.DesactiveLoadingPanel();
        if (chestOpenerUI.UserHasInteracted())
        {
            ShowRewardsUI();
        }
    }

    public void ShowRewardsUI()
    {
        rewardsPanel.gameObject.SetActive(true);
        rewardsContainer.gameObject.SetActive(true);
        chestOpenerUI.ResetFlags();
        Debug.Log("Rewards shown, resetting status for next interaction.");
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
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

    public void DisplayRewards(string tokenType, int amount)
    {
        GameObject rewardPrefab = null;

        switch (tokenType)
        {
            case "Shards":
                rewardPrefab = shardsRewardPrefab;
                break;
            case "Flux":
                rewardPrefab = fluxRewardPrefab;
                break;
            default:
                Debug.LogError("Unknown token type: " + tokenType);
                return;
        }

        // Instantiate the prefab
        GameObject rewardInstance = Instantiate(rewardPrefab, rewardsContainer);
        // Ensure the instantiated prefab is active in case the original prefab was inactive.
        rewardInstance.SetActive(true);

        RewardPrefabScript rewardScript = rewardInstance.GetComponent<RewardPrefabScript>();
        if (rewardScript != null)
        {
            // Set the amount to display on the prefab
            rewardScript.SetAmount(amount);

            // Setup the onClick event for the prefab
            Button rewardButton = rewardInstance.GetComponentInChildren<Button>();
            if (rewardButton != null)
            {
                rewardButton.onClick.AddListener(() => {
                    // Call the respective function to update balance based on the token type
                    if (tokenType == "Flux")
                    {
                        FluxScript.FetchBalance(); // Assuming FetchBalance updates the balance
                    }
                    else if (tokenType == "Shards")
                    {
                        ShardsScript.FetchBalance(); // Similarly for shards
                    }

                    // Destroy the prefab instance after clicking
                    Destroy(rewardInstance);
                });
            }
        }
    }

    public void HandleRewardMessage(string message)
    {
        // Remove the leading part of the message up to the first actual reward data
        string rewardsInfo = message.Substring(message.IndexOf('{'));
        
        var segments = System.Text.RegularExpressions.Regex.Split(rewardsInfo, @"(?<=})(?={)")
                            .Where(s => !string.IsNullOrEmpty(s))
                            .ToArray();

        foreach (var segment in segments)
        {
            string tokenType = ExtractValue(segment, "\"token\":\"", "\"");
            string amountString = ExtractValue(segment, "\"amount\":", "}");
            int amount = 0;
            
            // Parse the amount if possible
            if (!int.TryParse(amountString, out amount))
            {
                Debug.LogError("Failed to parse amount from segment: " + segment);
                continue;
            }

            DisplayRewards(tokenType, amount);
        }
    }

    // Extracts a value from a segment given start and end delimiters
    private string ExtractValue(string segment, string startDelimiter, string endDelimiter)
    {
        int startIndex = segment.IndexOf(startDelimiter) + startDelimiter.Length;
        int endIndex = segment.IndexOf(endDelimiter, startIndex);
        if (endIndex == -1) endIndex = segment.Length; // Adjust if endDelimiter is not found.
        string value = segment.Substring(startIndex, endIndex - startIndex).Trim(new char[] { '\"', ' ', '}' });
        return value;
    }

}