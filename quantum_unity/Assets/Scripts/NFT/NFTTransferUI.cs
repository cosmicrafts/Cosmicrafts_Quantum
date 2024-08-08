using UnityEngine;
using TMPro;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using System.Numerics;
using Cosmicrafts.MainCanister.Models;
using UnityEngine.UI;
namespace Cosmicrafts.Data
{
    public class NFTTransferUI : MonoBehaviour
    {
        public TMP_InputField recipientPrincipalInput;
        public Button transferButton;
        public TMP_Text notificationText;
        public NFTManager nftManager;
        public NFTCard nftCard;

        public TMP_Text nameText;
        public TMP_Text levelText;
        public TMP_Text tokenId;
        public Image avatarImage;
        public static NFTTransferUI Instance;

        public NotificationManager notificationManager;

        public SimpleDeactivate deactivationTarget1;
        public SimpleDeactivate deactivationTarget2;


        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            transferButton.onClick.AddListener(OnTransferButtonPressed);
            DisplayNFTInfo();
        }


        public void HandleCardSelected(NFTCard selectedCard)
        {
            Debug.Log($"HandleCardSelected called with {selectedCard.TokenId}");
            nftCard = selectedCard;
            DisplayNFTInfo();
        }

        private void DisplayNFTInfo()
        {
            if (nftCard != null && nftCard.nftData != null)
            {
                nameText.text = nftCard.Name;
                levelText.text = nftCard.Level;
                tokenId.text = nftCard.TokenId;
                avatarImage.sprite = nftCard.Avatar;
            }
        }

        public async void OnTransferButtonPressed()
        {
            LoadingPanel.Instance.ActiveLoadingPanel();
            string tokenIdToTransfer = nftCard.TokenId;
            if (string.IsNullOrEmpty(recipientPrincipalInput.text) || string.IsNullOrEmpty(tokenIdToTransfer))
            {
                Debug.LogError("Recipient Principal or Token ID is empty.");
                notificationText.text = "Recipient Principal or Token ID is empty.";
                notificationManager.ShowNotification("Recipient Principal or Token ID is empty.");  // Trigger notification
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
                    notificationManager.ShowNotification($"NFT transfer successful! Token ID: {tokenIdToTransfer}");  // Trigger notification
                    nftManager.RemoveNFT(tokenIdToTransfer);
                    LoadingPanel.Instance.DesactiveLoadingPanel();
                    deactivationTarget1?.StartDeactivation();
                    deactivationTarget2?.StartDeactivation();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[NFTTransferUI]Exception during NFT transfer: {ex.Message}");
                notificationText.text = $"Exception during NFT transfer: {ex.Message}";
                notificationManager.ShowNotification($"Exception during NFT transfer: {ex.Message}");  // Trigger notification
            }
        }

        private void HandleTransferError(TransferError error)
        {
            string errorMessage = "NFT transfer failed: ";
            switch (error.Tag)
            {
                case TransferErrorTag.CreatedInFuture:
                    errorMessage += "Transfer created in the future.";
                    break;
                case TransferErrorTag.Duplicate:
                    errorMessage += "Duplicate transfer.";
                    break;
                case TransferErrorTag.GenericError:
                    errorMessage += error.AsGenericError().Message;
                    break;
                case TransferErrorTag.TemporarilyUnavailable:
                    errorMessage += "Service temporarily unavailable.";
                    break;
                case TransferErrorTag.TooOld:
                    errorMessage += "Transfer too old.";
                    break;
                default:
                    errorMessage += "Unknown error.";
                    break;
            }
            Debug.LogError(errorMessage);
            notificationManager.ShowNotification(errorMessage);  // Trigger notification
            LoadingPanel.Instance.DesactiveLoadingPanel();
        }

    }
}