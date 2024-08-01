using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class AsyncLocalStorage
{
    // Save data asynchronously
    public static async Task SaveDataAsync(string key, string jsonData)
    {
        try
        {
            string path = GetFilePath(key);
            using (StreamWriter writer = new StreamWriter(path))
            {
                await writer.WriteAsync(jsonData);
                Debug.Log($"[AsyncLocalStorage] Successfully saved data for key: {key} at path: {path}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AsyncLocalStorage] Error saving data for key: {key}. Exception: {ex.Message}");
        }
    }

    // Load data asynchronously
    public static async Task<string> LoadDataAsync(string key)
    {
        string path = GetFilePath(key);
        try
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string jsonData = await reader.ReadToEndAsync();
                    Debug.Log($"[AsyncLocalStorage] Successfully loaded data for key: {key} from path: {path}");
                    return jsonData;
                }
            }
            else
            {
                Debug.LogWarning($"[AsyncLocalStorage] No data found for key: {key} at path: {path}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AsyncLocalStorage] Error loading data for key: {key}. Exception: {ex.Message}");
            return null;
        }
    }

    // Delete data
    public static void DeleteData(string key)
    {
        string path = GetFilePath(key);
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[AsyncLocalStorage] Successfully deleted data for key: {key} at path: {path}");
            }
            else
            {
                Debug.LogWarning($"[AsyncLocalStorage] No data found to delete for key: {key} at path: {path}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AsyncLocalStorage] Error deleting data for key: {key}. Exception: {ex.Message}");
        }
    }

    // Get file path
    private static string GetFilePath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key + ".json");
    }
}
