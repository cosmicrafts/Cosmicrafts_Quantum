using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cosmicrafts.Managers;

namespace Cosmicrafts.Data
{
    public class NFTCollection : MonoBehaviour
    {
        // UI deck of the player
        public NFTCard[] Deck = new NFTCard[6];
        // UI default card collection
        public NFTCard NftCardPrefab;
        // UI selected card preview
        public NFTCardDetail CardPreview;
        // UI dragging card reference
        public UICardDrag DragIcon;
        private PlayerData playerData;

        // Current UI card selected
        [HideInInspector]
        public NFTCard CurrentSelected;
        // Current dragging card
        NFTCard DragingCard;
        // Current mouse over card
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
            var savedKeys = playerData.HasKey("savedKeys") 
                ? new SavedKeys { listSavedKeys = playerData.GetKeyList("savedKeys") } 
                : new SavedKeys();

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
                playerData.SetKeyList("savedKeys", savedKeys.listSavedKeys);
            }

            // Refresh the collection and deck to reflect the change.
            RefreshCollection();
        }

        void UpdateCollection(string tokenId)
        {
            RefreshCollection();
        }

        // Updates the UI collection with the current data and filters
public void RefreshCollection()
{
    if (AllNFTDatas == null || AllNFTDatas.Count == 0)
    {
        Debug.LogWarning("No NFTs available for collection.");
        return;
    }

    // Filter the NFTs to include only those from the "Unit" category
    var unitNFTs = AllNFTDatas.Where(nft => nft.Category.TagName == "Unit").ToList();

    if (unitNFTs.Count == 0)
    {
        Debug.LogWarning("No Unit NFTs available for the deck.");
        return;
    }

    // Initialize the deck with the first 6 Unit NFTs
    var initialDeckNFTs = unitNFTs.Take(6).ToList();

    // Clear the existing keys in player data
    GameDataManager.Instance.playerData.DeckNFTsKeyIds.Clear();

    // Save the first 6 Unit NFTs to player data
    foreach (var nftData in initialDeckNFTs)
    {
        GameDataManager.Instance.playerData.DeckNFTsKeyIds.Add(nftData.TokenId);
    }

    // Save the updated player data
    GameDataManager.Instance.SavePlayerData();

    // Load the deck with the selected Unit NFTs
    for (int i = 0; i < Deck.Length; i++)
    {
        if (i < initialDeckNFTs.Count)
        {
            Deck[i].SetNFTData(initialDeckNFTs[i]);
            Deck[i].gameObject.SetActive(true);
        }
        else
        {
            Deck[i].gameObject.SetActive(false);
        }
    }

    // Populate the collection with the remaining Unit NFTs
    foreach (var nft in unitNFTs)
    {
        if (!GameDataManager.Instance.playerData.DeckNFTsKeyIds.Contains(nft.TokenId))
        {
            NFTCard card = Instantiate(NftCardPrefab, NftCardPrefab.transform.parent).GetComponent<NFTCard>();
            card.SetNFTData(nft);
            card.gameObject.SetActive(true);
        }
    }

    NftCardPrefab.gameObject.SetActive(false); // Hide the prefab template
}

        // Selects a card
        public void SelectCard(NFTCard card)
        {
            Debug.Log("Select");
            if (card.DeckSlot == -1 && EnterCard != null && EnterCard.DeckSlot != -1)
        {
            // Only block if EnterCard is actually from the deck
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

        // Drags a card
        public void DragCard(NFTCard card)
        {
            Debug.Log("Drag");
            DragingCard = card;
            DragIcon.gameObject.SetActive(true);
            DragIcon.Icon.sprite = card.iconImage.sprite;
            card.iconImage.enabled = false;
            DragIcon.transform.position = Input.mousePosition;
        }

        // Drops a card
// Drops a card
public void DropCard()
{
    if (EnterCard != null && DragingCard != null)
    {
        NFTData todeck = DragingCard.nftData;
        NFTData tocol = EnterCard.nftData;

        EnterCard.SetNFTData(todeck);
        EnterCard.animator.Play("Highlighted", -1, 0f);

        DragingCard.SetNFTData(tocol);
        DragingCard.animator.Play("Highlighted", -1, 0f);

        // Update the deck keys in PlayerData using GameDataManager
        UpdateDeckKeys();
    }

    DragingCard.iconImage.enabled = true;
    DragingCard = null;
    DragIcon.gameObject.SetActive(false);
}

private void UpdateDeckKeys()
{
    // Clear the existing keys in player data
    GameDataManager.Instance.playerData.DeckNFTsKeyIds.Clear();

    // Iterate over the deck and update the keys in player data
    for (int i = 0; i < Deck.Length; i++)
    {
        if (Deck[i] != null && Deck[i].nftData != null)
        {
            GameDataManager.Instance.playerData.DeckNFTsKeyIds.Add(Deck[i].nftData.TokenId);
        }
    }

    // Save the updated player data
    GameDataManager.Instance.SavePlayerData();
}


        // Mouse over enter to deck
        public void DeckEnterDrop(NFTCard card)
        {
            EnterCard = card;
        }

        // Mouse over exit from deck
        public void ClearEnterDrop(NFTCard card)
        {
            EnterCard = null;
        }
    }
}
