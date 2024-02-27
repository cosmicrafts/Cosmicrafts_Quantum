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
    public GameObject nftPrefab; // Asegúrate de asignar el prefab en el Inspector

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
            var account = new Account(Principal.FromText(userPrincipalId), null);
            var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);

            if (nftListResult.Tag == TokensOfResultTag.Ok)
            {
                var tokens = nftListResult.AsOk();
                foreach (var tokenId in tokens)
                {
                    var metadataResult = await CandidApiManager.Instance.testnft.Icrc7Metadata(tokenId);

                    if (metadataResult.Tag == MetadataResultTag.Ok)
{
    var metadataDict = metadataResult.AsOk(); // Esto debería darte Dictionary<string, Metadata>

    foreach (KeyValuePair<string, Metadata> entry in metadataDict)
    {
        string key = entry.Key;
        Metadata metadataValue = entry.Value;

        // Ahora debes verificar el tipo de cada metadataValue
        switch (metadataValue.Tag)
        {
            case MetadataTag.Blob:
                // Procesar Blob
                break;
            case MetadataTag.Int:
                // Procesar Int
                break;
            case MetadataTag.Nat:
                // Procesar Nat
                break;
            case MetadataTag.Text:
                string textValue = metadataValue.AsText(); // Correcto uso de AsText()
                // Utiliza 'textValue' como necesites, por ejemplo:
                Debug.Log($"{key}: {textValue}");
                break;
        }
    }
}

                }
            }
            else
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
