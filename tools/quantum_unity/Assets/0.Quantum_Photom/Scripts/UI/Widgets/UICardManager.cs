namespace TowerRush
{
	using System.Collections.Generic;
	using TowerRush.Core;
	using UnityEngine;
	using UnityEngine.UI;
	using TMPro;
	using Quantum;
	using Photon.Deterministic;
	using System;

	using QuantumCardManager = Quantum.CardManager;

	public unsafe class UICardManager : MonoBehaviour
	{
		// CONFIGURATION

		[SerializeField] Slider          m_EnergyBarFluid;
		[SerializeField] Slider          m_EnergyBarDescrete;
		[SerializeField] TextMeshProUGUI m_EnergyText;
		[SerializeField] TextMeshProUGUI m_MaxEnergyText;
		[SerializeField] Image           m_EnergyTile;
		[SerializeField] UICard          m_CardPrefab;

		[SerializeField] TextMeshProUGUI m_NextCountdown;
		[SerializeField] UICard          m_NextCard;
		[SerializeField] CanvasGroup     m_NextCardGroup;

		[SerializeField] UICard          m_DragCard;

		// PRIVATE MEMBERS

		private CardManager     m_CardManager;

		private CardCallbacks   m_Callbacks;
		private List<UICard>    m_Cards = new List<UICard>();
		private int             m_PlayerRef;

		private Camera          m_CanvasCamera;
		private UICard          m_DraggingCard;

		// PUBLIC METHODS

		public void Initialize(CardManager cardManager, Camera canvasCamera)
		{
			m_CardManager  = cardManager;
			m_CanvasCamera = canvasCamera;

			QuantumCallback.Subscribe<CallbackGameStarted>(this, OnGameStarted);
			QuantumEvent.Subscribe<EventCardsChanged>(this, OnCardsChanged);

			m_Cards.Add(m_CardPrefab);

			for (int idx = 1; idx < QuantumCardManager.AVAILABLE_CARDS_COUNT; idx++)
			{
				m_Cards.Add(Instantiate(m_CardPrefab, m_CardPrefab.transform.parent));
			}

			for (int idx = 0; idx < QuantumCardManager.AVAILABLE_CARDS_COUNT; idx++)
			{
				m_Cards[idx].SetActive(false);
			}

			m_NextCard.SetActive(false);
			m_DragCard.SetActive(true);

			m_Callbacks = new CardCallbacks()
			{
				OnPointerDown = OnCardPointerDown,
				OnPointerUp   = OnCardPointerUp,
			};

			m_PlayerRef = -1;
		}

		public void Deinitialize()
		{
			QuantumCallback.UnsubscribeListener<CallbackGameStarted>(this);
			QuantumEvent.UnsubscribeListener<EventCardsChanged>(this);

			m_Callbacks.OnPointerDown = null;
			m_Callbacks.OnPointerUp   = null;
			m_Callbacks               = null;
		}

		public void OnUpdate(SceneContext context)
		{
			if (Entities.LocalPlayerEntity.IsValid == false)
				return;

			var qCardManager = context.Frame.Unsafe.GetPointer<QuantumCardManager>(Entities.LocalPlayerEntity);

			var currentEnergy = qCardManager->CurrentEnergy.AsFloat;
			var maxEnergy     = qCardManager->MaxEnergy.AsFloat;

			m_EnergyBarFluid.value    = currentEnergy / maxEnergy;
			m_EnergyBarDescrete.value = (int)currentEnergy / maxEnergy;

			m_EnergyText.text = ((int)currentEnergy).ToString();

			/*
			if (qCardManager->NextFillTime > FP._0)
			{
				m_NextCountdown.text  = qCardManager->NextFillTime.AsFloat.ToString("0.0");
				m_NextCardGroup.alpha = 0f;
			}
			else
			{
				m_NextCountdown.text  = string.Empty;
				m_NextCardGroup.alpha = 0f;
			}
			*/

			// Set NextFillTime to 0
    		qCardManager->NextFillTime = FP._0;

			m_NextCardGroup.alpha = 0.25f;

			if (m_PlayerRef != Entities.LocalPlayer)
			{
				RefreshCards(Entities.LocalPlayerEntity);
				m_PlayerRef = Entities.LocalPlayer;

				if (m_DraggingCard != null)
				{
					m_DraggingCard.ShowFrame(true);
					m_DraggingCard = null;
				}
			}

			m_DragCard.SetActive(m_DraggingCard);

			for (int idx = 0, count = m_Cards.Count; idx < count; idx++)
			{
				m_Cards[idx].SetEnergy(currentEnergy);
			}

			if (m_DraggingCard != null)
			{
				var screenPoint = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 10f);
				var position    = m_CanvasCamera.ScreenToWorldPoint(screenPoint);

				m_DragCard.transform.position   = position;

				var distance = Mathf.Clamp01(position.y - m_DraggingCard.transform.position.y);
				m_DragCard.transform.localScale = (1f - distance) * Vector3.one;

				m_CardManager.ActiveCardSettings = m_DraggingCard.Settings;
			}
			else
			{
				m_CardManager.ActiveCardSettings = null;
			}
		}

		// PRIVATE METHODS

		private void OnGameStarted(CallbackGameStarted started)
		{
			var frame    = started.Game.Frames.Predicted;
			var settings = frame.FindAsset<GameplaySettings>(frame.RuntimeConfig.GameplaySettings.Id);

			for (int idx = 1; idx < settings.MaxEnergy; idx++)
			{
				Instantiate(m_EnergyTile, m_EnergyTile.transform.parent);
			}

			m_MaxEnergyText.text = $"Max: {settings.MaxEnergy}";
		}

		private void OnCardsChanged(EventCardsChanged cardsChanged)
		{
			if (cardsChanged.Entity != Entities.LocalPlayerEntity)
				return;

			RefreshCards(cardsChanged.Entity);
		}

		private void RefreshCards(EntityRef entity)
		{
			var frame        = QuantumRunner.Default.Game.Frames.Predicted;
			var qCardManager = frame.Unsafe.GetPointer<QuantumCardManager>(entity);
			var qCards       = frame.ResolveList(qCardManager->AvailableCards);

			for (int idx = 0; idx < QuantumCardManager.AVAILABLE_CARDS_COUNT; idx++)
			{
				var card  = m_Cards[idx];
				var qCard = qCards.GetPointer(idx);

				card.SetActive(true);

				if (qCard->CardSettings.Id.IsValid == true)
				{
					var settings = UnityDB.FindAsset<CardSettingsAsset>(qCard->CardSettings.Id);
					card.SetData(settings, m_Callbacks);
				}
				else
				{
					card.SetData(null, null);
				}
			}

			var queue        = frame.ResolveList(qCardManager->CardQueue);
			var nextSettings = UnityDB.FindAsset<CardSettingsAsset>(queue[qCardManager->QueueHeadIndex].CardSettings.Id);
			m_NextCard.SetActive(true);
			m_NextCard.SetData(nextSettings, null);
		}

		private void OnCardPointerDown(UICard uiCard)
		{
			if (m_DraggingCard != null)
				return;

			m_DragCard.SetData(uiCard.Settings, null);
			m_DraggingCard = uiCard;

			uiCard.ShowFrame(false);
		}

		private void OnCardPointerUp(UICard uiCard)
		{
			if (m_DraggingCard != uiCard)
				return;

			m_CardManager.UseCard();

			uiCard.ShowFrame(true);
			m_DraggingCard = null;
		}

		// HELPERS

		public class CardCallbacks
		{
			public Action<UICard> OnPointerDown;
			public Action<UICard> OnPointerUp;
		}
	}
}