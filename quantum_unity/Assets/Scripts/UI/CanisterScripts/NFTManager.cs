using UnityEngine;
using TMPro;
using Candid;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;

public class NFTManager : MonoBehaviour
{
    public TMP_Text nftListText;
    public GameObject nftPrefab;

    async void Start()
    {
        await FetchOwnedNFTs();
    }

    async Task FetchOwnedNFTs()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        if (string.IsNullOrEmpty(userPrincipalId))
        {
            LogError("User principal ID not found.");
            return;
        }

        var account = new Account(Principal.FromText(userPrincipalId), null);
        try
        {
            var tokens = await GetOwnedNFTs(account);
            if (tokens != null && tokens.Count > 0)
            {
                nftListText.text = $"Owned NFTs: {tokens.Count}";
                foreach (var tokenId in tokens)
                {
                    await LogNFTMetadata(tokenId);
                }
            }
            else
            {
                LogError("No NFTs found.");
            }
        }
        catch (System.Exception ex)
        {
            LogError($"Exception occurred - {ex.Message}");
        }
    }

    async Task<List<UnboundedUInt>> GetOwnedNFTs(Account account)
    {
        var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);
        return nftListResult.Tag == TokensOfResultTag.Ok ? nftListResult.AsOk() : null;
    }

    async Task LogNFTMetadata(UnboundedUInt tokenId)
    {
        var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);
        if (metadataResult.Tag != MetadataResultTag.Ok) return;

        var metadataDict = metadataResult.AsOk();

        // Convert the dictionary to JSON string
        string jsonMetadata = JsonConvert.SerializeObject(metadataDict, Formatting.Indented);
        Debug.Log($"Metadata for Token ID {tokenId}: {jsonMetadata}");

        // Optional: Parse the JSON string back to an object if necessary for further processing
        // var parsedMetadata = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonMetadata);
    }

    void LogError(string message)
    {
        Debug.LogError($"FetchOwnedNFTs: {message}");
        nftListText.text = message;
    }
}
