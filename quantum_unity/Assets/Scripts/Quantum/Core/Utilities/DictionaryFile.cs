namespace TowerRush.Core
{
	using UnityEngine;
	using System.Collections.Generic;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
	using Quantum;

	public class DictionaryFile
	{
		// CONSTANTS

#if UNITY_EDITOR
		private const string FILEPATH = "{0}/.editor/{1}";
#else
		private const string FILEPATH = "{0}/{1}";
#endif

		// PUBLIC MEMBERS

		public string FileName { get { return m_FileName; } }
		public string FilePath { get { return m_FilePath; } }

		// PRIVATE MEMBERS

		private readonly string            m_FileName;
		private readonly string            m_FilePath;
		private Dictionary<string, object> m_Dictionary = new Dictionary<string, object>();

		// C-TOR

		private DictionaryFile(string name, string path)
		{
			m_FileName = name;
			m_FilePath = path;
		}

		// PUBLIC MEMBERS

		public static bool Exists(string filename)
		{
			if (filename.IsNullOrEmpty() == true)
				return false;

			var filepath = GetPath(filename);

			return File.Exists(filepath);
		}

		public static DictionaryFile Load(string filename, bool create = true)
		{
			var filepath = GetPath(filename);
			var isNew    = false;

			if (File.Exists(filepath) == false)
			{
				if (create == false)
				{
					return null;
				}
				else
				{
					var directory = Path.GetDirectoryName(filepath);
					if (Directory.Exists(directory) == false)
					{
						Directory.CreateDirectory(directory);
					}

					using (File.Create(filepath))
					{
						isNew = true;
					}
				}
			}

			var dictFile = new DictionaryFile(filename, filepath);
			if (isNew == true)
			{
				dictFile.Save();

				return dictFile;
			}

			try
			{
#if UNITY_EDITOR
				using (var fs     = File.OpenRead(filepath))
				using (var reader = new BinaryReader(fs))
#else
				using (var fs     = File.OpenRead(filepath))
				using (var cs     = new CryptoStream(fs, Crypto().CreateDecryptor(), CryptoStreamMode.Read))
				using (var reader = new BinaryReader(cs))
#endif
				{
					if (fs.Length > 0)
					{
						Load(dictFile, reader);
					}
				}
			}
			catch (System.Exception e)
			{
				Log.Error($"DictionaryFile.Load() :: Exception '{e.GetType().Name}' occured for '{filename}'!");
			}

			return dictFile;
		}

		public void Save()
		{
			var tmpPath = FilePath + ".temp";

			try
			{
#if UNITY_EDITOR
				using (var fs     = File.OpenWrite(tmpPath))
				using (var writer = new BinaryWriter(fs))
#else
				using (var fs     = File.OpenWrite(tmpPath))
				using (var cs     = new CryptoStream(fs, Crypto().CreateEncryptor(), CryptoStreamMode.Write))
				using (var writer = new BinaryWriter(cs))
#endif
				{
					Save(this, writer);
				}

				if (File.Exists(tmpPath) == true)
				{
					if (File.Exists(FilePath) == true)
						File.Delete(FilePath);

					File.Move(tmpPath, FilePath);
				}
			}
			catch (System.Exception e)
			{
				Log.Error($"DictionaryFile.Save() :: Exception '{e.GetType().Name}' occured for '{FileName}'!");
			}
		}

		public void Clear()
		{
			m_Dictionary.Clear();
		}

		public bool Contains(string key)
		{
			return m_Dictionary.ContainsKey(key);
		}

		public void Remove(string key)
		{
			m_Dictionary.Remove(key);
		}

		public void SetBool(string key, bool val)
		{
			SetInt(key, val ? 1 : 0);
		}

		public void SetInt(string key, int val)
		{
			m_Dictionary[key] = val;
		}

		public void SetLong(string key, long val)
		{
			m_Dictionary[key] = val;
		}

		public void SetFloat(string key, float val)
		{
			m_Dictionary[key] = val;
		}

		public void SetString(string key, string val)
		{
			m_Dictionary[key] = val;
		}

		public bool GetBool(string key, bool defVal = false)
		{
			return GetInt(key, defVal ? 1 : 0) != 0 ? true : false;
		}

		public int GetInt(string key, int defVal = 0)
		{
			if (m_Dictionary.TryGetValue(key, out var value) == true)
				return (int)value;

			return defVal;
		}

		public long GetLong(string key, long defVal = 0L)
		{
			if (m_Dictionary.TryGetValue(key, out var value) == true)
				return (long)value;

			return defVal;
		}

		public float GetFloat(string key, float defVal = 0f)
		{
			if (m_Dictionary.TryGetValue(key, out var value) == true)
				return (float)value;

			return defVal;
		}

		public string GetString(string key, string defVal = null)
		{
			if (m_Dictionary.TryGetValue(key, out var value) == true)
				return (string)value;

			return defVal;
		}

		// PRIVATE MEMBERS

		private static void Load(DictionaryFile dictFile, BinaryReader reader)
		{
			var dict = dictFile.m_Dictionary;
			dict.Clear();

			for (int idx = 0, count = reader.ReadInt32(); idx < count; ++idx)
			{
				var key = reader.ReadString();
				var val = reader.ReadInt32();

				dict[key] = val;
			}

			for (int idx = 0, count = reader.ReadInt32(); idx < count; ++idx)
			{
				var key = reader.ReadString();
				var val = reader.ReadInt64();

				dict[key] = val;
			}

			for (int idx = 0, count = reader.ReadInt32(); idx < count; ++idx)
			{
				var key = reader.ReadString();
				var val = reader.ReadSingle();

				dict[key] = val;
			}

			for (int idx = 0, count = reader.ReadInt32(); idx < count; ++idx)
			{
				var key = reader.ReadString();
				var val = reader.ReadString();

				dict[key] = val;
			}
		}

		private static void Save(DictionaryFile dictFile, BinaryWriter writer)
		{
			var ints    = ListPool.Get<KeyValuePair<string, object>>(8);
			var longs   = ListPool.Get<KeyValuePair<string, object>>(8);
			var floats  = ListPool.Get<KeyValuePair<string, object>>(8);
			var strings = ListPool.Get<KeyValuePair<string, object>>(8);

			// categorize...

			foreach (var pair in dictFile.m_Dictionary)
			{
				if (pair.Value is int)
				{
					ints.Add(pair);
				}
				else if (pair.Value is long)
				{
					longs.Add(pair);
				}
				else if (pair.Value is float)
				{
					floats.Add(pair);
				}
				else if (pair.Value is string)
				{
					strings.Add(pair);
				}
			}

			// write...

			writer.Write(ints.Count);
			foreach (var pair in ints)
			{
				writer.Write(pair.Key);
				writer.Write((int)pair.Value);
			}

			writer.Write(longs.Count);
			foreach (var pair in longs)
			{
				writer.Write(pair.Key);
				writer.Write((long)pair.Value);
			}

			writer.Write(floats.Count);
			foreach (var pair in floats)
			{
				writer.Write(pair.Key);
				writer.Write((float)pair.Value);
			}

			writer.Write(strings.Count);
			foreach (var pair in strings)
			{
				writer.Write(pair.Key);
				writer.Write((string)pair.Value);
			}

			ListPool.Return(ints);
			ListPool.Return(longs);
			ListPool.Return(floats);
			ListPool.Return(strings);
		}

		private static string GetPath(string filename)
		{
			return string.Format(FILEPATH, Application.persistentDataPath, filename);
		}

		private static DESCryptoServiceProvider Crypto()
		{
			var crypto = new DESCryptoServiceProvider();
			crypto.Key = ASCIIEncoding.ASCII.GetBytes("muKzm2cr");
			crypto.IV  = ASCIIEncoding.ASCII.GetBytes("ZCyNhQEz");

			return crypto;
		}
	}
}
