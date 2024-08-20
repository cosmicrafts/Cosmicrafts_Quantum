using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * This code represents a character instance on the characters menu
 * The instance has the icon and name UI references
 */

public class UICharacter : MonoBehaviour
{
    //NFT data source
    private NFTsCharacter Data;

    //UI Text and image references (for name and avatar)
    public Image MyAvatar;
    public TMP_Text MyName;

    //UI icon used when the character is selected
    public GameObject IconSelected;

    //Initialize the UI from a NFT character data
    public void SetData(NFTsCharacter data)
    {
        Data = data;
        MyAvatar.sprite = ResourcesServices.LoadAvatarIcon(Data.LocalID);
        MyName.text = Lang.GetEntityName(Data.KeyId);
    }

    //returns the nft character data
    public NFTsCharacter GetData()
    {
        return Data;
    }

    //Show or hide the selection icon
    public void SetSelection(bool selected)
    {
        IconSelected.SetActive(selected);
    }
}
