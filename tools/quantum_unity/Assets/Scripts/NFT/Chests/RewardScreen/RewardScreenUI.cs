using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Linq;

namespace Cosmicrafts {
    public class RewardScreenUI : MonoBehaviour
    {
        public Image displayImage;
        public Button tapTarget;
        public UnityEvent onActivateRewardScreen = new UnityEvent();
        public Transform rewardsContainer;
        public GameObject StardustRewardPrefab;
        public GameObject rewardsPanel;
        public Stardust StardustScript;
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
        case "Stardust":
            rewardPrefab = StardustRewardPrefab;
            break;
        default:
            Debug.LogError("Unknown token type: " + tokenType);
            return;
    }

    GameObject rewardInstance = Instantiate(rewardPrefab, rewardsContainer);
    rewardInstance.SetActive(true);
    activeRewardsCount++;

    RewardPrefabScript rewardScript = rewardInstance.GetComponent<RewardPrefabScript>();
    if (rewardScript != null)
    {
        rewardScript.SetAmount(amount); // Pass the correct amount to the reward script

        Button rewardButton = rewardInstance.GetComponentInChildren<Button>();
        if (rewardButton != null)
        {
            rewardButton.onClick.AddListener(() => {

                if (tokenType == "Stardust")
                {
                    StardustScript.FetchBalance(); // Fetch updated balance
                }

                Animator animator = rewardInstance.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("notnew");
                }

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
    // Assuming the message comes in the format (true, "{\"token\":\"Stardust\", \"transaction_id\": 0, \"amount\": 101}")
    int jsonStartIndex = message.IndexOf('{');
    if (jsonStartIndex < 0)
    {
        Debug.LogError("Invalid message format: JSON part not found.");
        return;
    }

    string jsonResponse = message.Substring(jsonStartIndex);
    Debug.Log("Parsed JSON response: " + jsonResponse);

    // Extract token type and amount from the JSON response
    string tokenType = ExtractValue(jsonResponse, "\"token\":\"", "\"");
    string amountString = ExtractValue(jsonResponse, "\"amount\":", "}");

    int amount = 0;
    if (!int.TryParse(amountString, out amount))
    {
        Debug.LogError("Failed to parse amount from JSON response: " + jsonResponse);
        return;
    }

    // Display the rewards
    DisplayRewards(tokenType, amount);
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
}