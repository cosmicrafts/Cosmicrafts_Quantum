using UnityEngine;
using TMPro;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using System.Numerics;
using CanisterPK.testnft.Models;
using UnityEngine.UI;

public class NFTTransferUI : MonoBehaviour
{
    public TMP_InputField recipientPrincipalInput;
    public Button transferButton;
    public TMP_Text notificationText;
    public NFTManager nftManager;
    public NFTCard nftCard;

    private void Start()
    {
        transferButton.onClick.AddListener(OnTransferButtonPressed);
    }

    public async void OnTransferButtonPressed()
    {
        string tokenIdToTransfer = nftCard.TokenId;
        if (string.IsNullOrEmpty(recipientPrincipalInput.text) || string.IsNullOrEmpty(tokenIdToTransfer))
        {
            Debug.LogError("Recipient Principal or Token ID is empty.");
            notificationText.text = "Recipient Principal or Token ID is empty.";
            return;
        }


        Debug.Log($"Transferring NFT with Token ID: {tokenIdToTransfer}");

        Principal recipientPrincipal = Principal.FromText(recipientPrincipalInput.text);
        UnboundedUInt tokenId = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenIdToTransfer));

        List<UnboundedUInt> tokenIds = new List<UnboundedUInt> { tokenId };

        try
        {
            TransferReceipt receipt = await nftManager.TransferNFT(tokenIds, recipientPrincipal);

            if (receipt.Tag == TransferReceiptTag.Ok)
            {
                Debug.Log("NFT transfer successful!");
                notificationText.text = $"NFT transfer successful! Token ID: {tokenIdToTransfer}";
            }
            else if (receipt.Tag == TransferReceiptTag.Err)
            {
                Debug.LogError($"NFT transfer failed: {receipt.AsErr().ToString()}");
                notificationText.text = $"NFT transfer failed: {receipt.AsErr().ToString()}";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[NFTTransferUI]Exception during NFT transfer: {ex.Message}");
            notificationText.text = $"Exception during NFT transfer: {ex.Message}";
        }
    }

}
