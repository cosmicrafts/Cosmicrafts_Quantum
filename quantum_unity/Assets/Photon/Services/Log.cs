namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public enum ELogGroup
	{
		None           = 0,
		Chat           = 1,
		Realtime       = 2,
		Network        = 3,
		Messages       = 4,
		Matchmaking    = 5,
		Lobby          = 6,
		Party          = 7,
	}

	public enum ELogSeverity
	{
		None      = 0,
		Exception = 1,
		Error     = 2,
		Warning   = 3,
		Info      = 4,
	}

	public sealed class Log : IService
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public bool EnableFrameCount;
		public bool EnableTimeStamp;
		public bool OutputToUnity;
		public bool SaveMessages;

		public readonly List<string> Messages = new List<string>();

		//========== PRIVATE MEMBERS ==================================================================================

		private readonly ELogSeverity[] Severity = new ELogSeverity[128];

		//========== CONSTRUCTORS =====================================================================================

		public Log()
		{
			EnableFrameCount = false;
			EnableTimeStamp  = true;
			OutputToUnity    = true;
			SaveMessages     = false;

			SetSeverity(ELogSeverity.Info);
		}

		//========== PUBLIC METHODS ===================================================================================

		public void Info     (ELogGroup group, string message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Info)      { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message);                           if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.Log       (formattedMessage); } }
		public void Warning  (ELogGroup group, string message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Warning)   { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message);                           if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogWarning(formattedMessage); } }
		public void Error    (ELogGroup group, string message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Error)     { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message);                           if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogError  (formattedMessage); } }
		public void Exception(ELogGroup group, string message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Exception) { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message);                           if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogError  (formattedMessage); } }

		public void Info     (ELogGroup group, object message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Info)      { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message.ToString());                if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.Log       (formattedMessage); } }
		public void Warning  (ELogGroup group, object message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Warning)   { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message.ToString());                if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogWarning(formattedMessage); } }
		public void Error    (ELogGroup group, object message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Error)     { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message.ToString());                if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogError  (formattedMessage); } }
		public void Exception(ELogGroup group, object message)                            { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Exception) { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(message.ToString());                if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogError  (formattedMessage); } }

		public void Info     (ELogGroup group, string format, params object[] parameters) { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Info)      { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(string.Format(format, parameters)); if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.Log       (formattedMessage); } }
		public void Warning  (ELogGroup group, string format, params object[] parameters) { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Warning)   { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(string.Format(format, parameters)); if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogWarning(formattedMessage); } }
		public void Error    (ELogGroup group, string format, params object[] parameters) { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Error)     { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(string.Format(format, parameters)); if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogError  (formattedMessage); } }
		public void Exception(ELogGroup group, string format, params object[] parameters) { ELogSeverity severity = Severity[(int)group]; if (severity < ELogSeverity.Exception) { return; } if (OutputToUnity == false && SaveMessages == false) { return; } string formattedMessage = GetFormattedMessage(string.Format(format, parameters)); if (SaveMessages == true) { Messages.Add(formattedMessage); } if (OutputToUnity == true) { UnityEngine.Debug.LogError  (formattedMessage); } }

		public void SetSeverity(ELogGroup group, ELogSeverity severity)
		{
			Severity[(int)group] = severity;
		}

		public void SetSeverity(ELogSeverity severity)
		{
			for (int i = 0, count = Severity.Length; i < count; ++i)
			{
				Severity[i] = severity;
			}
		}

		//========== IService INTERFACE ===============================================================================

		void IService.Initialize(IServiceProvider serviceProvider)
		{
		}

		void IService.Deinitialize()
		{
			Messages.Clear();
		}

		void IService.Tick()
		{
		}

		//========== PRIVATE METHODS ==================================================================================

		private string GetFormattedMessage(string message)
		{
			StringBuilder builder = Pool<StringBuilder>.Get();
			builder.Clear();

			if (EnableFrameCount == true)
			{
				try
				{
					builder.AppendFormat("[{0}]", UnityEngine.Time.frameCount);
				}
				catch {}
			}

			if (EnableTimeStamp == true)
			{
				builder.AppendFormat("[{0:HH:mm:ss.fff}]", DateTime.Now);
			}

			if (EnableFrameCount == true || EnableTimeStamp == true)
			{
				builder.Append(" ");
			}

			builder.Append(message);

			string text = builder.ToString();

			builder.Clear();
			Pool<StringBuilder>.Return(builder);

			return text;
		}
	}
}
