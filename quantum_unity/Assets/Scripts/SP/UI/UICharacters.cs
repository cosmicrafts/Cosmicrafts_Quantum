namespace CosmicraftsSP {
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UICharacters : MonoBehaviour
{
    //Player NFT collection reference
    public UserCollection PlayerCollection;
    //UI characters references
    List<UICharacter> AllCharacters;
    //Current player character
    UICharacter CurrentChar;
    //Default UI character reference
    public UICharacter DefaultUIChar;
    //Scroll bar reference (with the list of characters)
    public Scrollbar CharactersScroll;

    //UI Stats of the selected character
    public Image PreviewAvatar;
    public Image Emblem;
    //public TMP_Text CharName;
    //public TMP_Text CharName2;
    public UILang CharTitle;
    public UILang CharTitle2;
   // public TMP_Text CharDesc;
    public TMP_Text CharAka;
    public TMP_Text CharLvl;
    public TMP_Text CosRunsAv;
    public UILang[] SkillNames = new UILang[3];
    public UILang[] SkillDesc = new UILang[3];
    public Image[] skillImage = new Image[3];
    public Image statsImage;
    public Image BGImage;
    // Start is called before the first frame update
    void Start()
    {
        //Initialize the UI characters list
        AllCharacters = new List<UICharacter>();
        //Get the player collection data
        PlayerCollection = GlobalManager.GMD.GetUserCollection();
        //Get the current player character
        NFTsCharacter pCharacter = GlobalManager.GMD.GetUserCharacter();
        PlayerCollection.ChangeDeckFaction(pCharacter); //||||||| Agregue esta linea para Local Selección de caracter

        //Show the UI characters from the player collection characters data
        foreach (NFTsCharacter character in PlayerCollection.Characters.OrderByDescending(o => o.LocalID))
        {
            if (!PlayerCollection.FactionDeckExist((Factions)character.Faction))
                continue;

            UICharacter uichar = Instantiate(DefaultUIChar.gameObject, DefaultUIChar.transform.parent).GetComponent<UICharacter>();
            uichar.SetData(character);
            uichar.gameObject.SetActive(true);
            uichar.transform.SetAsFirstSibling();
            AllCharacters.Add(uichar);
        }

        //Sets the player selected character, if is null, select the first one of the list
        CurrentChar = AllCharacters.FirstOrDefault(f => f.GetData() == pCharacter);
        if (CurrentChar == null)
        {
            CurrentChar = AllCharacters[0];
        }
        //Update the UI of the selected character
        UpdateUIInfo();
    }

    //Selects and changes the current player´s character
    public void BtnSelectCharacter(UICharacter character)
    {
        CurrentChar.SetSelection(false);
        NFTsCharacter nFTsCharacter = character.GetData();
        CurrentChar = character;
        UpdateUIInfo();
        GlobalManager.GMD.SetUserCharacter(nFTsCharacter.ID);
        
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.Character);
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.CharacterName);
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.Description);
        UIMainMenu.Menu.RefreshProperty(PlayerProperty.Emblem);
        
        if (GlobalManager.GMD.IsProductionWeb())
        {
            GameNetwork.JSSavePlayerCharacter(CurrentChar.GetData().ID);
        }
        else
        {
            PlayerPrefs.SetInt("CharacterSaved", CurrentChar.GetData().ID);
        }
    }

    //Updates the UI info of the selected character
    public void UpdateUIInfo()
    {
        CurrentChar.SetSelection(true);
        string key = CurrentChar.GetData().KeyId;
        PreviewAvatar.sprite = CurrentChar.GetData().IconSprite;
        Emblem.sprite = ResourcesServices.LoadCharacterEmblem(key);
        /*CharName.text = Lang.GetEntityName(key);
        CharName2.text = Lang.GetEntityName(key);
        CharDesc.text = Lang.GetEntityDescription(key);*/
        CharTitle.ID = $"{key}_title"; CharTitle.SetMyText();
        CharTitle2.ID = $"{key}_title"; CharTitle2.SetMyText();
        CharAka.text = Lang.GetText($"{key}_aka");
        CharLvl.text = $"{Lang.GetText("mn_level")} 1";
        CosRunsAv.text = $"{Lang.GetText("mn_cr_left")} 0";
        for(int i=0; i<3; i++)
        {
            SkillNames[i].ID = $"{key}_skill_{i + 1}_name";  SkillNames[i].SetMyText();
            SkillDesc[i].ID  = $"{key}_skill_{i + 1}_description"; SkillDesc[i].SetMyText();
            skillImage[i].sprite = ResourcesServices.LoadCharacterSkill($"Skill_{key}_{i+1}");
        }
        statsImage.sprite = ResourcesServices.LoadCharacterStats($"Stats_{key}");
        BGImage.sprite = ResourcesServices.LoadCharacterBG($"BG_{key}");
    }

    //Scrolls to the right limit of the characters list
    public void GoLimit()
    {
        CharactersScroll.value = 1;
    }
}
}