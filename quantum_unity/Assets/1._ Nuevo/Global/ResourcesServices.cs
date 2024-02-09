using System.Collections.Generic;
using UnityEngine;

/*
 * This is the game resources services
 * Returns assets from the resources folder
 */

public static class ResourcesServices
{
   
    
    /*
    //Global Bot Prefab
    public static GameObject LoadBot(int id, int dificulty)
    {
        //Debug.Log("Bot: " + $"Prefabs/Manager/BOTS/BOT_{id}_{dificulty}");
        return Resources.Load<GameObject>($"Prefabs/Manager/BOTS/BOT_{id}_{dificulty}");
        
    }
    //Name Bots Prefabs
    public static List<string> GetNameBots()
    {
        List<string> names = new List<string>(); 
        GameObject[] bots = Resources.LoadAll<GameObject>($"Prefabs/Manager/BOTS");

        foreach (var bot in bots)
        {
            //names.Add(bot.GetComponent<BotEnemy>().botName);
        }

        return names;
    }*/
    
    public static GameObject LoadLoadingPanel() { return Resources.Load<GameObject>($"CanvasLoading/CanvasLoading"); }
    public static GameObject LoadUIMatchLoading() { return Resources.Load<GameObject>($"MatchLoadingPanel"); }
    public static GameObject LoadGlobalManager() { return Resources.Load<GameObject>($"GlobalGameData"); }
    
    public static Sprite LoadAvatarUser(int id)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Icons/Avatars_Profile/avatar_{id}"));
    }
    
    
    
    
    //Returns a sprite avatar
    public static Sprite LoadAvatarIcon(int id)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Icons/Avatars/Avatar_{id}"));
    }
    
    //Returns a sprite character icon
    public static Sprite LoadCharacterIcon(string nftCharacterKey)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Characters/Ico_{nftCharacterKey}"));
    }
    //Returns a sprite card icon
    public static Sprite LoadCardIcon(string nftCardKey)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Icons/Cards/Ico_{nftCardKey}"));
    }
    //Returns the prefab of a spell or unit
    public static GameObject LoadCardPrefab(string key, bool isSkill)
    {
        string folder = isSkill ? "Skills" : "Units";
        Debug.Log($"Prefabs/{folder}/{key.Substring(2, 3)}/{key}");
        return Resources.Load<GameObject>($"Prefabs/{folder}/{key.Substring(2, 3)}/{key}");
    }
    //Returns the prefab base station from a faction
    public static GameObject LoadBaseStationPrefab(string nftCharacterKey)
    {
        return Resources.Load<GameObject>($"Prefabs/BaseStations/BS_{nftCharacterKey}");
    }
    //Returns the prefab of a character
    public static GameObject LoadCharacterPrefab(string key)
    {
        return Resources.Load<GameObject>($"Prefabs/Characters/{key}");
    }
    //Returns the sprite emblem of a character
    public static Sprite LoadCharacterEmblem(string nftCharacterKey)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Characters/Emblems/{nftCharacterKey}_Emb"));
    }
    public static Sprite LoadCharacterSkill(string nftCharacterKey)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Icons/Skills/{nftCharacterKey}"));
    }
    public static Sprite LoadCharacterStats(string nftCharacterKey)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Icons/Stats/{nftCharacterKey}"));
    }
    public static Sprite LoadCharacterBG(string nftCharacterKey)
    {
        return ValidateSprite(Resources.Load<Sprite>($"UI/Icons/BG/{nftCharacterKey}"));
    }
    public static Sprite ValidateSprite(Sprite sprite)
    {
        return sprite == null ? Resources.Load<Sprite>($"UI/Loading") : sprite;
    }
}
