using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class NFTCollection : MonoBehaviour
{
    //UI deck of the player
    public NFTCard[] Deck = new NFTCard[6];
    //UI default card collection
    public NFTCard NftCardPrefab;
    //UI selected card preview
    public NFTCardDetail CardPreview;
    //UI draging card reference
    public UICardDrag DragIcon;

    //Current ui card selected
    [HideInInspector]
    public NFTCard CurrentSelected;
    //Current draging card
    NFTCard DragingCard;
    //Current mouse over card
    NFTCard EnterCard;
    
    public List<NFTData> AllNFTDatas = null;
    
  
    //Updates the UI collection with the current data and filters
    public void RefreshCollection()
    {
        
        if (AllNFTDatas == null) { return; }
        
        //Clean the collection section
        foreach (Transform child in NftCardPrefab.transform.parent)
        {
            if (child.gameObject != NftCardPrefab.gameObject)
            {
                Destroy(child.gameObject);
            }
        }
        
        List<String> listSavedKeys = new List<string>();
        
        if (PlayerPrefs.HasKey("savedList"))
        {
            listSavedKeys = JsonUtility.FromJson<List<string>>(PlayerPrefs.GetString("savedList"));
            Debug.Log(JsonUtility.ToJson(listSavedKeys));
        }
        
        if (!PlayerPrefs.HasKey("savedList") || listSavedKeys.Count < 6 ||
            !listSavedKeys.All(savedKey => AllNFTDatas.Any(nftData => nftData.TokenId == savedKey)))
        {
            listSavedKeys = AllNFTDatas.Take(6).Select(nft => nft.TokenId).ToList();
        }

        Debug.Log(JsonUtility.ToJson(listSavedKeys));
        
        for (int i = 0; i < listSavedKeys.Count; i++)
        {
            Deck[i].SetNFTData( AllNFTDatas.Find(nftData => nftData.TokenId == listSavedKeys[i] ) );
        }
        
        foreach (NFTData nftData in AllNFTDatas)
        {
            if (listSavedKeys.Contains(nftData.TokenId)) { continue; }
            
            NFTCard card = Instantiate(NftCardPrefab.gameObject, NftCardPrefab.transform.parent).GetComponent<NFTCard>();
            card.SetNFTData(nftData);
        }
        
        NftCardPrefab.gameObject.SetActive(false);

        /*//Select the first card
        if (AllNFTSCards.Count > 0)
        {
            SelectCard(AllCards[0]);
        }*/
        
    }

    //Selects a card
    public void SelectCard(NFTCard card)
    {
        Debug.Log("Select");
        if (card.DeckSlot == -1 && EnterCard != null)
        {
            return;
        }

        if (CurrentSelected != null)
        {
            CurrentSelected.DeselectCard();
        }
        CurrentSelected = card;
        card.SelectCard();
        CardPreview.SetNFTData(card.nftData);
        CardPreview.gameObject.SetActive(true);
    }
    //Drags a card
    public void DragCard(NFTCard card)
    {
        Debug.Log("Drag");
        DragingCard = card;
        DragIcon.gameObject.SetActive(true);
        DragIcon.Icon.sprite = card.iconImage.sprite;
        card.iconImage.enabled = false;
        DragIcon.transform.position = Input.mousePosition;
    }
    //Drops a card
    public void DropCard()
    {
        Debug.Log("Drop 1 ");
        if (EnterCard!= null && DragingCard != null)
        {
            NFTData todeck = DragingCard.nftData;
            NFTData tocol = EnterCard.nftData;
            
         
            Deck[EnterCard.DeckSlot] = DragingCard;

            EnterCard.SetNFTData(todeck);
            EnterCard.animator.Play("DeckChange", -1, 0f);
           
            DragingCard.SetNFTData(tocol);
            DragingCard.animator.Play("DeckChange", -1, 0f);
         
        }
       
        Debug.Log("Drop8");
        DragingCard.iconImage.enabled = true;
        DragingCard = null;
        DragIcon.gameObject.SetActive(false);

        List<String> listSavedKeys = new List<string>();

        for (int i = 0; i < Deck.Length; i++) {
            listSavedKeys.Add( Deck[i].TokenId );
        }
        
        PlayerPrefs.SetString("savedList", JsonUtility.ToJson(listSavedKeys));
        Debug.Log(JsonUtility.ToJson(listSavedKeys));
    }

    //Mouse over enter to deck
    public void DeckEnterDrop(NFTCard card) { EnterCard = card;
        Debug.Log("Entro");}
    //Mouse over exit from deck
    public void ClearEnterDrop(NFTCard card)
    {
        EnterCard = null; 
        Debug.Log("Salio");
    }

    
}
