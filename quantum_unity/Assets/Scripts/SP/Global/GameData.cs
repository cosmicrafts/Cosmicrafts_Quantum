namespace CosmicraftsSP {
    using System.Linq;
using UnityEngine;
/*
    Here we save and manage the global data of the game like
    player's data, game configuration, game language, current game mode, etc.
    This data always exist
 */

//Enum Game Modes Types
public enum Match
{
    testing,
    tutorial,
    bots,
    multi
}
//Enum Plataforms Types
public enum Plataform
{
    Web,
    Pc
}

//Types of NFTS
public enum NFTClass
{
    Character,
    Skill,
    Station,
    Ship
}
public class GameData
{
    //Save the selected game mode
    public Match CurrentMatch = Match.multi;

  
    //Save if the player is the multiplayer master
    public bool ImMaster = false;
    //Save if the game is running in Debug mode
    public bool DebugMode = false;
    //Save the current plataform
    public Plataform CurrentPlataform = Plataform.Pc;
    //Save if the player data is loaded and ready
    public bool DataReady = false;
    //Save the game configuration
    Config config;
    //Save the basic data of the player
    User PlayerUser;
    //Save the basic data of Vs player (for multiplayer)
    UserGeneral VsPlayerUser;
    //Save the data progression of the player
    UserProgress PlayerProgress;
    //Save the NFTs collection data of the player
    UserCollection PlayerCollection;
    //Save the current character selected by the player
    NFTsCharacter PlayerCharacter;
    //Save all the NFTs types in the game
    NFTsCollection NFTCollection;
    //Save te current region of the game
    public string Region = "LAN";

    //Returns the current game config, if is null, create a new one
    public Config GetConfig()
    {
        if (config == null)
        {
            config = new Config();
        }

        return config;
    }
    //Set the current game config
    public void SetConfig(Config newconfig)
    {
        config = newconfig;
    }
    //Get the current game language
    public Language GetGameLanguage()
    {
        return (Language)GetConfig().language;
    }
    //Set the basic player data
    public void SetUser(User user)
    {
        PlayerUser = user;
    }
    //Set the basic Vs player data
    public void SetVsUser(UserGeneral user)
    {
        VsPlayerUser = user;
    }
    //Set the player's progression
    public void SetUserProgress(UserProgress userprogress)
    {
        PlayerProgress = userprogress;
    }
    //Set the NFTs collection data for the player
    public void SetUserCollection(UserCollection userCollection)
    {
        PlayerCollection = userCollection;
    }
    //Set the current player character
    public void SetUserCharacter(int NFTid)
    {
        NFTsCharacter character = GetUserCollection().Characters.FirstOrDefault(f => f.ID == NFTid);
        if (character == null)
        {
            PlayerCharacter = PlayerCollection.Characters[0];
        } else
        {
            PlayerCharacter = character;
        }
        PlayerCollection.ChangeDeckFaction(PlayerCharacter);
    }
    //Set the global NFTs Collection
    public void SetNFTsCollection(NFTsCollection nFTsCollection)
    {
        NFTCollection = nFTsCollection;
    }
    //Returns the basic Player data
    public User GetUserData()
    {
        if (PlayerUser == null)
        {
            
            PlayerUser = new User() {NikeName = "Tester", WalletId = "TestWalletId", 
                Avatar = PlayerPrefs.HasKey("savedAvatar") ? PlayerPrefs.GetInt("savedAvatar") : 1 };
        }
        else{
            
            PlayerUser.Avatar = PlayerPrefs.HasKey("savedAvatar") ? PlayerPrefs.GetInt("savedAvatar") : 1 ;
            
        }

        return PlayerUser;
    }
    //Returns the basic Vs Player data
    public UserGeneral GetVsUser()
    {
        return VsPlayerUser;
    }
    //Create a data resume of the player, used to send it to the network
    public UserGeneral BuildMyProfileHasVS()
    {
        if (PlayerUser == null || PlayerProgress == null)
            return null;

        return new UserGeneral()
        {
            WalletId = PlayerUser.WalletId,
            NikeName = PlayerUser.NikeName,
            Level = PlayerProgress.GetLevel(),
            Xp = PlayerProgress.GetXp(),
            Avatar = PlayerUser.Avatar,
            CharacterNFTId = PlayerCharacter.ID,
            DeckNFTsId = PlayerCollection.Deck.Select(s => s.ID).ToList()
        };
    }
    //Returns the progression of the player
    public UserProgress GetUserProgress()
    {
        if (PlayerProgress == null)
        {
            PlayerProgress = new UserProgress();
            PlayerProgress.InitValues(new Progress { BattlePoints = 0, Xp = 0, Level = 1 });
        }

        return PlayerProgress;
    }
    //Returns the NFTs data of the player
    public UserCollection GetUserCollection()
    {
        if (PlayerCollection == null)
        {
            PlayerCollection = new UserCollection();
            PlayerCollection.InitCollection();
        }

        return PlayerCollection;
    }
    //Returns the current player character
    public NFTsCharacter GetUserCharacter()
    {
        if (PlayerCharacter == null)
        {
            var Characters = GetUserCollection().Characters;
            if (Characters == null)
            {
                PlayerCharacter = GetUserCollection().DefaultCharacter;
            } else if (Characters.Count == 0)
            {
                PlayerCharacter = GetUserCollection().DefaultCharacter;
            } else if (PlayerPrefs.HasKey("CharacterSaved"))
            {
                PlayerCharacter = Characters.FirstOrDefault(f=>f.ID == PlayerPrefs.GetInt("CharacterSaved") );
            }
            else
            {
                PlayerCharacter = Characters[0];
            }
        }

        return PlayerCharacter;
    }
    //Returns the Global NFTs collection
    public NFTsCollection GetNFTsCollection()
    {
        if (NFTCollection == null)
        {
            NFTCollection = new NFTsCollection();
            NFTCollection.InitGlobalCollection();
        }

        return NFTCollection;
    }
    //Change the language of the game
    public void ChangeLang(Language newlang)
    {
        Lang.SetLang(newlang);

        config.language = (int)newlang;
        SaveData.SaveGameConfig();
    }
    //Returns if the player information is loaded and ready
    public bool UserIsInit()
    {
        return PlayerUser != null;
    }
    //Clear the player information
    public void ClearUser()
    {
        PlayerUser = null;
        PlayerProgress = null;
        PlayerCharacter = null;
        PlayerCollection = null;
        ImMaster = false;
    }
    //Returns the current version of the game
    public string GetVersion()
    {
        return Application.version;
    }
    //Returns if the game is running on web and if is a production build
    public bool IsProductionWeb()
    {
        return CurrentPlataform == Plataform.Web && !DebugMode;
    }
}
}