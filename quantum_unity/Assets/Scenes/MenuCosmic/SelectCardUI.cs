using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Quantum;
using TowerRush;
using TowerRush.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCardUI : MonoBehaviour
{
    // PRIVATE MEMBERS
    [SerializeField] GameplaySettingsAsset  m_GameplaySettings;
    
    private MenuCardInfo[]                  m_Cards;
    private DictionaryFile                  m_CardsSaveFile;
    

    [SerializeField] UIMenuCard m_CardPrefab;

    // PUBLIC MEMBERS
    public List<MenuCardInfo> CardSelection { get; set; } = new List<MenuCardInfo>();

    // PRIVATE MEMBERS
    private UIElementCache<UIMenuCard> m_CardsUI;

    // PUBLIC METHODS
    private void Awake()
    {
	    LoadCardSelection();
	    m_CardsUI = new UIElementCache<UIMenuCard>(m_CardPrefab, 8);
	    SetData(m_Cards, m_GameplaySettings.Settings.AllCards);
    }
    
    private void LoadCardSelection()
    {
    	if (m_CardsSaveFile == null)
    	{
    		m_CardsSaveFile = DictionaryFile.Load("Cards");
    	}

    	var stringData = m_CardsSaveFile.GetString("Selection", "");
        
    	if (stringData.Length == 0)
    	{
	        Debug.Log("[SELECT CARD UI] StringLenght= 0");
    		var allCards = m_GameplaySettings.Settings.AllCards;
    		var count    = allCards.Length;
    		m_Cards      = new MenuCardInfo[count];

    		for (int idx = 0; idx < count; idx++)
    		{
    			m_Cards[idx] = new MenuCardInfo
    			{
    				CardSettings = allCards[idx],
    				Level        = 1,
    				InDeck       = true,
                    Hp           = 100f,
	                Dmg          = 20f,
    			};
    		}

    		SaveCardSelection();
    		return;
    	}

        try
        {
	        var data = System.Convert.FromBase64String(stringData);

	        using (var stream = new MemoryStream(data))
	        using (var reader = new BinaryReader(stream))
	        {
		        var count = reader.ReadInt32();
		        m_Cards = new MenuCardInfo[count];

		        for (int idx = 0; idx < count; idx++)
		        {
			        var id     = new AssetGuid(reader.ReadInt64());
			        var level  = reader.ReadByte();
			        var inDeck = reader.ReadBoolean();
			        var hp = reader.ReadSingle();
			        var dmg = reader.ReadSingle();

			        if (UnityDB.FindAsset(id) != null)
			        {
				        m_Cards[idx] = new MenuCardInfo
				        {
					        CardSettings = new AssetRefCardSettings { Id = id },
					        Level        = level,
					        InDeck       = inDeck,
					        Hp           = hp, 
					        Dmg          = dmg, 
				        };
			        }
		        }
	        }
        }
        catch (Exception e)
        {
	        Debug.Log(e);
	        Debug.Log("SELECT CARD UI");
	        m_CardsSaveFile.Clear();
	        LoadCardSelection();
        }
    	
        
    }
    
    private void SaveCardSelection()
    {
	    using (var stream = new MemoryStream())
	    using (var writer = new BinaryWriter(stream))
	    {
		    var count = m_Cards.Length;
		    writer.Write(count);

		    for (int idx = 0; idx < count; idx++)
		    {
			    var card = m_Cards[idx];
			    writer.Write(card.CardSettings.Id.Value);
			    writer.Write(card.Level);
			    writer.Write(card.InDeck);
			    writer.Write(card.Hp);
			    writer.Write(card.Dmg);
		    }


		    writer.Flush();

		    var data       = stream.ToArray();
		    var stringData = System.Convert.ToBase64String(data);

		    m_CardsSaveFile.SetString("Selection", stringData);
		    m_CardsSaveFile.Save();
	    }
    }
    public void SetData(MenuCardInfo[] cards, AssetRefCardSettings[] allCards)
    {
    	for (int idx = 0, count = allCards.Length; idx < count; idx++)
    	{
    		var settings  = allCards[idx];
    		var infoIndex = cards.FindIndex(obj => obj.CardSettings == settings);

    		if (infoIndex >= 0)
    		{
    			var cardInfo = cards[infoIndex];
                m_CardsUI[idx].SetData(settings, cardInfo, cardInfo.InDeck);
    		}
    		else
    		{
	            m_CardsUI[idx].SetData(settings, new MenuCardInfo(), false);
    		}
    	}

        m_CardsUI.HideAll(allCards.Length);
    }

   
    public void OnChange()
    {
	    CardSelection.Clear();

    	foreach (var card in m_CardsUI)
    	{
    		if (card.gameObject.activeSelf == false)
    			continue;

    		CardSelection.Add(card.GetCardInfo());
    	}
        
        m_Cards = CardSelection.ToArray();
        SaveCardSelection();
    }

   
    	

}
