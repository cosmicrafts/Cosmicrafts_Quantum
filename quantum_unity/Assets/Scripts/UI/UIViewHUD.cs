namespace TowerRush
{
	using TowerRush.Core;
	using Quantum;
	using UnityEngine;
	using TMPro;

	public unsafe class UIViewHUD : UIView
	{
		// CONFIGURATION

		[SerializeField] UICardManager   m_CardManager;
		[SerializeField] TextMeshProUGUI m_MatchText;
		[SerializeField] TextMeshProUGUI m_MatchTextSmall;
		[SerializeField] TextMeshProUGUI m_MatchTime;
		[SerializeField] TextMeshProUGUI m_ScoreFriendText;
		[SerializeField] TextMeshProUGUI m_ScoreEnemyText;


		// UIVIew INTERFACE

		protected override void OnInitialize()
		{
			base.OnInitialize();

			m_MatchText.SetActive(false);

			m_CardManager.Initialize(Frontend.Scene.GetSceneComponent<CardManager>(), Frontend.Canvas.worldCamera);

			QuantumEvent.Subscribe<EventGameplayStateChanged>(this, OnGameplayStateChanged);
			QuantumEvent.Subscribe<EventGameplayResult>(this, OnGameplayResult);
			QuantumEvent.Subscribe<EventScoreGained>(this, OnScoreGained);

			Signals.LocalPlayerChanged.Connect(OnLocalPlayerChanged);

			m_ScoreFriendText.text = "0";
			m_ScoreEnemyText.text  = "0";

			m_MatchTextSmall.text = "Waiting for other player";
			m_MatchTextSmall.SetActive(true);
		}

		protected override void OnDeinitialize()
		{
			m_CardManager.Deinitialize();

			QuantumEvent.UnsubscribeListener<EventGameplayStateChanged>(this);
			QuantumEvent.UnsubscribeListener<EventGameplayResult>(this);
			QuantumEvent.UnsubscribeListener<EventScoreGained>(this);

			Signals.LocalPlayerChanged.Disconnect(OnLocalPlayerChanged);

			base.OnDeinitialize();
		}

		protected override void OnUpdate(SceneContext context)
		{
			base.OnUpdate(context);

			var qGameplay = context.Frame.Unsafe.GetPointerSingleton<Gameplay>();

			m_MatchTime.text = new System.TimeSpan(0, 0, Mathf.Max((int)(qGameplay->StateTime.AsFloat + 0.5f), 0)).ToString("m\\:ss");

			m_CardManager.OnUpdate(context);
		}

		private void OnGameplayStateChanged(EventGameplayStateChanged stateChanged)
		{
			m_MatchTextSmall.SetActive(false);

			switch (stateChanged.State)
			{
				case EGameplayState.WaitingForPlayers:
					m_MatchTextSmall.text = "Waiting for other player";
					m_MatchTextSmall.SetActive(true);
					break;
				case EGameplayState.Match:
					ShowMatchText("FIGHT!");
					break;
				case EGameplayState.Overtime:
					ShowMatchText("OVERTIME!");
					break;
			}
		}

		private void OnGameplayResult(EventGameplayResult gameplayResult)
		{
			if (gameplayResult.Winner < 0)
			{
				ShowMatchText("DRAW");
			}
			else if (gameplayResult.Winner == Entities.LocalPlayer)
			{
				ShowMatchText("VICTORY!");
			}
			else
			{
				ShowMatchText("DEFEAT");
			}
		}

		private void OnScoreGained(EventScoreGained scoreGained)
		{
			if (scoreGained.PlayerRef == Entities.LocalPlayer)
			{
				m_ScoreFriendText.text = scoreGained.TotalScore.ToString();
			}
			else
			{
				m_ScoreEnemyText.text = scoreGained.TotalScore.ToString();
			}
		}

		private void OnLocalPlayerChanged(int playerRef, EntityRef playerEntity)
		{
			var qGameplay = QuantumRunner.Default.Game.Frames.Predicted.Unsafe.GetPointerSingleton<Gameplay>();

			if (Entities.LocalPlayer == 0)
			{
				m_ScoreFriendText.text = qGameplay->AlphaScore.ToString();
				m_ScoreEnemyText.text  = qGameplay->BetaScore.ToString();
			}
			else
			{
				m_ScoreFriendText.text = qGameplay->BetaScore.ToString();
				m_ScoreEnemyText.text  = qGameplay->AlphaScore.ToString();
			}
		}

		private void ShowMatchText(string text)
		{
			m_MatchText.SetActive(false);
			m_MatchText.text = text;
			m_MatchText.SetActive(true);
		}
	}
}