using System.Threading.Tasks;
using UnityEngine;
using Cosmicrafts.Data;

public static class AsyncDataManager
{
    private const string PlayerDataKey = "PlayerData";

    public static async Task SavePlayerDataAsync(PlayerData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);
        await AsyncLocalStorage.SaveDataAsync(PlayerDataKey, jsonData);
    }

    public static async Task<PlayerData> LoadPlayerDataAsync()
    {
        string jsonData = await AsyncLocalStorage.LoadDataAsync(PlayerDataKey);
        if (jsonData != null)
        {
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        return new PlayerData(); // Return a new instance if no data is found
    }
}
