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
    
    [Serializable]
    public class SavedKeys
    {
        public List<string> listSavedKeys = new List<string>();
    }

        void OnEnable()
    {
        NFTManager.OnMetadataUpdated += UpdateCollection;
        NFTManager.OnNFTTransferred += RemoveNFTFromCollection;
    }

    void OnDisable()
    {
        NFTManager.OnMetadataUpdated -= UpdateCollection;
        NFTManager.OnNFTTransferred -= RemoveNFTFromCollection;
    }

    void RemoveNFTFromCollection(string tokenId)
    {
        // Remove the NFT from AllNFTDatas.
        AllNFTDatas.RemoveAll(nft => nft.TokenId == tokenId);

        // Load the saved keys to manage the deck.
        SavedKeys savedKeys = PlayerPrefs.HasKey("savedKeys") ? JsonUtility.FromJson<SavedKeys>(PlayerPrefs.GetString("savedKeys")) : new SavedKeys();

        // Check if the transferred NFT is in the deck.
        if (savedKeys.listSavedKeys.Contains(tokenId))
        {
            // Remove the NFT from the deck.
            savedKeys.listSavedKeys.Remove(tokenId);

            // Attempt to fill the deck if there are less than 6 NFTs.
            while (savedKeys.listSavedKeys.Count < 6 && AllNFTDatas.Count > savedKeys.listSavedKeys.Count)
            {
                // Find the first NFT that's not already in the deck.
                NFTData nextNFT = AllNFTDatas.FirstOrDefault(nft => !savedKeys.listSavedKeys.Contains(nft.TokenId));
                if (nextNFT != null)
                {
                    // Add the next available NFT to the deck.
                    savedKeys.listSavedKeys.Add(nextNFT.TokenId);
                }
                else
                {
                    // No more NFTs available to add to the deck.
                    break;
                }
            }

            // Save the updated deck.
            PlayerPrefs.SetString("savedKeys", JsonUtility.ToJson(savedKeys));
        }

        // Refresh the collection and deck to reflect the change.
        RefreshCollection();
    }

    void UpdateCollection(string tokenId)
    {
        RefreshCollection();
    }
  
    //Updates the UI collection with the current data and filters
   public void RefreshCollection()
    {
        if (AllNFTDatas == null || AllNFTDatas.Count == 0)
        {
            Debug.Log("No NFT Data available for collection.");
            return;
        }

        // Clean up existing UI elements.
        foreach (Transform child in NftCardPrefab.transform.parent)
        {
            if (child.gameObject != NftCardPrefab.gameObject)
            {
                Destroy(child.gameObject);
            }
        }

        // Repopulate the deck with the first available NFTs from AllNFTDatas.
        var deckNFTs = AllNFTDatas.Take(6).ToList();
        for (int i = 0; i < deckNFTs.Count; i++)
        {
            Deck[i].SetNFTData(deckNFTs[i]);
            Deck[i].gameObject.SetActive(true);
        }

        // Hide unused deck slots if any.
        for (int i = deckNFTs.Count; i < Deck.Length; i++)
        {
            Deck[i].gameObject.SetActive(false);
        }

        // Display the rest of the collection, excluding those in the deck.
        foreach (NFTData nftData in AllNFTDatas.Skip(6))
        {
            NFTCard card = Instantiate(NftCardPrefab.gameObject, NftCardPrefab.transform.parent).GetComponent<NFTCard>();
            card.SetNFTData(nftData);
            card.gameObject.SetActive(true);
        }

        NftCardPrefab.gameObject.SetActive(false); // Hide the prefab template.

        // Update PlayerPrefs with the new deck configuration.
        SavedKeys savedKeys = new SavedKeys { listSavedKeys = deckNFTs.Select(nft => nft.TokenId).ToList() };
        PlayerPrefs.SetString("savedKeys", JsonUtility.ToJson(savedKeys));
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
        
        CardPreview.SetNFTData(card.nftData);
        CardPreview.iconImage.sprite = card.iconImage.sprite;
        CardPreview.gameObject.SetActive(true);
        
        card.SelectCard();
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
      //  Debug.Log("Drop 1 ");
        if (EnterCard!= null && DragingCard != null)
        {
            NFTData todeck = DragingCard.nftData;
            NFTData tocol = EnterCard.nftData;
            
            //Deck[EnterCard.DeckSlot] = DragingCard;

            EnterCard.SetNFTData(todeck);
            EnterCard.animator.Play("DeckChange", -1, 0f);
           
            DragingCard.SetNFTData(tocol);
            DragingCard.animator.Play("DeckChange", -1, 0f);
         
        }
       
       // Debug.Log("Drop8");
        DragingCard.iconImage.enabled = true;
        DragingCard = null;
        DragIcon.gameObject.SetActive(false);
        
        SavedKeys savedKeys = new SavedKeys();
        
        for (int i = 0; i < Deck.Length; i++) {
            savedKeys.listSavedKeys.Add( Deck[i].TokenId );
        }
        
        PlayerPrefs.SetString("savedKeys", JsonUtility.ToJson(savedKeys));
      //  Debug.Log(JsonUtility.ToJson(savedKeys));
    }

    //Mouse over enter to deck
    public void DeckEnterDrop(NFTCard card) { EnterCard = card;
      //  Debug.Log("[NFTCollection]Swap In");
      }
    //Mouse over exit from deck
    public void ClearEnterDrop(NFTCard card)
    {
        EnterCard = null; 
      //  Debug.Log("[NFTCollection]Swap Out");
    }

    
}
