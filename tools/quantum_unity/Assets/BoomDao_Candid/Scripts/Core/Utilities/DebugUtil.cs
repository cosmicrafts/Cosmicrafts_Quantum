using UnityEngine;

namespace Boom.Utility
{
    public static class DebugUtil
    {
        public static bool enableBoomLogs = true;

        public static void Log(this string value, string source = "")
        {
            if (enableBoomLogs == false) return;
            Debug.Log($"> [DebugUtil] Message = from: {source}, content: {value}");
        }

        public static void Warning(this string value, string source = "")
        {
            if (enableBoomLogs == false) return;
            Debug.LogWarning($"> [DebugUtil] Warning = from: {source}, content: {value}");
        }

        public static void Error(this string value, string source = "")
        {
            if (enableBoomLogs == false) return;
            Debug.LogError($"> [DebugUtil] Error = from: {source}, content: {value}");
        }

        public static void Log<Source>(this string value)
        {
            value.Log(typeof(Source).Name);
        }

        public static void Warning<Source>(this string value)
        {
            value.Warning(typeof(Source).Name);
        }

        public static void Error<Source>(this string value)
        {
            value.Error(nameof(Source));
        }
    }
}
