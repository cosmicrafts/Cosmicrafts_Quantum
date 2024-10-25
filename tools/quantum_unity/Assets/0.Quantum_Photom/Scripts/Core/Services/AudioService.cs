namespace TowerRush.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Audio;

	public class AudioService : MonoBehaviour
	{
		// CONSTANTS

		private const string SOUNDS_VOLUME = "SoundsVolume";
		private const string MUSIC_VOLUME  = "MusicVolume";

		// CONFIGURATION

		[SerializeField] AudioMixer  m_AudioMixer;
		[SerializeField] MusicInfo[] m_MusicSetup;

		// PRIVATE MEMBERS

		private AudioSource   m_MusicAudio;
		private AudioSource   m_SoundsAudio;
		private Transform     m_AudioListenerTransform;
		private Transform     m_ReferenceTransform;
		private Coroutine     m_FadeSoundsCoroutine;
		private Coroutine     m_FadeMusicCoroutine;
		private string        m_CurrentMusic;
		private List<AudioSource> activeAudioSources = new List<AudioSource>();
		private int maxAudioSources = 5; // Set your limit here

		// PUBLIC METHODS

		public void Initialize()
		{
			transform.SetParent(null);
			DontDestroyOnLoad(gameObject);

			m_AudioListenerTransform = new GameObject("AudioListener", typeof(AudioListener)).transform;
			m_AudioListenerTransform.SetParent(transform);

			m_MusicAudio = transform.FindDeep("MusicSource").GetComponent<AudioSource>();
			m_SoundsAudio = transform.FindDeep("SoundsSource").GetComponent<AudioSource>();

			m_AudioMixer.SetFloat(SOUNDS_VOLUME, ToDb(GameOptions.SoundsVolume));
			m_AudioMixer.SetFloat(MUSIC_VOLUME,  ToDb(GameOptions.MusicVolume));

			Signals.GameOptionsChanged.Connect(OnGameOptionsChanged);
		}

		public void Deinitialize()
		{
			StopAllCoroutines();

			Signals.GameOptionsChanged.Disconnect(OnGameOptionsChanged);

			m_FadeSoundsCoroutine    = null;
			m_FadeMusicCoroutine     = null;
			m_AudioListenerTransform = null;
		}

		public void OnUpdate()
		{
			if (m_ReferenceTransform != null)
			{
				m_AudioListenerTransform.position = m_ReferenceTransform.position;
			}
		}

		public void FadeSounds(float volume, float duration, float delay)
		{
			if (m_FadeSoundsCoroutine != null)
			{
				StopCoroutine(m_FadeSoundsCoroutine);
			}

			m_FadeSoundsCoroutine = StartCoroutine(FadeSounds_Coroutine(volume, duration, delay));
		}

		public void FadeMusic(float duration, string clipName)
		{
			var volume = 0f;

			if (clipName.HasValue() == true)
			{
				var musicInfo = m_MusicSetup.Find(obj => obj.Name == clipName);
				if (musicInfo != null)
				{
					volume = musicInfo.Volume;
				}
				else
				{
					volume = 1;
				}
			}

			if (m_FadeMusicCoroutine != null)
			{
				StopCoroutine(m_FadeMusicCoroutine);
			}

			m_FadeMusicCoroutine = StartCoroutine(FadeMusic_Coroutine(volume, duration, clipName));
		}

		public void SetSoundsVolume(float value)
		{
			m_AudioMixer.SetFloat(SOUNDS_VOLUME, ToDb(value));
		}

		public void SetMusicVolume(float value)
		{
			m_AudioMixer.SetFloat(MUSIC_VOLUME, ToDb(value));
		}

		public void SetReferenceTransform(Transform referenceTransform)
		{
			m_ReferenceTransform = referenceTransform;
		}

		public void PlayAudio(AudioClip clip, string tag)
		{
			if (activeAudioSources.Count >= maxAudioSources)
			{
				Debug.LogWarning("Maximum number of audio sources reached.");
				return;
			}

			GameObject audioObject = new GameObject("AudioSource");
			AudioSource audioSource = audioObject.AddComponent<AudioSource>();
			audioSource.clip = clip;
			audioSource.tag = tag;
			audioSource.Play();

			Debug.Log($"Playing audio clip '{clip.name}' with tag '{tag}'");

			activeAudioSources.Add(audioSource);

			StartCoroutine(RemoveAudioSourceWhenFinished(audioSource));
		}

		private IEnumerator RemoveAudioSourceWhenFinished(AudioSource source)
		{
			yield return new WaitUntil(() => !source.isPlaying);
			activeAudioSources.Remove(source);
			Destroy(source.gameObject);
		}

		// PRIVATE METHODS

		private IEnumerator FadeSounds_Coroutine(float targetVolume, float duration, float delay)
		{
			if (delay > 0f)
			{
				yield return WaitFor.SecondsRealtime(delay);
			}

			if (duration > 0)
			{
				var progress    = 0f;
				m_AudioMixer.GetFloat(SOUNDS_VOLUME, out var startVolume);

				startVolume = FromDb(startVolume);

				while (progress < 1f)
				{
					progress  = Mathf.Clamp01(progress + Time.deltaTime / duration);
					var value = Mathf.Lerp(startVolume, targetVolume, progress) * GameOptions.SoundsVolume;
					m_AudioMixer.SetFloat(SOUNDS_VOLUME, ToDb(value));

					yield return null;
				}
			}
			else
			{
				m_AudioMixer.SetFloat(SOUNDS_VOLUME, ToDb(targetVolume));
			}

			m_FadeSoundsCoroutine = null;
		}

		private IEnumerator FadeMusic_Coroutine(float targetVolume, float duration, string clipName)
		{
			m_AudioMixer.GetFloat(MUSIC_VOLUME, out var startVolume);

			startVolume = FromDb(startVolume);

			if (startVolume == targetVolume && clipName == m_CurrentMusic)
				yield break;

			// FADE OUT
			if (startVolume > 0)
			{
				if (m_CurrentMusic.HasValue() == true)
				{
					yield return FadeMusic_Coroutine(startVolume, 0, duration);
				}
				else
				{
					m_AudioMixer.SetFloat(MUSIC_VOLUME, ToDb(0f));
				}
			}

			if (targetVolume == 0)
				yield break;

			if (clipName.HasValue() == true)
			{
				ChangeMusicClip(clipName);
			}

			if (m_CurrentMusic.IsNullOrEmpty() == true)
				yield break;

			// FADE IN
			yield return FadeMusic_Coroutine(0, targetVolume, duration);

			m_FadeMusicCoroutine = null;
		}

		private IEnumerator FadeMusic_Coroutine(float startVolume, float targetVolume, float duration)
		{
			if (duration > 0)
			{
				var progress = 0f;

				while (progress < 1f)
				{
					progress  = Mathf.Clamp01(progress + Time.deltaTime / duration);
					var value = Mathf.Lerp(startVolume, targetVolume, progress) * GameOptions.MusicVolume;
					m_AudioMixer.SetFloat(MUSIC_VOLUME, ToDb(value));

					yield return null;
				}
			}
			else
			{
				m_AudioMixer.SetFloat(MUSIC_VOLUME, ToDb(targetVolume));
			}
		}

		public void ChangeMusicClip(string clipName)
		{
			var musicInfo = m_MusicSetup.Find(obj => obj.Name == clipName);
			if (musicInfo != null)
			{
				m_MusicAudio.clip = musicInfo.Clip;
				m_CurrentMusic    = clipName;

				m_MusicAudio.Play();
			}
			else
			{
				m_MusicAudio.clip = null;
				m_CurrentMusic    = null;
			}
		}

		private void OnGameOptionsChanged()
		{
			m_AudioMixer.SetFloat(SOUNDS_VOLUME, ToDb(GameOptions.SoundsVolume));
			m_AudioMixer.SetFloat(MUSIC_VOLUME,  ToDb(GameOptions.MusicVolume));
		}

		private float FromDb(float db)
		{
			return db / 80f + 1f;
		}

		private float ToDb(float volume)
		{
			return (volume - 1f) * 80f;
		}

		// HELPERS

		[System.Serializable]
		private class MusicInfo
		{
			public string    Name;
			[Range(0f, 1f)]
			public float     Volume = 1f;
			public AudioClip Clip;
		}
	}
}
