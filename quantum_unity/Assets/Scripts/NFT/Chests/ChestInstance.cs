using UnityEngine;
using UnityEngine.UI;
using EdjCase.ICP.Candid.Models;
using TMPro;

namespace Cosmicrafts
{

    public class ChestInstance : MonoBehaviour
    {
        public ChestSO chestSO;
        public UnboundedUInt tokenId;
        public Image chestImage;
        public ChestOpenerUI chestOpenerUI;
        public ChestTransferUI chestTransferUI;
        public TMP_Text nameText;
        public TMP_Text tokenIdText;

        public void OnChestSelected()
        {
            Debug.Log("[Chest Manager] Chest selected. Determining action...");

            // Check if the ChestOpenerUI is active and perform the opening action.
            if (chestOpenerUI != null && chestOpenerUI.gameObject.activeSelf)
            {
                Debug.Log("[Chest Manager] Opening chest.");
                chestOpenerUI.SelectChestForOpening(chestSO, tokenId);
            }

            // Check if the ChestTransferUI is active and perform the transfer action.
            if (chestTransferUI != null && chestTransferUI.gameObject.activeSelf)
            {
                Debug.Log("[Chest Manager] Transferring chest.");
                chestTransferUI.SetSelectedChest(this);
            }

            // Log an error if no appropriate UI was found or active.
            if ((chestOpenerUI == null || !chestOpenerUI.gameObject.activeSelf) &&
                (chestTransferUI == null || !chestTransferUI.gameObject.activeSelf))
            {
                Debug.LogError("[Chest Manager] No operation UI found or active for the chest.");
            }
        }

        public void Setup(ChestSO so, UnboundedUInt id)
        {
            chestSO = so;
            tokenId = id;

            // Assign the icon from the ChestSO to the Image component of this instance
            if (chestImage != null)
            {
                chestImage.sprite = chestSO.icon;
                Debug.Log($"[Chest Manager] Set chest icon: {chestSO.icon.name}");
            }
            else
            {
                Debug.LogError("[Chest Manager] Image component not found on ChestInstance. Please assign it in the Inspector.");
            }

            // Set the text for name, rarity, and tokenId
            if (nameText != null)
            {
                nameText.text = chestSO.chestName;
                Debug.Log($"[Chest Manager] Set chest name: {chestSO.chestName}");
            }
            if (tokenIdText != null)
            {
                tokenIdText.text = "ID: " + tokenId.ToString();
                Debug.Log($"[Chest Manager] Set token ID text: ID: {tokenId.ToString()}");
            }
        }
    }

}