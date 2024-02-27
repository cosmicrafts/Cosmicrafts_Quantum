using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NFTMetadataHandler : MonoBehaviour
{
    // UI Elements
    public TMP_Text metadataText;

    // Sets and displays agnostic NFT metadata
    public void SetNFTMetadata(Dictionary<string, object> metadata)
    {
        string metadataAsString = MetadataDictionaryToString(metadata);
        metadataText.text = metadataAsString;
    }

    // Converts metadata dictionary to string for display
    private string MetadataDictionaryToString(Dictionary<string, object> metadata)
    {
        string result = "";
        foreach (var item in metadata)
        {
            result += $"{item.Key}: {item.Value.ToString()}\n";
        }
        return result;
    }
}
