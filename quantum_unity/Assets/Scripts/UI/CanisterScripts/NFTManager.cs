using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Candid;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

public class NFTManager : MonoBehaviour
{
    public TMP_Text nftListText;

    async void Start()
    {
        await FetchOwnedNFTs();
    }

    async Task FetchOwnedNFTs()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;

        if (string.IsNullOrEmpty(userPrincipalId))
        {
            Debug.LogError("No user principal ID found.");
            nftListText.text = "Error: User principal ID not found.";
            return;
        }

        try
        {
            Debug.Log("Fetching NFT tokens...");
            var account = new Account(Principal.FromText(userPrincipalId), null);
            var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);

            if (nftListResult.Tag == TokensOfResultTag.Ok)
            {
                var tokens = nftListResult.AsOk();
                string displayText = "Owned NFTs:\n";

                foreach (var tokenId in tokens)
                {
                    Debug.Log($"Fetching metadata for token ID: {tokenId}");
                    var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);

                    if (metadataResult.Tag == MetadataResultTag.Ok)
                    {
                        var metadata = metadataResult.AsOk();

                        // Serialize the metadata to a JSON string for readability
                        string jsonMetadata = JsonConvert.SerializeObject(metadata, Formatting.Indented);

                        // Deserialize the JSON metadata into a dictionary
                        var metadataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonMetadata);

                        // Add the token ID to the display text
                        displayText += $"Token ID: {tokenId}\n";

                        // Add each metadata field and value to the display text
                        foreach (var kvp in metadataDict)
                        {
                            displayText += $"{kvp.Key}: {kvp.Value}\n";
                            Debug.Log($"Metadata for token ID {tokenId}:\n{displayText}");
                        }

                        displayText += "\n";
                    }
                    else
                    {
                        displayText += $"Token ID: {tokenId}\nFailed to fetch metadata.\n\n";
                    }
                }

                nftListText.text = displayText;
            }
            else if (nftListResult.Tag == TokensOfResultTag.Err)
            {
                Debug.LogError("Error fetching NFT tokens.");
                nftListText.text = "Error fetching NFTs.";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception occurred: {ex}");
            nftListText.text = "Error fetching NFTs.";
        }
    }
}
