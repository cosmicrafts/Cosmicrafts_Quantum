namespace TowerRush
{
	using UnityEngine;
	using System.Collections.Generic;
	using Quantum;
	using TowerRush.Core;

	public struct MenuCardInfo
	{
		public AssetRefCardSettings CardSettings;
		public byte                 Level;
		public bool                 InDeck;
	}

	public class UIChangeCards : UIView
	{
		// CONFIGURATION

		[SerializeField] UIMenuCard m_CardPrefab;

		// PUBLIC MEMBERS

		public List<MenuCardInfo> CardSelection { get; set; } = new List<MenuCardInfo>();

		// PRIVATE MEMBERS

		private UIElementCache<UIMenuCard> m_Cards;

		// PUBLIC METHODS

		public void SetData(MenuCardInfo[] cards, AssetRefCardSettings[] allCards)
		{
			for (int idx = 0, count = allCards.Length; idx < count; idx++)
			{
				var settings  = allCards[idx];
				var infoIndex = cards.FindIndex(obj => obj.CardSettings == settings);

				if (infoIndex >= 0)
				{
					var cardInfo = cards[infoIndex];
					m_Cards[idx].SetData(settings, cardInfo.Level, cardInfo.InDeck);
				}
				else
				{
					m_Cards[idx].SetData(settings, 1, false);
				}
			}

			m_Cards.HideAll(allCards.Length);
		}

		// UIView INTERFACE

		protected override void OnClosed()
		{
			base.OnClosed();

			CardSelection.Clear();

			foreach (var card in m_Cards)
			{
				if (card.gameObject.activeSelf == false)
					continue;

				CardSelection.Add(card.GetCardInfo());
			}
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			m_Cards = new UIElementCache<UIMenuCard>(m_CardPrefab, 8);
		}
	}
}
