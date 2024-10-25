#if ENABLE_CHEAT_MANAGER
using UnityEngine;
using TowerRush.Core;
using System.Collections.Generic;
using Quantum;

[System.Serializable]
public class CheatManager : ScriptableObject
{
	// CONSTANTS

	private const string KEY_ACTIVE_PROFILE = "ActiveProfile";
	private const string KEY_PROFILES       = "Profiles";
	private const string DEFAULT            = ".Default";

	// PUBLIC MEMBERS

	[Header("Debug")]
	public bool ShowDebugInfo = true;

	[Header("Intro")]
	public EIntroBehavior IntroBehavior;

	[Header("Player")]
	public byte           Level;
	public CardInfo[]     Cards;


	public  static string         ActiveProfile { get { return GetActiveProfile(); } set { SetActiveProfile(value); } }
	public  static CheatManager   Instance      { get { return GetInstance(); } }
	public  static string[]       Profiles      { get; private set; }

	// PRIVATE MEMBERS

	private static CheatManager   m_Instance;
	private static DictionaryFile m_Storage;
	private static string         m_ActiveProfile;

	// PUBLIC METHODS

	public static void Save()
	{
		#if UNITY_EDITOR
		var json    = JsonUtility.ToJson(m_Instance);
		var storage = GetStorage();

		storage.SetString(ActiveProfile, json);
		storage.Save();
		#endif
	}

	public static string SaveToJson()
	{
		var instance = GetInstance();
		if (instance == null)
			return null;

		return JsonUtility.ToJson(instance);
	}

	public static void Load(string json)
	{
		if (m_Instance != null)
		{
			DestroyImmediate(m_Instance);
		}

		m_Instance = CreateInstance<CheatManager>();

		if (json.HasValue() == true)
		{
			JsonUtility.FromJsonOverwrite(json, m_Instance);
		}
		else
		{
			Load_Editor();
		}
	}

	public static void CreateProfile(string profile)
	{
		var profiles = new List<string>(Profiles);
		if (profiles.Contains(profile) == true)
			return;

		profiles.Add(profile);
		profiles.Sort();

		Profiles      = profiles.ToArray();
		ActiveProfile = profile;

		var storage = GetStorage();
		storage.SetString(KEY_PROFILES, string.Join(";", Profiles));
		storage.Save();
	}

	public static void DeleteProfile(string profile)
	{
		if (Profiles.Length <= 1)
			return;
		var profiles = new List<string>(Profiles);
		if (profiles.Remove(profile) == false)
			return;

		Profiles      = profiles.ToArray();
		ActiveProfile = Profiles[0];

		var storage = GetStorage();
		storage.Remove(profile);
		storage.SetString(KEY_PROFILES, string.Join(";", Profiles));
		storage.Save();
	}

	// PRIVATE METHODS

	private static CheatManager GetInstance() 
	{
		if (m_Instance == null)
		{
			Load_Editor();
		}

		return m_Instance;
	}

	private static DictionaryFile GetStorage()
	{
		if (m_Storage == null)
		{
			m_Storage = DictionaryFile.Load("CheatManager");
		}

		return m_Storage;
	}

	private static void Load_Editor()
	{
		m_Instance = CreateInstance<CheatManager>();

		var storage  = GetStorage();
		var profiles = storage.GetString(KEY_PROFILES, DEFAULT);
		Profiles     = profiles.Split(';');

		#if UNITY_EDITOR
		var json = storage.GetString(ActiveProfile, null);

		if (json.HasValue() == true)
		{
			JsonUtility.FromJsonOverwrite(json, m_Instance);
		}
		#endif
	}

	private static string GetActiveProfile()
	{
		if (m_ActiveProfile.IsNullOrEmpty() == true)
		{
			m_ActiveProfile = GetStorage().GetString(KEY_ACTIVE_PROFILE, DEFAULT);
		}

		return m_ActiveProfile;
	}

	private static void SetActiveProfile(string profile)
	{
		if (m_ActiveProfile == profile)
			return;

		m_ActiveProfile = profile;

		var storage = GetStorage();
		Load(storage.GetString(profile, null));
		storage.SetString(KEY_ACTIVE_PROFILE, profile);
		storage.Save();
	}

	// HELPERS

	public enum EIntroBehavior
	{
		Default,
		Always,
		Never,
	}
}
#endif // ENABLE_CHEAT_MANAGER
