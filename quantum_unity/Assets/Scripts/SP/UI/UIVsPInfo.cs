namespace CosmicraftsSP {
    using UnityEngine;
using UnityEngine.UI;

/*
 * This code shows a enemy player property on a UI element (UI component)
 * Is only used for multiplayer
 */

public class UIVsPInfo : MonoBehaviour
{
    //The enemy player property
    public PlayerProperty Property;

    //Load and show the property when begins
    void Start()
    {
        LoadProperty();
    }

    public void LoadProperty()
    {
        //Check if we have an enemy data
        if (GlobalManager.GMD.GetVersion() == null)
        {
            return;
        }

        //Show the selected property
        switch (Property)
        {
            case PlayerProperty.Name:
                {
                    UserGeneral user = GlobalManager.GMD.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = user.NikeName;
                }
                break;/*
            case PlayerProperty.WalletId:
                {
                    UserGeneral user = GlobalManager.GMD.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = Utils.GetWalletIDShort(user.WalletId);
                }*/
                break;
            case PlayerProperty.Level:
                {
                    UserGeneral user = GlobalManager.GMD.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{Lang.GetText("mn_lvl")} {user.Level}";
                }
                break;
            case PlayerProperty.Xp:
                {
                    UserGeneral user = GlobalManager.GMD.GetVsUser();
                    Text mytext = GetComponent<Text>();
                    mytext.text = $"{user.Xp} {Lang.GetText("mn_xp")}";
                }
                break;
            case PlayerProperty.Character:
                {
                    NFTsCharacter character = GameNetwork.GetVSnftCharacter();
                    //UserGeneral user = GlobalManager.GMD.GetVsUser();
                    if (character != null)
                    {
                        Image myimage = GetComponent<Image>();
                        myimage.sprite = ResourcesServices.LoadCharacterIcon(character.KeyId);
                    }
                }
                break;
            case PlayerProperty.Avatar:
                {
                    UserGeneral user = GlobalManager.GMD.GetVsUser();
                    Image myimage = GetComponent<Image>();
                    myimage.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
                }
                break;
        }
    }
}
}