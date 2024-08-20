namespace CosmicraftsSP {
    using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
 * This is the UI collection menu controller
 * Initialize all UI collection window (cards, decks and previews)
 * Manage the changes on the deck and only shows cards of the current character faction or neutral faction
 */

//Types of cards sorting
public enum CardOrder
{
    Name,
    Cost,
    Rarity
}

public class UICollection : MonoBehaviour
{
    //UI deck of the player
    public UICard[] Deck = new UICard[8];
    //UI default card collection
    public UICard CardCollection;
    //UI selected card preview
    public UICardDetail CardPreview;

    //Current ui card selected
    [HideInInspector]
    public UICard CurrentSelected;

    //Current draging card
    UICard DragingCard;
    //Current mouse over card
    UICard EnterCard;

    //All NFTs cards
    List<NFTsCard> AvCards;
    //Current showing cards
    List<UICard> AllCards;

    //Player collection and character data reference
    UserCollection PlayerCollection;
    NFTsCharacter PlayerCharacter;

    //UI draging card reference
    public UICardDrag DragIcon;

    //UI card sorting drop down
    public Dropdown DD_OrderBy;

    //List of cards types (filter)
    List<NFTClass> ClassFilter;
    //Dictionary of key cards (nfts) and cards names
    Dictionary<string, string> UnitNames;

    //Filtering and sorting variables
    public bool FilterShips { get; set; }
    public bool FilterStations { get; set; }
    public bool FilterSkills { get; set; }
    public string FilterSearch { get; set; }
    public int OrderBy { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //initialize the filters and sorts default vaules
        FilterShips = true;
        FilterStations = true;
        FilterSkills = true;

        OrderBy = 0;
        FilterSearch = string.Empty;
        ClassFilter = new List<NFTClass>();
        ClassFilter.Add(NFTClass.Ship);
        ClassFilter.Add(NFTClass.Station);
        ClassFilter.Add(NFTClass.Skill);

        DD_OrderBy.ClearOptions();
        DD_OrderBy.AddOptions(new List<Dropdown.OptionData>() {
            new Dropdown.OptionData { text = Lang.GetText("mn_name")},
            new Dropdown.OptionData { text = Lang.GetText("mn_cost")},
            new Dropdown.OptionData { text = Lang.GetText("mn_rarity")}
        });

        //Gets the user collection data
        PlayerCollection = GlobalManager.GMD.GetUserCollection();
        //Refresh de UI
        RefreshCollection();
    }

    private void OnEnable()
    {
        //Refresh de UI
        RefreshCollection();
    }

    //Updates the UI collection with the current data and filters
    void RefreshCollection()
    {
        //Debug.Log("refresh colection");
        //If the collection data does't exit, exit the function
        if (PlayerCollection == null)
            return;

        //Gets the current player character
        PlayerCharacter = GlobalManager.GMD.GetUserCharacter();

        //Clean the collection section
        foreach (Transform child in CardCollection.transform.parent)
        {
            if (child.gameObject != CardCollection.gameObject)
            {
                Destroy(child.gameObject);
            }
        }

        //Get the collection cards deppendig the cuurent faction of the current player character
        AvCards = PlayerCollection.Cards.Where(
            f => f.Faction == PlayerCharacter.Faction || (Factions)f.Faction == Factions.Neutral).ToList();
        //Initialize the dictionary with the keys and names of the cards
        UnitNames = new Dictionary<string, string>();
        foreach(NFTsCard nFTsCard in AvCards)
        {
            UnitNames.Add(nFTsCard.KeyId, Lang.GetEntityName(nFTsCard.KeyId));
        }
        
        //Sort the data by name (default sort)
        List<NFTsCard> Sorted = AvCards.OrderBy(f => UnitNames[f.KeyId]).ToList();
        

        //instantiate the ui cards with the NFTs data
        AllCards = new List<UICard>();
        foreach (NFTsCard nFTsCard in Sorted)
        {
            if (PlayerCollection.Deck.Contains(nFTsCard))
            {
                continue;
            }
            UICard card = Instantiate(CardCollection.gameObject, CardCollection.transform.parent).GetComponent<UICard>();
            AllCards.Add(card);
            card.SetData(nFTsCard);
        }

        //Initialize the UI Deck data
        for (int i = 0; i < PlayerCollection.Deck.Count; i++)
        {
            Deck[i].SetData(PlayerCollection.Deck[i]);
        }

        //Hide the default ui card
        CardCollection.gameObject.SetActive(false);

        //Select the first card
        if (AllCards.Count > 0)
        {
           // SelectCard(AllCards[0]);
        }

        //Apply the sort and filters values
        SortAndFilterCollection();
    }

    //Selects a card
    public void SelectCard(UICard card)
    {
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
        CardPreview.SetData(card.GetData());
        CardPreview.gameObject.SetActive(true);
    }
    
    //Drags a card
    public void DragCard(UICard card)
    {
        DragingCard = card;
        DragIcon.gameObject.SetActive(true);
        DragIcon.Icon.sprite = card.Icon.sprite;
        card.Icon.enabled = false;
        DragIcon.transform.position = Input.mousePosition;
    }

    //Drops a card
    public void DropCard()
    {
        if (EnterCard!= null && DragingCard != null)
        {
            NFTsCard todeck = DragingCard.GetData();
            NFTsCard tocol = EnterCard.GetData();
            PlayerCollection.Deck[EnterCard.DeckSlot] = todeck;

            EnterCard.SetData(todeck);
            
            EnterCard.animator.Play("DeckChange", -1, 0f);
            DragingCard.SetData(tocol);
            DragingCard.animator.Play("DeckChange", -1, 0f);
            
            SortAndFilterCollection();
        }
        DragingCard.Icon.enabled = true;
        DragingCard = null;
        DragIcon.gameObject.SetActive(false);

        List<String> listSavedKeys = new List<string>();

        for (int i = 0; i < 8; i++) {
            listSavedKeys.Add( PlayerCollection.Deck[i].KeyId );
            
        }
        
        if( (Factions)PlayerCharacter.Faction == Factions.Alliance ) {PlayerCollection.savedKeyIds.AllSavedKeyIds = listSavedKeys; }
        if( (Factions)PlayerCharacter.Faction == Factions.Spirats  ) {PlayerCollection.savedKeyIds.SpiSavedKeyIds = listSavedKeys; }
        if( (Factions)PlayerCharacter.Faction == Factions.Webe  )    {PlayerCollection.savedKeyIds.WebSavedKeyIds = listSavedKeys; }
        
        PlayerPrefs.SetString("savedKeyIds", JsonUtility.ToJson(PlayerCollection.savedKeyIds));
        Debug.Log(JsonUtility.ToJson(PlayerCollection.savedKeyIds));


    }

    //Mouse over enter to deck
    public void DeckEnterDrop(UICard card)
    {    
        EnterCard = card;
    }

    //Mouse over exit from deck
    public void ClearEnterDrop(UICard card)
    {
        EnterCard = null;
    }

    //Applay the filters and sort on the collection list
    public void SortAndFilterCollection()
    {
        ClassFilter.Clear();
        if (FilterShips)
            ClassFilter.Add(NFTClass.Ship);
        if (FilterStations)
            ClassFilter.Add(NFTClass.Station);
        if (FilterSkills)
            ClassFilter.Add(NFTClass.Skill);

        List<NFTsCard> Sorted;

        switch((CardOrder)OrderBy)
        {
            case CardOrder.Cost:
                {
                    Sorted = AvCards.OrderByDescending(f => f.EnergyCost).ToList();
                }
                break;
            case CardOrder.Rarity:
                {
                    Sorted = AvCards.OrderByDescending(f => f.Rarity).ToList();
                }
                break;
            default:
                {
                    Sorted = AvCards.OrderByDescending(f => UnitNames[f.KeyId]).ToList();
                }
                break;
        }

        for(int i = 0; i < Sorted.Count; i++)
        {
            UICard card = AllCards.FirstOrDefault(f => f.KeyName == Sorted[i].KeyId);
            if (card)
            {
                card.transform.SetSiblingIndex(0);
            }
        }

        foreach (UICard uICard in AllCards)
        {
            uICard.gameObject.SetActive(
                ClassFilter.Contains(uICard.TypeCard) 
                && (string.IsNullOrWhiteSpace(FilterSearch) || uICard.NameCard.ToLower().Contains(FilterSearch.ToLower()))
                );
        }
    }
}
}