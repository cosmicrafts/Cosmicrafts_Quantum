using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cosmicrafts.Data
{
    public class NFTCardDetail : NFTCard
    {
        [Header("CardDetailSlots")]
        public TMP_Text descriptionText;
        public TMP_Text unitClassText;
        public TMP_Text rarityText;
        public TMP_Text healthText;
        public TMP_Text damageText;
        public TMP_Text skillsText;
        public TMP_Text skinsText;
        public TMP_Text tokenIdText;
        public Image factionImage;
        public Sprite[] factionSprites;

        [Header("ModelRender")]
        private GameObject CurrentObjPrev;
        public GameObject PlaceToInstancePrev;

        void OnEnable()
        {
            NFTManager.OnMetadataUpdated += OnNFTMetadataUpdated;
        }

        void OnDisable()
        {
            NFTManager.OnMetadataUpdated -= OnNFTMetadataUpdated;
        }

        void OnNFTMetadataUpdated(string tokenId)
        {
            if (this.tokenId == tokenId)
            {
                // Fetch the updated data from NFTManager and update the UI
                NFTData updatedData = NFTManager.Instance.GetNFTDataById(tokenId);
                if (updatedData != null)
                {
                    UpdateUI(updatedData);
                }
            }
        }

        public override void SetNFTData(NFTData nftData)
        {
            this.nftData = nftData;
            tokenId = nftData.TokenId;
            iconImage.sprite = GetIconSpriteById(nftData.General.FirstOrDefault()?.Icon ?? 0);
            tokenIdText.text = tokenId;

            if (nftData == null)
            {
                Debug.LogWarning("NFTData not assigned.");
                return;
            }

            var general = nftData.General.FirstOrDefault();

            if (general != null)
            {
                unitNameText.text = general.Name;
                descriptionText.text = general.Description;
                SetFactionIcon(general.Faction);
                unitClassText.text = general.Class;
                rarityText.text = general.Rarity.ToString();
                iconImage.sprite = GetIconSpriteById(general.Icon);
                costText.text = GetEnergyCostById(general.UnitId).ToString();

                if (CurrentObjPrev) { Destroy(CurrentObjPrev); }
                CurrentObjPrev = Instantiate(GetPrefabById(general.UnitId), PlaceToInstancePrev.transform);
            }
            UpdateStatsUI();
        }

        public void UpdateStatsUI()
        {
            if (nftData == null)
            {
                Debug.LogWarning("NFTData is null when updating stats UI.");
                return;
            }

            levelText.text = GetValueFromStats("level");
            healthText.text = GetValueFromStats("health");
            damageText.text = GetValueFromStats("damage");
            skillsText.text = string.Join(", ", nftData.Skills.Select(s => $"{s.SkillName}: {s.SkillValue}"));
            skinsText.text = string.Join("\n", nftData.Skins.Select(s => $"{s.SkinName} - {s.SkinDescription}"));

            Debug.Log($"Updated NFTCardDetail: Level={levelText.text}, Health={healthText.text}, Damage={damageText.text}");
        }

        private void SetFactionIcon(string factionText)
        {
            int index = -1;

            switch (factionText.ToLower())
            {
                case "faction1":
                    index = 0;
                    break;
                case "faction2":
                    index = 1;
                    break;
                    // Add other cases as necessary
            }

            if (index >= 0 && index < factionSprites.Length)
            {
                factionImage.sprite = factionSprites[index];
            }
            else
            {
                Debug.LogWarning($"Could not find a matching faction sprite for: {factionText}");
            }
        }

        public void UpdateUI(NFTData updatedData)
        {
            Debug.Log("Updating UI with new NFTData");
            SetNFTData(updatedData);
            UpdateStatsUI();
        }
    }
}