using Candid;
using CanisterPK.flux.Models;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;
using System.Collections;
using System.Linq;


public class flux : MonoBehaviour
{
    public TMP_Text balanceText;
    public TMP_InputField principalInputField;
    public TMP_InputField tokenAmountInputField;
    public Button sendTokenButton;
    public TMP_Text transferStatusText; 
    public Animator TokenPanel;


    private const int DECIMAL_PLACES = 6;
    void Start()
    {
        sendTokenButton.onClick.AddListener(SendTokenButtonClicked);
        TokenPanel = GameObject.Find("TokenPanel").GetComponent<Animator>();
        FetchBalance();
    }

    private void OnEnable()
{
    BalanceManager.OnBalanceUpdateNeeded += FetchBalance;
}

private void OnDisable()
{
    BalanceManager.OnBalanceUpdateNeeded -= FetchBalance;
}


    public async void FetchBalance()
    {
        string principalId = GlobalGameData.Instance.GetUserData().WalletId;
        Debug.Log($"Fetching balance for Principal ID: {principalId}");

        try
        {
            var account = new CanisterPK.flux.Models.Account1(Principal.FromText(principalId), new Account1.SubaccountInfo());
            var balance = await CandidApiManager.Instance.flux.Icrc1BalanceOf(account);

            // Trigger the balance animation with the new balance
            AnimateBalanceUpdate(balance);

            if (TokenPanel == null)
            {
                Debug.LogError("Panel Animator is not assigned.");
            }
            else
            {
                TokenPanel.Play("TokenPanelRefresh", -1, 0f);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error fetching balance: {ex.Message}");
        }
    }

    private string FormatBalance(UnboundedUInt balance)
    {
       // string balanceString = balance.ToString().PadLeft(DECIMAL_PLACES + 1, '0'); // Add one to account for the integer part
      //  return balanceString.Substring(0, balanceString.Length - DECIMAL_PLACES);
      return balance.ToString();
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

        tranferTokens(principalInputField.text, UnboundedUInt.FromBigInteger(tokenAmountBigInt));
    }

    private BigInteger ConvertToBigInteger(decimal value)
    {
        // Scale the decimal value to the appropriate number of decimal places and convert to BigInteger
       // return new BigInteger(value * (decimal)Math.Pow(10, DECIMAL_PLACES));
       return BigInteger.Parse(value.ToString());
    }

    public async void tranferTokens(string recipientPrincipalId, UnboundedUInt tokenAmount)
    {
        try
        {
            var fee = await CandidApiManager.Instance.flux.Icrc1Fee();

            var transfer = new CanisterPK.flux.Models.TransferArgs(
                UnboundedUInt.FromBigInteger(1),
                null,
                new OptionalValue<UnboundedUInt>(fee),
                null,
                null,
                new Account(Principal.FromText(recipientPrincipalId), null)
            );

            var transferResult = await CandidApiManager.Instance.flux.Icrc1Transfer(transfer);

            // Update status text based on transfer result
            if (transferResult.Tag == TransferResultTag.Err)
            {
                SetTransferStatus("Transfer failed");
                Debug.LogError(JsonUtility.ToJson(transferResult.Value));
            }
            else
            {
                SetTransferStatus("Transfer successful");
                Debug.Log("Transfer successful");
            }

            // Update balance after transfer
            FetchBalance();
        }
        catch (Exception ex)
        {
            SetTransferStatus($"Failed to send tokens: {ex.Message}");
            Debug.LogError($"Failed to send tokens: {ex.Message}");
        }
    }

    private void SetTransferStatus(string status)
    {
        transferStatusText.text = status;
    }

    // Method to start the balance update animation
    private void AnimateBalanceUpdate(UnboundedUInt newBalance)
    {
        // Assuming you have a method to convert UnboundedUInt to BigInteger or decimal for comparison
        BigInteger newBalanceBigInt = newBalance.ToBigInteger();
        StartCoroutine(IncrementBalanceAnimation(newBalanceBigInt));
    }

    // Coroutine for animating the balance update
    private IEnumerator IncrementBalanceAnimation(BigInteger targetBalance) {
    float duration = 0.75f;
    BigInteger currentBalance;

    if (!BigInteger.TryParse(SanitizeText(balanceText.text), out currentBalance)) {
        Debug.Log("Current balance is not a valid number, defaulting to zero.");
        currentBalance = BigInteger.Zero;
    }

    float elapsed = 0f;
    while (elapsed < duration) {
        elapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsed / duration);

        BigInteger progressBalance = currentBalance + (targetBalance - currentBalance) * new BigInteger((long)(progress * 100000)) / new BigInteger(100000);

        balanceText.text = progressBalance.ToString();
        yield return null;
    }

    balanceText.text = targetBalance.ToString();
    Debug.Log($"Final balance text: {balanceText.text}");
}

    private string SanitizeText(string text) {
    return new string(text.Where(char.IsDigit).ToArray());
}

}
