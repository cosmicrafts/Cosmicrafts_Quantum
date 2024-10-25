namespace TowerRush
{
	using TowerRush.Core;

	public static class GameOptions
	{
		// PUBLIC MEMBERS

		public static float MusicVolume  { get { return m_MusicVolume;  } set { SetValue(ref m_MusicVolume,  value); } }
		public static float SoundsVolume { get { return m_SoundsVolume; } set { SetValue(ref m_SoundsVolume, value); } }

		// PRIVATE MEMBERS

		private static DictionaryFile m_DictionaryFile;

		private static float          m_MusicVolume;
		private static float          m_SoundsVolume;

		// PUBLIC METHODS

		public static void Load()
		{
			LoadDefaultValues();

			m_DictionaryFile = DictionaryFile.Load("GameOptions", true);

			m_MusicVolume    = m_DictionaryFile.GetFloat(nameof(MusicVolume),  MusicVolume);
			m_SoundsVolume   = m_DictionaryFile.GetFloat(nameof(SoundsVolume), SoundsVolume);

			Signals.GameOptionsChanged.Emit();
		}

		public static void Save()
		{
			if (m_DictionaryFile == null)
				return;

			m_DictionaryFile.SetFloat(nameof(MusicVolume),  m_MusicVolume);
			m_DictionaryFile.SetFloat(nameof(SoundsVolume), m_SoundsVolume);

			m_DictionaryFile.Save();
		}

		public static void Reset()
		{
			m_DictionaryFile.Clear();
			m_DictionaryFile.Save();

			LoadDefaultValues();

			Signals.GameOptionsChanged.Emit();
		}

		// PRIVATE METHODS

		private static void LoadDefaultValues()
		{
			m_MusicVolume  = 0.4f;
			m_SoundsVolume = 1f;
		}

		private static void SetValue(ref float field, float newValue)
		{
			if (field == newValue)
				return;

			field = newValue;
			Signals.GameOptionsChanged.Emit();
		}

		private static void SetValue(ref bool field, bool newValue)
		{
			if (field == newValue)
				return;

			field = newValue;
			Signals.GameOptionsChanged.Emit();
		}
	}
}