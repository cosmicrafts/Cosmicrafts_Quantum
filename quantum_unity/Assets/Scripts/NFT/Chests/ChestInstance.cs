using UnityEngine;
using UnityEngine.UI;
using EdjCase.ICP.Candid.Models;

public class ChestInstance : MonoBehaviour
{
    public ChestSO chestSO;
    public UnboundedUInt tokenId;
    public Image chestImage;
    public ChestOpenerUI chestOpenerUI;
    public ChestTransferUI chestTransferUI;

    public void OnChestSelected()
    {
        Debug.Log("Chest selected. Determining action...");

        // Check if the ChestOpenerUI is active and perform the opening action.
        if (chestOpenerUI != null && chestOpenerUI.gameObject.activeSelf)
        {
            Debug.Log("Opening chest.");
            chestOpenerUI.SelectChestForOpening(chestSO, tokenId);
        }
        
        // Check if the ChestTransferUI is active and perform the transfer action.
        if (chestTransferUI != null && chestTransferUI.gameObject.activeSelf)
        {
            Debug.Log("Transferring chest.");
            chestTransferUI.SetSelectedChest(this);
        }

        // Log an error if no appropriate UI was found or active.
        if ((chestOpenerUI == null || !chestOpenerUI.gameObject.activeSelf) && 
            (chestTransferUI == null || !chestTransferUI.gameObject.activeSelf))
        {
            Debug.LogError("No operation UI found or active for the chest.");
        }
    }

    public void Setup(ChestSO so, UnboundedUInt id)
    {
        chestSO = so;
        tokenId = id;
        if (chestImage != null)
        {
            chestImage.sprite = chestSO.icon;
        }
        else
        {
            Debug.LogError("Image component not found on ChestInstance. Please assign it in the Inspector.");
        }
    }
}