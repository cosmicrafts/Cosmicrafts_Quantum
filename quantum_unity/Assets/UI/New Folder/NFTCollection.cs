using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cosmicrafts.Data
{
    public class NFTCollection : MonoBehaviour
    {
        //UI deck of the player
        public NFTCard[] Deck = new NFTCard[6];
        //UI default card collection
        public NFTCard NftCardPrefab;
        //UI selected card preview
        public NFTCardDetail CardPreview;
        //UI dragging card reference
        public UICardDrag DragIcon;

        //Current UI card selected
        [HideInInspector]
        public NFTCard CurrentSelected;
        //Current dragging card
        private NFTCard DragingCard;
        //Current mouse over card
        private NFTCard EnterCard;

        public List<NFTData> AllNFTDatas = null;

        private PlayerData playerData;

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

        async void Start()
        {
            playerData = await AsyncDataManager.LoadPlayerDataAsync();
            RefreshCollection();
        }

        void RemoveNFTFromCollection(string tokenId)
        {
            // Remove the NFT from AllNFTDatas.
            AllNFTDatas.RemoveAll(nft => nft.TokenId == tokenId);

            // Check if the transferred NFT is in the deck.
            if (playerData.DeckNFTsKeyIds.Contains(tokenId))
            {
                // Remove the NFT from the deck.
                playerData.DeckNFTsKeyIds.Remove(tokenId);

                // Attempt to fill the deck if there are less than 6 NFTs.
                while (playerData.DeckNFTsKeyIds.Count < 6 && AllNFTDatas.Count > playerData.DeckNFTsKeyIds.Count)
                {
                    // Find the first NFT that's not already in the deck.
                    NFTData nextNFT = AllNFTDatas.FirstOrDefault(nft => !playerData.DeckNFTsKeyIds.Contains(nft.TokenId));
                    if (nextNFT != null)
                    {
                        // Add the next available NFT to the deck.
                        playerData.DeckNFTsKeyIds.Add(nextNFT.TokenId);
                    }
                    else
                    {
                        // No more NFTs available to add to the deck.
                        break;
                    }
                }

                // Save the updated deck.
                SavePlayerData();
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

            // Update player data with the new deck configuration.
            playerData.DeckNFTsKeyIds = deckNFTs.Select(nft => nft.TokenId).ToList();
            SavePlayerData();
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
            if (EnterCard != null && DragingCard != null)
            {
                NFTData todeck = DragingCard.nftData;
                NFTData tocol = EnterCard.nftData;

                EnterCard.SetNFTData(todeck);
                EnterCard.animator.Play("DeckChange", -1, 0f);

                DragingCard.SetNFTData(tocol);
                DragingCard.animator.Play("DeckChange", -1, 0f);
            }

            DragingCard.iconImage.enabled = true;
            DragingCard = null;
            DragIcon.gameObject.SetActive(false);

            playerData.DeckNFTsKeyIds.Clear();
            for (int i = 0; i < Deck.Length; i++)
            {
                playerData.DeckNFTsKeyIds.Add(Deck[i].TokenId);
            }

            SavePlayerData();
        }

        //Mouse over enter to deck
        public void DeckEnterDrop(NFTCard card)
        {
            EnterCard = card;
        }

        //Mouse over exit from deck
        public void ClearEnterDrop(NFTCard card)
        {
            EnterCard = null;
        }

        private async void SavePlayerData()
        {
            await AsyncDataManager.SavePlayerDataAsync(playerData);
        }
    }
}
