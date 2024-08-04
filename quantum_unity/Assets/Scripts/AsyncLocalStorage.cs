using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AsyncLocalStorage
{
    // A 32-byte (256-bit) encryption key for AES
    private static readonly byte[] EncryptionKey = Encoding.UTF8.GetBytes("S5hVAGhIl9vH9sgYO5zemf6XNMyIak5L");

    // Save data asynchronously with encryption
    public static async Task SaveDataAsync(string key, string data)
    {
        try
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = EncryptionKey;
                aesAlg.GenerateIV();
                string iv = Convert.ToBase64String(aesAlg.IV);

                string encryptedData = Encrypt(data, aesAlg.Key, aesAlg.IV);

                // Save the encrypted data with IV
                string dataToSave = $"{iv}:{encryptedData}";
                string path = GetFilePath(key);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteAsync(dataToSave);
                    Debug.Log($"[AsyncLocalStorage] Successfully saved encrypted data for key: {key} at path: {path}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AsyncLocalStorage] Error saving encrypted data for key: {key}. Exception: {ex.Message}");
        }
    }

    // Load data asynchronously with decryption
    public static async Task<string> LoadDataAsync(string key)
    {
        string path = GetFilePath(key);
        try
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string dataToLoad = await reader.ReadToEndAsync();

                    // Ensure data is in the expected format with one colon separator
                    int colonIndex = dataToLoad.IndexOf(':');
                    if (colonIndex == -1 || colonIndex >= dataToLoad.Length - 1)
                    {
                        Debug.LogError($"[AsyncLocalStorage] Invalid data format for key: {key}. Data: {dataToLoad}");
                        throw new Exception("Invalid data format. Missing IV or encrypted data.");
                    }

                    string iv = dataToLoad.Substring(0, colonIndex);
                    string encryptedData = dataToLoad.Substring(colonIndex + 1);

                    if (string.IsNullOrEmpty(iv) || string.IsNullOrEmpty(encryptedData))
                    {
                        Debug.LogError($"[AsyncLocalStorage] Invalid data components for key: {key}. IV: {iv}, Data: {encryptedData}");
                        throw new Exception("Invalid data format. IV or encrypted data is empty.");
                    }

                    string data = Decrypt(encryptedData, EncryptionKey, Convert.FromBase64String(iv));
                    Debug.Log($"[AsyncLocalStorage] Successfully loaded and decrypted data for key: {key} from path: {path}");
                    return data;
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
            Debug.LogError($"[AsyncLocalStorage] Error loading or decrypting data for key: {key}. Exception: {ex.Message}");
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

    // Encryption method with IV
    private static string Encrypt(string plainText, byte[] key, byte[] iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    // Decryption method with IV
    private static string Decrypt(string cipherText, byte[] key, byte[] iv)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}
