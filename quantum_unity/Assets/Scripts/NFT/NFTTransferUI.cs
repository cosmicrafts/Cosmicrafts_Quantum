using UnityEngine;
using TMPro; // Add this for TextMeshPro
using EdjCase.ICP.Candid.Models; // For Principal and UnboundedUInt
using System.Collections.Generic;
using System.Numerics; // For BigInteger
using System.Linq;
using CanisterPK.testnft.Models;
using UnityEngine.UI;

public class NFTTransferUI : MonoBehaviour
{
    public TMP_InputField recipientPrincipalInput;
    public TMP_InputField tokenIdInput;
    public Button transferButton; // Reference to the transfer button
    public TMP_Text notificationText;
    public NFTManager nftManager;

private void Start()
    {
        transferButton.onClick.AddListener(OnTransferButtonPressed); // Add listener for button click
    }
    // This method could be called when the user presses the transfer button
    public async void OnTransferButtonPressed()
    {
        if (recipientPrincipalInput.text == "" || tokenIdInput.text == "")
        {
            Debug.LogError("Recipient Principal or Token ID is empty.");
            return;
        }

        Principal recipientPrincipal = Principal.FromText(recipientPrincipalInput.text);
        UnboundedUInt tokenId = UnboundedUInt.FromBigInteger(BigInteger.Parse(tokenIdInput.text));

        // Assuming TransferNFT now only takes a single tokenId, adjust the call accordingly
        List<UnboundedUInt> tokenIds = new List<UnboundedUInt> { tokenId };

        try
        {
            TransferReceipt receipt = await nftManager.TransferNFT(tokenIds, recipientPrincipal);
            
            if (receipt.Tag == TransferReceiptTag.Ok)
            {
                Debug.Log("NFT transfer successful!");
                // Additional logic for a successful transfer
            }
            else if (receipt.Tag == TransferReceiptTag.Err)
            {
                Debug.LogError($"NFT transfer failed: {receipt.AsErr().ToString()}");
                // Handle the error case
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception during NFT transfer: {ex.Message}");
            // Handle exceptions
        }
    }
}
