using UnityEngine;

public class NFTItem : MonoBehaviour
{
    private NFTData nftData;

    public void SetNFTData(NFTData data)
    {
        this.nftData = data;
    }

    // Ensure there's a way to trigger this, like attaching it to a button click or another event
    public void OnClick()
    {
        NFTManager.Instance.UpdateNFTDisplay(nftData);
    }
}
