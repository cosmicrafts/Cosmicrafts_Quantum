namespace TowerRush.Core
{
	using UnityEngine;

	using System.Collections;
	using System.Collections.Generic;

	public static class WaitFor
	{
		//========== CONSTANTS ========================================================================================

		private const float RESOLUTION = 1f / 100f;
		private const float ALMOST_ONE = 0.999996f;

		//========== PRIVATE MEMBERS ==================================================================================

		private static WaitForFixedUpdate                              _fixedUpdate       = new WaitForFixedUpdate();
		private static WaitForEndOfFrame                               _endOfFrame        = new WaitForEndOfFrame();
		private static Dictionary<int, WaitForSeconds>                 _seconds           = new Dictionary<int, WaitForSeconds>(64);
		private static Dictionary<int, List<WaitForSecondsRealtime>>   _realtimeSeconds   = new Dictionary<int, List<WaitForSecondsRealtime>>(64);

		//========== PUBLIC MEMBERS ===================================================================================

		public static YieldInstruction FixedUpdate()
		{
			return _fixedUpdate;
		}

		public static YieldInstruction EndOfFrame()
		{
			return _endOfFrame;
		}

		public static YieldInstruction Seconds(float seconds)
		{
			if (seconds <= 0f)
				return null;

			int frames = (int)(seconds / RESOLUTION + ALMOST_ONE);

			if (_seconds.TryGetValue(frames, out var waitFor) == false)
			{
				waitFor = new WaitForSeconds(frames * RESOLUTION);
			
				_seconds.Add(frames, waitFor);
			}

			return waitFor;
		}

		public static IEnumerator SecondsRealtime(float seconds)
		{
			if (seconds <= 0f)
				return null;

			int frames = (int)(seconds / RESOLUTION + ALMOST_ONE);

			if (_realtimeSeconds.TryGetValue(frames, out var list) == false)
			{
				list                     = new List<WaitForSecondsRealtime>();
				_realtimeSeconds[frames] = list;
			}

			WaitForSecondsRealtime waitFor = null;

			for (int i = list.Count; i --> 0;)
			{
				WaitForSecondsRealtime candidate = list[i];
				if (candidate.IsBusy == true)
					continue;

				waitFor = candidate;
				break;
			}

			if (waitFor == null)
			{
				waitFor = new WaitForSecondsRealtime(frames * RESOLUTION);
				list.Add(waitFor);
			}

			waitFor.Reset();
			return waitFor;
		}

		//========== HELPERS ==========================================================================================

		sealed class WaitForSecondsRealtime : IEnumerator
		{
			internal bool           IsBusy;

			private  readonly float m_Duration;
			private  float          m_EndTime;

			// C-TOR

			public WaitForSecondsRealtime(float duration)
			{
				m_Duration = duration;
			}

			// IENUMERATOR INTERFACE

			object IEnumerator.Current { get { return null; } }

			bool IEnumerator.MoveNext()
			{
				IsBusy = m_EndTime > Time.realtimeSinceStartup;
				return IsBusy;
			}

			public void Reset()
			{
				m_EndTime = Time.realtimeSinceStartup + m_Duration;
				IsBusy = true;
			}
		}
	}
}
