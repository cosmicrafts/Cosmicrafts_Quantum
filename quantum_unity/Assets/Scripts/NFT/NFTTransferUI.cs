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
                // Invoke the removal of the transferred NFT
                nftManager.RemoveNFT(tokenIdToTransfer);
            }

            else if (receipt.Tag == TransferReceiptTag.Err)
{
    TransferError error = receipt.AsErr();
    switch (error.Tag)
    {
        case TransferErrorTag.CreatedInFuture:
            Debug.LogError("NFT transfer failed: Transfer created in the future.");
            // Access specific fields if needed
            var futureInfo = error.AsCreatedInFuture();
            Debug.LogError($"Ledger Time: {futureInfo.LedgerTime}");
            break;
        case TransferErrorTag.Duplicate:
            Debug.LogError("NFT transfer failed: Duplicate transfer.");
            // Access specific fields if needed
            var duplicateInfo = error.AsDuplicate();
            Debug.LogError($"Duplicate of Transfer ID: {duplicateInfo.DuplicateOf}");
            break;
        case TransferErrorTag.GenericError:
            var genericInfo = error.AsGenericError();
            Debug.LogError($"NFT transfer failed: Generic error - {genericInfo.Message}");
            break;
        case TransferErrorTag.TemporarilyUnavailable:
            Debug.LogError("NFT transfer failed: Service temporarily unavailable.");
            break;
        case TransferErrorTag.TooOld:
            Debug.LogError("NFT transfer failed: Transfer too old.");
            break;
        case TransferErrorTag.Unauthorized:
            Debug.LogError("NFT transfer failed: Unauthorized.");
            var unauthorizedInfo = error.AsUnauthorized();
            Debug.LogError($"Unauthorized Token IDs: {string.Join(", ", unauthorizedInfo.TokenIds)}");
            break;
        default:
            Debug.LogError("NFT transfer failed: Unknown error.");
            break;
    }
}

        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[NFTTransferUI]Exception during NFT transfer: {ex.Message}");
            notificationText.text = $"Exception during NFT transfer: {ex.Message}";
        }
    }

}
