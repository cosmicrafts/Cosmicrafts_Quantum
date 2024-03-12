using System.Collections;
using System.Collections.Generic;
using Candid;
using CanisterPK.testicrc1.Models;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;

public class PruebaCanisters : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text text2;
    public TMP_Text text3;

    public async void getICPInfo()
    {
        var Account = new CanisterPK.testicrc1.Models.Account1(
            Principal.FromText(GlobalGameData.Instance.GetUserData().WalletId),
            new Account1.SubaccountInfo()
            );
        var infoICP = await CandidApiManager.Instance.testicrc1.Icrc1BalanceOf(Account);

        text.text = infoICP.ToString();
    }
    
    public async void sendICPInfo()
    {

        var fee = await CandidApiManager.Instance.testicrc1.Icrc1Fee();
        
        var transfer = new CanisterPK.testicrc1.Models.TransferArgs(
            UnboundedUInt.FromBigInteger(1000000),
            null,
            new OptionalValue<UnboundedUInt>(fee),
            null, 
            null, 
            new Account(Principal.FromText("5dnhr-udp4w-qtu2u-lbz46-wvyqh-oj2fn-uddhe-7d76y-h3az3-c4veb-6qe"), null)
        );
        
        var infoTransfer = await CandidApiManager.Instance.testicrc1.Icrc1Transfer(transfer);
        
        if (infoTransfer.Tag == TransferResultTag.Err)
        {
            Debug.Log( JsonUtility.ToJson(infoTransfer.Value));
            Debug.Log(infoTransfer.AsErr().Value.ToString());
        }
        text2.text = infoTransfer.Value.ToString();
        text3.text = infoTransfer.Tag.ToString();
    }


}
