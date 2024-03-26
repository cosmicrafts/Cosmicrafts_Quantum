using System;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class NFTCard : MonoBehaviour
{
    [HideInInspector]
    public bool IsSelected;
    public int DeckSlot;
    public Animator animator;
    
    [Header("SlotsInfo: ")]
    public Image iconImage;
    public TMP_Text unitNameText;
    public Image factionIcon;
    public TMP_Text levelText;
    public TMP_Text costText;
    
    protected string tokenId;
    [HideInInspector] public NFTData nftData;

    public string TokenId => tokenId;

    public virtual void SetNFTData(NFTData nftData)
    {
        this.nftData = nftData;
        tokenId = nftData.TokenId;
        
        if (nftData == null)
        {
            Debug.LogWarning("NFTData not assigned.");
            return;
        }

        var general = nftData.General.FirstOrDefault();

        // Debug.Log(nftData.General.Count);
        
        if (general != null)
        {
            unitNameText.text = tokenId + ":" + general.UnitId.ToString() ;//general.Name;
          //  Debug.Log(general.Class +""+ general.UnitId.ToString() );
          //  Debug.Log(int.Parse(tokenId) % 10);
            iconImage.sprite = GetIconSpriteById( general.UnitId );
        }
        levelText.text = GetValueFromStats("level");
    }
   
    public string GetValueFromStats(string statName)
    {
        var stat = nftData.BasicStats.FirstOrDefault(s => s.StatName.ToLower() == statName.ToLower());
        return stat != null ? $"{stat.StatName}: {stat.StatValue}" : $"{statName}: Not Available";
    }
    public Sprite GetIconSpriteById(int iconId)
    {
        return UnityDB.FindAsset<CardSettingsAsset>(NFTManager.Instance.m_GameplaySettings.Settings.AllCards[iconId].Id).Sprite;
    }
    
    
    public void SelectCard()
    {
        IsSelected = true;
        unitNameText.color = Color.green;
    }
    public void DeselectCard()
    {
        IsSelected = false;
        unitNameText.color = Color.white;
    }
}
