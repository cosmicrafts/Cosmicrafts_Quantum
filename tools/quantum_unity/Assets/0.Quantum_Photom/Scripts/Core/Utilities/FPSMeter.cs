namespace TowerRush.Core
{
	using UnityEngine;
	using System.Collections;
	using TMPro;

	public class FPSMeter : MonoBehaviour
	{
		private static FPSMeter m_Instance;

		private void Awake()
		{
			if (m_Instance != null)
			{
				Destroy(gameObject);
				return;
			}

			m_Instance = this;

			DontDestroyOnLoad(transform.root);
		}

		private IEnumerator Start()
		{
			var yielder    = new WaitForSecondsRealtime(0.4f);
			var text       = GetComponentInChildren<TextMeshProUGUI>();
			var frameCount = 0;
			var frameDiff  = 0;
			var lastTime   = 0f;
			var timeDiff   = 0f;
			var fps        = 0;

			var orange     = new Color(1f, 0.5f, 0.3f);
			var white      = new Color(1f, 1f, 1f, 0.3f);

			while (true)
			{
				frameDiff  = Time.frameCount - frameCount;
				frameCount = Time.frameCount;

				timeDiff   = Time.realtimeSinceStartup - lastTime;
				lastTime   = Time.realtimeSinceStartup;

				fps        = Mathf.RoundToInt(frameDiff / timeDiff);
				text.text  = $"{fps:0}";

				if (fps > 30)
				{
					text.color = white;

				}
				else if (fps > 20)
				{
					text.color = orange;
				}
				else
				{
					text.color = Color.red;
				}

				yield return yielder;
			}
		}
	}
}
