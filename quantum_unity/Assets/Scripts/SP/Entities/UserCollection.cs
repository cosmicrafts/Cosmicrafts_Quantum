namespace CosmicraftsSP {
    using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
* Here we save and manages the player NFTs
*/
public class UserCollection
{
    //The user collection is ready to use
    bool DeckReady;

    //All user decks
    public Dictionary<Factions, List<NFTsCard>> Decks;
    //String saved deck ID

    [Serializable]
    public class SavedKeyIds
    {
        public List<String> SpiSavedKeyIds = new List<string>(); 
        public List<String> AllSavedKeyIds = new List<string>();
        public List<String> WebSavedKeyIds = new List<string>(); 
    }
    public SavedKeyIds savedKeyIds = new SavedKeyIds();
    
    //Current selected deck
    public List<NFTsCard> Deck;

    //All user cards collection
    public List<NFTsCard> Cards;

    //All user characters
    public List<NFTsCharacter> Characters;

    //Default character (for testing)
    public NFTsCharacter DefaultCharacter;
    public NFTsCharacter DefaultCharacter2;
    public NFTsCharacter DefaultCharacter3;
    public NFTsCharacter DefaultCharacter4;
    public NFTsCharacter DefaultCharacter5;
    public NFTsCharacter DefaultCharacter6;

    //Init default variables and data structures
    public void InitCollection()
    {
        DeckReady = false;
        Decks = new Dictionary<Factions, List<NFTsCard>>();
        Cards = new List<NFTsCard>();
        Characters = new List<NFTsCharacter>();
       
        DefaultCharacter = new NFTsCharacter()
        {
            ID = 1,
            Name = "Alpha-9",
            IconSprite = ResourcesServices.LoadCharacterIcon("Chr_1"),
            Faction = (int) Factions.Webe,
            LocalID = 1,
            EntType = (int) NFTClass.Character,
            Level = 10
        };
        DefaultCharacter2 = new NFTsCharacter()
        {
            ID = 2,
            Name = "Epsilon",
            IconSprite = ResourcesServices.LoadCharacterIcon("Chr_2"),
            Faction = (int) Factions.Webe,
            LocalID = 2,
            EntType = (int) NFTClass.Character,
            Level = 10
        };
        DefaultCharacter3 = new NFTsCharacter()
        {
            ID = 3,
            Name = "Elara Dawnstrider",
            IconSprite = ResourcesServices.LoadCharacterIcon("Chr_3"),
            Faction = (int) Factions.Alliance,
            LocalID = 3,
            EntType = (int) NFTClass.Character,
            Level = 10
        };
        DefaultCharacter4 = new NFTsCharacter()
        {
            ID = 4,
            Name = "Talon Frostbane",
            IconSprite = ResourcesServices.LoadCharacterIcon("Chr_4"),
            Faction = (int) Factions.Alliance,
            LocalID = 4,
            EntType = (int) NFTClass.Character,
            Level = 10
        };
        DefaultCharacter5 = new NFTsCharacter()
        {
            ID = 5,
            Name = "Echo Nodeblast",
            IconSprite = ResourcesServices.LoadCharacterIcon("Chr_5"),
            Faction = (int) Factions.Spirats,
            LocalID = 5,
            EntType = (int) NFTClass.Character,
            Level = 10
        };
        DefaultCharacter6 = new NFTsCharacter()
        {
            ID = 6,
            Name = "Lucius Darkstorm",
            IconSprite = ResourcesServices.LoadCharacterIcon("Chr_6"),
            Faction = (int) Factions.Spirats,
            LocalID = 6,
            EntType = (int) NFTClass.Character,
            Level = 10
        };
        
    }
    
    public void ChangeDeckFaction(NFTsCharacter nFTsCharacter)
    {
        Deck = Decks[(Factions) nFTsCharacter.Faction];
    }

    public void SetCharacters(string jsonList)
    {
        Characters = JsonConvert.DeserializeObject<List<NFTsCharacter>>(jsonList);
    }

    public void SetSpellsCards(string jsonList)
    {
        Cards.AddRange(JsonConvert.DeserializeObject<List<NFTsSpell>>(jsonList));
    }

    public void SetUnitCards(string jsonList)
    {
        Cards.AddRange(JsonConvert.DeserializeObject<List<NFTsUnit>>(jsonList));
    }

    //Set decks when the collection data is complete
public void InitDecks()
{
    // Check if the decks are already complete
    if (DeckReady || Decks == null || !Decks.Any())
        return;

    // Init Cards
    foreach (NFTsCard card in Cards)
    {
        card.TypePrefix = NFTsCollection.NFTsPrefix[card.EntType];
        card.FactionPrefix = NFTsCollection.NFTsFactionsPrefixs[(Factions)card.Faction];
    }

    // Distinct values
    Characters = Characters.GroupBy(g => g.KeyId).Select(s => s.First()).ToList();
    Cards = Cards.GroupBy(g => g.KeyId).Select(s => s.First()).ToList();

    // Set Factions Decks
    foreach (Factions faction in (Factions[])Enum.GetValues(typeof(Factions)))
    {
        if (faction == Factions.Neutral)
            continue;

        List<NFTsCard> factionCards = Cards.Where(f => (Factions)f.Faction == faction || (Factions)f.Faction == Factions.Neutral).ToList();

        if (factionCards.Count >= 8)
        {
            if (!Decks.ContainsKey(faction))
            {
                Decks.Add(faction, factionCards.Take(8).ToList());
            }
            else
            {
                Decks[faction] = factionCards.Take(8).ToList();
            }
        }
    }

    // Set current deck only if there are decks available
    if (Decks.Any())
    {
        Deck = Decks[Decks.Keys.First()];
    }

    // Decks are ready
    DeckReady = true;
}

    //Build a testing collection
public void AddUnitsAndCharactersDefault(List<ShipsDataBase> listAlliance = null, List<ShipsDataBase> listSpirats = null, List<ShipsDataBase> listWebe = null)
{
    // CHARACTERS
    Characters.Add(DefaultCharacter);
    Characters.Add(DefaultCharacter2);
    Characters.Add(DefaultCharacter3);
    Characters.Add(DefaultCharacter4);
    Characters.Add(DefaultCharacter5);
    Characters.Add(DefaultCharacter6);

    // ALLIANCE
    if (listAlliance != null)
    {
        foreach (ShipsDataBase shipsDataBase in listAlliance)
        {
            Cards.Add(shipsDataBase.ToNFTCard());
        }
    }

    if (listWebe != null)
    {
        foreach (ShipsDataBase shipsDataBase in listWebe)
        {
            Cards.Add(shipsDataBase.ToNFTCard());
        }
    }

    if (listSpirats != null)
    {
        foreach (ShipsDataBase shipsDataBase in listSpirats)
        {
            Cards.Add(shipsDataBase.ToNFTCard());
        }
    }

    Cards.Add(new NFTsSpell()
    {
        EnergyCost = 10,
        IconSprite = ResourcesServices.LoadCardIcon("H_COM_1"),
        Rarity = 5,
        Faction = (int)Factions.Neutral,
        EntType = (int)NFTClass.Skill,
        LocalID = 1
    });

    // CURRENT DECK
    InitDecks();
}

    //Find a card from key
    public NFTsCard FindCard(string NFTkey)
    {
        return Cards.FirstOrDefault(f => f.KeyId == NFTkey);
    }

    //Check if player has a deck for some faction
    public bool FactionDeckExist(Factions faction)
    {
        return Decks.ContainsKey(faction);
    }
}
}