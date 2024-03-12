namespace TowerRush
{
	using TowerRush.Core;
	using System.Collections;
	using UnityEngine;

	public class UIViewLoading : UIView
	{
		// CONFIGURATION

		[SerializeField] Animation m_Animation;

		// PUBLIC MEMBERS

		public  bool   FullyVisible { get { return m_VisibilityState == EState.Visible; } }
		public  bool   FullyHidden  { get { return m_VisibilityState == EState.Hidden;  } }

		// PRIVATE MEMBERS

		private EState m_VisibilityState;

		// PUBLIC METHODS

		public void Show()
		{
			StartCoroutine(Animate_Coroutine(true));
		}

		public void Hide()
		{
			StartCoroutine(Animate_Coroutine(false));
		}

		// PRIVATE METHODS

		private IEnumerator Animate_Coroutine(bool show)
		{
			var state = m_Animation[m_Animation.clip.name];
			state.speed          = show == true ? 1f : -1f;
			state.normalizedTime = show == true ? 0f : 1f;

			m_Animation.Play();

			while (m_Animation.isPlaying == true)
				yield return null;

			m_VisibilityState = show == true ? EState.Visible : EState.Hidden;
		}

		// HELPERS

		private enum EState
		{
			None,
			AnimIn,
			Visible,
			AnimOut,
			Hidden,
		}
	}
}