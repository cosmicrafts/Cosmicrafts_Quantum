using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using TMPro;
using System;
using System.Linq;
using TowerRush;

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
    private int activeRewardsCount = 0;
    public SimpleDeactivate deactivationAnimScript;
    public Animator chestAnimator;

    public AudioClip rewardSound;

    private void Awake()
    {
        onActivateRewardScreen.AddListener(ActivateRewardScreen);
        tapTarget.onClick.AddListener(HandleTapToOpen);
    }

    public void ActivateRewardScreen()
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
        LoadingPanel.Instance.ActiveLoadingPanel();
    }

    public void OnChestOpenedSuccessfully()
    {
        LoadingPanel.Instance.DesactiveLoadingPanel();
        StartCoroutine(DeactivateAfterDelay(.35f));
        ShowRewardsUI();
    }

    public void ShowRewardsUI()
    {
        rewardsPanel.gameObject.SetActive(true);
        rewardsContainer.gameObject.SetActive(true);
        Debug.Log("Rewards shown, resetting status for next interaction.");
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        if (chestAnimator != null)
        {
            chestAnimator.Play("ChestClose");
        }

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
        // Increment the active rewards count
        activeRewardsCount++;

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
                    // Call the respective function to update balance locally based on the token type
                    if (tokenType == "Flux")
                    {
                        FluxScript.UpdateBalanceLocally(-amount);
                    }
                    else if (tokenType == "Shards")
                    {
                        ShardsScript.UpdateBalanceLocally(-amount);
                    }

                    // Play the animation associated with the reward
                    Animator animator = rewardInstance.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.Play("notnew");
                    }

                    // Optionally, wait for the animation to finish before destroying the instance
                    float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
                    StartCoroutine(DestroyAfterAnimation(rewardInstance, animationDuration));

                    activeRewardsCount--;
                    if (activeRewardsCount <= 0)
                    {
                        StartCoroutine(StartDeactivationWithDelay(0.5f));
                    }
                });
            }
        }
    }

    IEnumerator StartDeactivationWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (deactivationAnimScript != null)
        {
            deactivationAnimScript.StartDeactivation();
        }
        else
        {
            Debug.LogError("DeactivationAnim script not assigned.");
        }
    }

    IEnumerator DestroyAfterAnimation(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(instance);
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
