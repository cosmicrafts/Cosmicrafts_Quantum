using Candid;
using CanisterPK.testicrc1.Models;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;
using System.Collections;
using System.Linq;
using Cosmicrafts.Managers;

public class shards : MonoBehaviour
{
    public TMP_Text balanceText;
    public TMP_InputField principalInputField;
    public TMP_InputField tokenAmountInputField;
    public Button sendTokenButton;
    public TMP_Text transferStatusText; 
    public Animator ShardsPanel;
    public BigInteger CurrentBalance { get; private set; } = BigInteger.Zero;
    public TMP_Text tokenNameText; 
    public Image tokenImage; 
    public Sprite referenceImage;
    public NotificationManager notificationManager;

    private const int DECIMAL_PLACES = 6;
    private Coroutine balanceAnimationCoroutine;

    void Start()
    {
        sendTokenButton.onClick.AddListener(SendTokenButtonClicked);
        ShardsPanel = GameObject.Find("ShardsPanel").GetComponent<Animator>();
        FetchBalance();
    }

    private void OnEnable()
    {
        BalanceManager.OnBalanceUpdateNeeded += FetchBalance;
    }

    private void OnDisable()
    {
        BalanceManager.OnBalanceUpdateNeeded -= FetchBalance;

        // Stop any ongoing balance animation coroutine
        if (balanceAnimationCoroutine != null)
        {
            StopCoroutine(balanceAnimationCoroutine);
            balanceAnimationCoroutine = null;
        }
    }

    public async void FetchBalance()
    {
        if (GameDataManager.Instance == null)
            {
                Debug.LogError("[boomtoken] GameDataManager instance is null.");
                return;
            }

            var playerData = GameDataManager.Instance.playerData;
            if (playerData == null)
            {
                Debug.LogError("Failed to load player data.");
                return;
            }

            string principalId = playerData.PrincipalId;
            Debug.Log($"Fetching balance for Principal ID: {principalId}");

        try
        {
            var account = new CanisterPK.testicrc1.Models.Account1(Principal.FromText(principalId), new Account1.SubaccountInfo());
            var balance = await CandidApiManager.Instance.testicrc1.Icrc1BalanceOf(account);

            if (balance == null)
            {
                Debug.LogError("[shards] Fetched balance is null.");
                return;
            }

            // Trigger the balance animation with the new balance
            AnimateBalanceUpdate(balance);

            if (ShardsPanel == null)
            {
                Debug.LogError("Panel Animator is not assigned.");
            }
            else
            {
                ShardsPanel.Play("TokenPanelRefresh", -1, 0f);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching balance: {ex.Message}");
        }
        Debug.Log($"[shards] Fetched balance: {CurrentBalance}");
    }

    private string FormatBalance(UnboundedUInt balance)
    {
        return balance.ToString();
    }

    public void UpdateBalanceLocally(int cost)
    {
        CurrentBalance -= new BigInteger(cost);
        balanceText.text = CurrentBalance.ToString();
    }

    private void SendTokenButtonClicked()
    {
        if (!decimal.TryParse(tokenAmountInputField.text, out decimal tokenAmount))
        {
            Debug.LogError("Invalid token amount");
            return;
        }

        BigInteger tokenAmountBigInt = ConvertToBigInteger(tokenAmount);

        SetTransferStatus("Sending...");
        LoadingPanel.Instance.ActiveLoadingPanel();

        transferTokens(principalInputField.text, UnboundedUInt.FromBigInteger(tokenAmountBigInt), "Shards");
    }

    private BigInteger ConvertToBigInteger(decimal value)
    {
        return BigInteger.Parse(value.ToString());
    }

    public async void transferTokens(string recipientPrincipalId, UnboundedUInt tokenAmount, string tokenName)
    {
        try
        {
            var fee = await CandidApiManager.Instance.testicrc1.Icrc1Fee();

            var transfer = new CanisterPK.testicrc1.Models.TransferArgs(
                tokenAmount, // Use the actual tokenAmount here
                null,
                new OptionalValue<UnboundedUInt>(fee),
                null,
                null,
                new Account(Principal.FromText(recipientPrincipalId), null)
            );

            var transferResult = await CandidApiManager.Instance.testicrc1.Icrc1Transfer(transfer);
            LoadingPanel.Instance.DesactiveLoadingPanel();

            // Update status text based on transfer result
            if (transferResult.Tag == TransferResultTag.Err)
            {
                SetTransferStatus("Transfer failed");
                notificationManager.ShowNotification("Transfer failed");
                Debug.LogError(JsonUtility.ToJson(transferResult.Value));
            }
            else
            {
                SetTransferStatus("Transfer successful");
                notificationManager.ShowNotification($"Transferred {tokenAmount} {tokenName} successfully");
                Debug.Log("Transfer successful");
            }

            // Update balance after transfer
            FetchBalance();
        }
        catch (Exception ex)
        {
            SetTransferStatus($"Failed to send tokens: {ex.Message}");
            notificationManager.ShowNotification($"Failed to send tokens: {ex.Message}");
            Debug.LogError($"Failed to send tokens: {ex.Message}");
        }
    }

    private void SetTransferStatus(string status)
    {
        transferStatusText.text = status;
    }

    private void AnimateBalanceUpdate(UnboundedUInt newBalance)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("AnimateBalanceUpdate called on an inactive GameObject.");
            return;
        }

        BigInteger newBalanceBigInt = newBalance.ToBigInteger();
        CurrentBalance = newBalanceBigInt;

        if (balanceAnimationCoroutine != null)
        {
            StopCoroutine(balanceAnimationCoroutine);
        }

        balanceAnimationCoroutine = StartCoroutine(IncrementBalanceAnimation(newBalanceBigInt));
    }

    private IEnumerator IncrementBalanceAnimation(BigInteger targetBalance) 
    {
        float duration = 0.5f;
        BigInteger currentBalance;

        if (!BigInteger.TryParse(SanitizeText(balanceText.text), out currentBalance)) 
        {
            Debug.Log("Current balance is not a valid number, defaulting to zero.");
            currentBalance = BigInteger.Zero;
        }

        float elapsed = 0f;
        while (elapsed < duration) 
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);

            BigInteger progressBalance = currentBalance + (targetBalance - currentBalance) * new BigInteger((long)(progress * 100000)) / new BigInteger(100000);

            balanceText.text = progressBalance.ToString();
            yield return null;
        }

        balanceText.text = targetBalance.ToString();
    }

    private string SanitizeText(string text) 
    {
        return new string(text.Where(char.IsDigit).ToArray());
    }

    public void UpdateTokenPanel(string tokenName, Sprite tokenSprite)
    {
        tokenNameText.text = tokenName;
        tokenImage.sprite = tokenSprite;
    }

    public void OnUpdateTokenPanelClick()
    {
        UpdateTokenPanel("Shards", referenceImage);
    }
}