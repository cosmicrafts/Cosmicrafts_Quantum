using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * This code shows the player data on the in-game UI player banner
 */

public class UIPlayerGameInfo : MonoBehaviour
{
    //Player UI stats and icons references
    public TMP_Text PlayerName;
    public TMP_Text WalletId;
    public TMP_Text Level;
    public Image XpBar;
    public Image Avatar;

    //Update the UI banner with the player data
    public void InitInfo(User user, UserProgress progress, NFTsCharacter character)
    {
       // WalletId.text = Utils.GetWalletIDShort(user.WalletId);
       // PlayerName.text = user.NikeName;
       // Level.text = $"{Lang.GetText("mn_lvl")} {progress.GetLevel()}";
       // XpBar.fillAmount = (float)progress.GetXp() / (float)progress.GetNextXpGoal();
       // Avatar.sprite = ResourcesServices.LoadAvatarUser(user.Avatar);
    }

    //Update the UI banner with a resume of some player data (for multiplayer)
    public void InitInfo(UserGeneral user)
    {
       // WalletId.text = Utils.GetWalletIDShort(user.WalletId);
       // PlayerName.text = user.NikeName;
       // Level.text = $"{Lang.GetText("mn_lvl")} {user.Level}";
        //XpBar.fillAmount = (float)user.Xp / (float)user.GetNextXpGoal();
        //Avatar.sprite = ResourcesServices.LoadAvatarUser(user.Avatar);
        //Avatar.sprite = ResourcesServices.LoadAvatarIcon(user.Avatar);
    }
}
