using Candid;
using CanisterPK.testicrc1.Models;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;

public class icrc1token : MonoBehaviour
{
    public TMP_Text balanceText;
    public TMP_InputField principalInputField;
    public TMP_InputField tokenAmountInputField;
    public Button sendTokenButton;
    

    void Start()
    {
        // Add click listener for the send token button
        sendTokenButton.onClick.AddListener(SendTokenButtonClicked);

        // Automatically fetch and update balance when the script starts
        FetchBalance();
    }

    public async void FetchBalance()
{
    if (!GlobalGameData.Instance.userDataLoaded || string.IsNullOrEmpty(GlobalGameData.Instance.GetUserData().WalletId) || GlobalGameData.Instance.GetUserData().WalletId == "TestWalletId")
    {
        Debug.LogError("Wallet ID not initialized with actual principal ID. Aborting fetch.");
        return;
    }

    string principalId = GlobalGameData.Instance.GetUserData().WalletId;
    Debug.Log($"Fetching balance for Principal ID: {principalId}");

    try
    {
        var account = new CanisterPK.testicrc1.Models.Account1(Principal.FromText(principalId), new Account1.SubaccountInfo());
        var balance = await CandidApiManager.Instance.testicrc1.Icrc1BalanceOf(account);
        balanceText.text = $"Balance: {balance.ToString()}";
    }
    catch (Exception ex)
    {
        Debug.LogError($"Error fetching balance: {ex.Message}");
    }
}


    private void SendTokenButtonClicked()
    {
        // Parse the token amount input by the user
        if (!BigInteger.TryParse(tokenAmountInputField.text, out BigInteger tokenAmount))
        {
            Debug.LogError("Invalid token amount");
            return;
        }

        // Call send token method with user inputs
        tranferTokens(principalInputField.text, UnboundedUInt.FromBigInteger(tokenAmount));
    }

    public async void tranferTokens(string recipientPrincipalId, UnboundedUInt tokenAmount)
    {
        try
        {
            var fee = await CandidApiManager.Instance.testicrc1.Icrc1Fee();
            
            var transfer = new CanisterPK.testicrc1.Models.TransferArgs(
                UnboundedUInt.FromBigInteger(1000000),
                null,
                new OptionalValue<UnboundedUInt>(fee),
                null, 
                null, 
                new Account(Principal.FromText(recipientPrincipalId), null)
            );
            
            var transferResult = await CandidApiManager.Instance.testicrc1.Icrc1Transfer(transfer);
            
            // Log the result of the transfer
            if (transferResult.Tag == TransferResultTag.Err)
            {
                Debug.LogError(JsonUtility.ToJson(transferResult.Value));
            }
            else
            {
                Debug.Log("Transfer successful");
            }

            // Update balance after transfer
            FetchBalance();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send tokens: {ex.Message}");
        }
    }
}
