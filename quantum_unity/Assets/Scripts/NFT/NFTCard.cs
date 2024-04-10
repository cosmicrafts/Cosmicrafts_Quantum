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
        public static event Action<NFTCard> OnCardSelected;

        
        protected string tokenId;
        [HideInInspector] public NFTData nftData;

        public string TokenId => tokenId;
        public string Name => nftData.General.FirstOrDefault()?.Name ?? "Default Name";
        public string Level => GetValueFromStats("level");
        public Sprite Avatar => iconImage.sprite;

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
                costText.text = GetEnergyCostById( general.UnitId ).ToString();
            }
            levelText.text = GetValueFromStats("level");
        }
    
        public string GetValueFromStats(string statName)
        {
            var stat = nftData.BasicStats.FirstOrDefault(s => s.StatName.ToLower() == statName.ToLower());
            return stat != null ? $"{stat.StatValue}" : "Not Available";
        }

        public Sprite GetIconSpriteById(int iconId)
        {
            return UnityDB.FindAsset<CardSettingsAsset>(NFTManager.Instance.m_GameplaySettings.Settings.AllCards[iconId].Id).Sprite;
        }
        
        public int GetEnergyCostById(int cardId)
        {
            return UnityDB.FindAsset<CardSettingsAsset>(NFTManager.Instance.m_GameplaySettings.Settings.AllCards[cardId].Id).GetEnergyCost();
        }
        
        public void SelectCard()
        {
            Debug.Log($"SelectCard called on {tokenId}");
            IsSelected = true;
            unitNameText.color = Color.green;
            OnCardSelected?.Invoke(this);
            NFTTransferUI.Instance?.HandleCardDirectly(this);
            NFTUpgradeUI.Instance?.HandleCardSelected(this);
        }
        public void DeselectCard()
        {
            IsSelected = false;
            unitNameText.color = Color.white;
        }
    }
