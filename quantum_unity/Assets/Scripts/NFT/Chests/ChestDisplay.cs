using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChestDisplay : MonoBehaviour
{
    public Image chestImage;
    public Sprite[] raritySprites;

    public void SetChestData(ChestData chestData)
    {
        if (raritySprites.Length >= chestData.Rarity) 
        {
            chestImage.sprite = raritySprites[chestData.Rarity - 1]; // Adjust if your indices start from 0
        } 
        else 
        {
            Debug.LogWarning("Not enough rarity sprites for this chest rarity.");
        }
    }
}
