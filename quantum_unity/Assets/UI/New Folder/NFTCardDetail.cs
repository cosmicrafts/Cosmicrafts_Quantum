using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
 * This is a detailed view of the selected card on the deck collection menu
 * Shows more data and a 3D preview of the selected card
 * Gets the base references and funcions of UICard code
 */
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
    //References the object model of the selected card
    public GameObject Model;
    //References the mesh renderer model of the selected card
    public MeshRenderer ModelRender;
    //References the mesh filter model of the selected card
    public MeshFilter ModelFilter;

    /*
    //The text references of specific card stats
    public TMP_Text Txt_HP;
    public TMP_Text Txt_Shield;
    public TMP_Text Txt_Dmg;

    //The images bars references of specific card stats
    public Image Bar_HP;
    public Image Bar_Shield;
    public Image Bar_Dmg;
    */
    
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
                UpdateUI(updatedData); // Assuming you have an UpdateUI method in NFTCardDetail
            }
        }
    }
    

    //The particles reference for skills cards
    GameObject CurrentObjPrev;

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
            //factionIcon.text = general.Faction;
            SetFactionIcon(general.Faction);
            unitClassText.text = general.Class;
            rarityText.text = general.Rarity.ToString();
            skinsText.text = general.SkinsText;
            iconImage.sprite = GetIconSpriteById(general.Icon);
        }
        levelText.text = GetValueFromStats("level");
        healthText.text = GetValueFromStats("health");
        damageText.text = GetValueFromStats("damage");
        skillsText.text = string.Join(", ", nftData.Skills.Select(s => $"{s.SkillName}: {s.SkillValue}"));
        skinsText.text = string.Join("\n", nftData.Skins.Select(s => $"{s.SkinName} - {s.SkinDescription}"));
    }

    private void SetFactionIcon(string factionText)
    {
       // Debug.Log($"Attempting to set faction icon for: {factionText}");
        int index = -1;

        // Your switch statement...

        if (index >= 0 && index < factionSprites.Length)
        {
            Debug.Log($"Setting faction sprite at index: {index}");
            factionImage.sprite = factionSprites[index];
        }
        else
        {
            Debug.LogWarning($"Could not find a matching faction sprite for: {factionText}");
        }
    }

    public void UpdateUI(NFTData nftData) {
    SetNFTData(nftData); // This method already exists in your code
    // You can add more UI update logic here if necessary
    }


    
    //Sets the UI data from a selected card from NFT data
    /*
    public override void SetData(NFTCard data)
    {
        //Set the basic properties
        Data = data;
        IsSelected = false;
        IsSkill = (NFTClass)data.EntType == NFTClass.Skill;

        //Set the name, description and cost of the card
        Txt_Name.text = Lang.GetEntityName(data.KeyId);
        Txt_Details.text = Lang.GetEntityDescription(data.KeyId);
        Txt_Cost.text = data.EnergyCost.ToString();

        //Deletes the last particules preview if exist
        if (CurrentObjPrev != null)
        {
            Destroy(CurrentObjPrev);
        }

        //Types
        if (IsSkill)
        {
            //SKILLS
            SpellCard SkillPrefab = ResourcesServices.LoadCardPrefab(data.KeyId, IsSkill).GetComponent<SpellCard>();
            CurrentObjPrev = Instantiate(SkillPrefab.PreviewEffect, Model.transform.position, Quaternion.identity);
            Model.SetActive(false);
            Txt_HP.transform.parent.gameObject.SetActive(false);
            Txt_Shield.transform.parent.gameObject.SetActive(false);
            Txt_Dmg.transform.parent.gameObject.SetActive(false);
            Txt_Type.text = Lang.GetText("mn_skill");
        } else
        {
            Debug.Log("Show Preview Model");
            //UNITS
            Model.SetActive(true);
            GameObject UnitPrefab = ResourcesServices.LoadCardPrefab(data.KeyId, IsSkill);
            //UnitPrefab.GetComponent<GameCharacter>().CanMove = false;
            CurrentObjPrev = Instantiate(UnitPrefab, Model.transform);
            
            //ModelFilter.mesh = UnitPrefab.UnitMesh.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
            //ModelRender.material = UnitPrefab.UnitMesh.transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
            
            Txt_HP.transform.parent.gameObject.SetActive(true);
            Txt_Shield.transform.parent.gameObject.SetActive(true);
            Txt_Dmg.transform.parent.gameObject.SetActive(true);
            
            Debug.Log("3");

            NFTsUnit unitdata = data as NFTsUnit;
            Txt_HP.text = unitdata.HitPoints.ToString();
            Bar_HP.fillAmount = (float)unitdata.HitPoints / 200f;
            Txt_Shield.text = unitdata.Shield.ToString();
            Bar_Shield.fillAmount = (float)unitdata.Shield / 200f;
            Txt_Dmg.text = unitdata.Dammage.ToString();
            Bar_Dmg.fillAmount = (float)unitdata.Dammage / 100f;
            Txt_Type.text = Lang.GetText(unitdata.EntType == (int)NFTClass.Station ? "mn_station" : "mn_ship");
        }
    }
    */
    
    
    
}
