using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;
using CanisterPK.testnft.Models;
using EdjCase.ICP.Candid.Models;
using Candid;

public class NFTManager : MonoBehaviour
{
    public TMP_Text nftListText;
    public GameObject nftPrefab;
    public Transform nftDisplayContainer;

    async void Start()
    {
        nftPrefab.SetActive(false);
        await FetchOwnedNFTs();
    }

    async Task FetchOwnedNFTs()
    {
        string userPrincipalId = GlobalGameData.Instance.GetUserData().WalletId;
        var account = new Account(Principal.FromText(userPrincipalId), null);
        var tokens = await GetOwnedNFTs(account);
        nftListText.text = $"Owned NFTs: {tokens.Count}";
        foreach (var tokenId in tokens)
        {
            var instance = Instantiate(nftPrefab, nftDisplayContainer);
            instance.SetActive(true);
        }
    }

    async Task<List<UnboundedUInt>> GetOwnedNFTs(Account account)
    {
        var nftListResult = await CandidApiManager.Instance.testnft.Icrc7TokensOf(account);
        return nftListResult.Tag == TokensOfResultTag.Ok ? nftListResult.AsOk() : null;
    }
}
