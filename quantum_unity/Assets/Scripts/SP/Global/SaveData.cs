namespace CosmicraftsSP {
    using Newtonsoft.Json;
using UnityEngine;
/*
 * This is the local data controller
 * Is used to save information, like the game configuration, on the device
 */
public static class SaveData 
{
    //The path of the game configuration file
    public readonly static string fullPathConfig = Application.persistentDataPath + "/" + "Config";

    public static bool error = false;
    //Load the configuration data
    public static void LoadGameConfig()
    {
        SaveManager.Instance.Load<Config>(fullPathConfig, DataWasLoaded, false);
    }
    //This function is called when the configuration was loaded
    private static void DataWasLoaded(Config data, SaveResult result, string message)
    {
        //Check for some error or emptu data
        if (result == SaveResult.EmptyData || result == SaveResult.Error)
        {
            //Create a new game config and set it
            Config newConfig = new Config();
            GlobalManager.GMD.SetConfig(newConfig);
            //initialize the language
            Lang.InitLanguage((Language)newConfig.language);
            return;
        }
        //Success load
        if (result == SaveResult.Success)
        {
            //Set the configuration
            GlobalManager.GMD.SetConfig(data);
            //initialize the language
            Lang.InitLanguage((Language)data.language);
        }
    }
    //Save the current game configuration locally
    public static void SaveGameConfig()
    {
        SaveManager.Instance.Save(GlobalManager.GMD.GetConfig(), fullPathConfig, DataWasSaved, false);
        if (GlobalManager.GMD.IsProductionWeb())
        {
            GameNetwork.JSSavePlayerConfig(JsonConvert.SerializeObject(GlobalManager.GMD.GetConfig()));
        }
    }
    //This function is called when the configuration was saved
    private static void DataWasSaved(SaveResult result, string message)
    {
        error = result == SaveResult.Error;
    }
}
}