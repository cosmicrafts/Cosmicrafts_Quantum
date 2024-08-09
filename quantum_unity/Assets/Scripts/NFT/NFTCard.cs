using System;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Cosmicrafts.Data
{
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

            // Filter to only allow Unit NFTs
            if (nftData.Category.TagName != "Unit")
            {
                Debug.LogWarning("NFT is not a Unit. Skipping...");
                return;
            }

            var general = nftData.General.FirstOrDefault();

            if (general != null)
            {
                unitNameText.text = tokenId + ":" + general.UnitId.ToString();
                iconImage.sprite = GetIconSpriteById(general.UnitId);
                costText.text = GetEnergyCostById(general.UnitId).ToString();
            }
            levelText.text = GetValueFromStats("level");
        }

        public string GetValueFromStats(string statName)
        {
            var stat = nftData.BasicStats.FirstOrDefault(s => s.StatName.Equals(statName, StringComparison.OrdinalIgnoreCase));
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

        public GameObject GetPrefabById(int cardId)
        {
            return UnityDB.FindAsset<CardSettingsAsset>(NFTManager.Instance.m_GameplaySettings.Settings.AllCards[cardId].Id).prevUIPrefab;
        }

        public void SelectCard()
        {
            Debug.Log($"SelectCard called on {tokenId}");
            IsSelected = true;
            unitNameText.color = Color.green;
            NFTTransferUI.Instance?.HandleCardSelected(this);
            NFTUpgradeUI.Instance?.HandleCardSelected(this);
        }

        public void DeselectCard()
        {
            IsSelected = false;
            unitNameText.color = Color.white;
        }

        public void UpdateStats(int level, int hp, int damage)
        {
            var levelStat = nftData.BasicStats.FirstOrDefault(s => s.StatName.Equals("level", StringComparison.OrdinalIgnoreCase));
            if (levelStat != null) levelStat.StatValue = level;

            var hpStat = nftData.BasicStats.FirstOrDefault(s => s.StatName.Equals("health", StringComparison.OrdinalIgnoreCase));
            if (hpStat != null) hpStat.StatValue = hp;

            var damageStat = nftData.BasicStats.FirstOrDefault(s => s.StatName.Equals("damage", StringComparison.OrdinalIgnoreCase));
            if (damageStat != null) damageStat.StatValue = damage;

            // Update UI elements
            levelText.text = level.ToString();
            var hpText = GetComponentsInChildren<TMP_Text>().FirstOrDefault(t => t.name == "HealthText");
            if (hpText != null) hpText.text = hp.ToString();
            var damageText = GetComponentsInChildren<TMP_Text>().FirstOrDefault(t => t.name == "DamageText");
            if (damageText != null) damageText.text = damage.ToString();
        }
    }
}