namespace CosmicraftsSP {
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class TestNft : MonoBehaviour
{

    string urlSelectedNFTX;
   List<Sprite> selectedNFTXImages = new List<Sprite>();
    // Start is called before the first frame update
    [SerializeField]
    GameObject nftTestPanel;
    [SerializeField]
    Image spriteX;

    void Start()
    {
        nftTestPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       

    }
   
    public void JSAnvilConnect()
    {
        GameNetwork.JSAnvilConnect();
          nftTestPanel.SetActive(true);
    }
    public void GetNFT(string url)
    {
      
        StartCoroutine(GetPlayerNFTX(url));
        GameNetwork.JSGetAnvilNfts(url);
        
    }
    public IEnumerator GetPlayerNFTX(string u)
    {
        urlSelectedNFTX = u;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(u);
        yield return www.SendWebRequest();
        Texture2D webTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        spriteX.sprite = Sprite.Create(webTexture, new Rect(0.0f, 0.0f, webTexture.width, webTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

}
}