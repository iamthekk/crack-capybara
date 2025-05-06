using System;
using System.Diagnostics;
using System.Text;
using Framework;
using Framework.SDKManager;
using UnityEngine;
using UnityEngine.Internal;

public static class HLog
{
	static HLog()
	{
		Application.logMessageReceived -= new Application.LogCallback(HLog.HandleLog);
		Application.logMessageReceived += new Application.LogCallback(HLog.HandleLog);
		HLog.ConsoleEnabled = false;
	}

	private static void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (HLog.isConsole)
		{
			return;
		}
		if (type == 4)
		{
			HLog.isConsole = true;
			Exception ex = new Exception(HLog.StringBuilder("[Capture]", logString, "\n[Stack Trace]:\n", stackTrace ?? Environment.StackTrace));
			SDKManager sdk = GameApp.SDK;
			if (sdk != null)
			{
				SDKManager.SDKFirebase firebaseSDK = sdk.FirebaseSDK;
				if (firebaseSDK != null)
				{
					firebaseSDK.ReportException(ex);
				}
			}
			Debug.LogException(ex);
			HLog.isConsole = false;
		}
	}

	public static bool ConsoleEnabled
	{
		get
		{
			return Debug.unityLogger.filterLogType > 0;
		}
		set
		{
			if (value)
			{
				HLog.SetLogFilterType(3);
				return;
			}
			HLog.SetLogFilterType(0);
		}
	}

	public static bool IsConsole(LogType type)
	{
		return type >= Debug.unityLogger.filterLogType;
	}

	public static void SetLogFilterType(LogType type)
	{
		Debug.unityLogger.filterLogType = type;
	}

	[Conditional("UnityLog")]
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		if (HLog.ConsoleEnabled)
		{
			Debug.DrawLine(start, end);
		}
	}

	[Conditional("UnityLog")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		if (HLog.ConsoleEnabled)
		{
			Debug.DrawLine(start, end, color);
		}
	}

	[Conditional("UnityLog")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
	{
		if (HLog.ConsoleEnabled)
		{
			Debug.DrawLine(start, end, color, duration);
		}
	}

	[Conditional("UnityLog")]
	[ExcludeFromDocs]
	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	public static void LogStackTrace(string e)
	{
		HLog.IsReture();
	}

	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	[ExcludeFromDocs]
	public static void LogStackTrace(string e1, string e2)
	{
		HLog.IsReture();
	}

	[ExcludeFromDocs]
	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	public static void LogStackTrace(string e1, string e2, string e3)
	{
		HLog.IsReture();
	}

	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	[ExcludeFromDocs]
	public static void LogStackTrace(string e1, string e2, string e3, string e4)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogStackTrace(string e1, string e2, string e3, string e4, string e5)
	{
		HLog.IsReture();
	}

	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	public static void LogStackTrace(string e1, string e2, string e3, string e4, string e5, string e6)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogStackTrace(string e1, string e2, string e3, string e4, string e5, string e6, string e7)
	{
		HLog.IsReture();
	}

	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	public static void LogStackTrace(params string[] e)
	{
		if (HLog.IsReture())
		{
			return;
		}
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		for (int i = 0; i < e.Length; i++)
		{
			clearStringBuilder.Append(e[i]);
		}
		clearStringBuilder.Append("\n");
		clearStringBuilder.Append(Environment.StackTrace);
	}

	private static void Error(string e)
	{
		Debug.LogError(e);
		SDKManager sdk = GameApp.SDK;
		if (sdk == null)
		{
			return;
		}
		SDKManager.SDKFirebase firebaseSDK = sdk.FirebaseSDK;
		if (firebaseSDK == null)
		{
			return;
		}
		firebaseSDK.ReportCrashLog(HLog.StringBuilder(e, "\n", Environment.StackTrace));
	}

	public static void LogError(string e)
	{
		HLog.Error(HLog.StringBuilder("[Error]", e));
	}

	public static void LogError(string e1, string e2)
	{
		HLog.Error(HLog.StringBuilder("[Error]", e1, e2));
	}

	public static void LogError(string e1, string e2, string e3)
	{
		HLog.Error(HLog.StringBuilder("[Error]", e1, e2, e3));
	}

	public static void LogError(string e1, string e2, string e3, string e4)
	{
		HLog.Error(HLog.StringBuilder("[Error]", e1, e2, e3, e4));
	}

	public static void LogError(string e1, string e2, string e3, string e4, string e5)
	{
		HLog.Error(HLog.StringBuilder("[Error]", e1, e2, e3, e4, e5));
	}

	public static void LogError(string e1, string e2, string e3, string e4, string e5, string e6)
	{
		HLog.Error(HLog.StringBuilder("[Error]", e1, e2, e3, e4, e5, e6));
	}

	public static void LogError(string e1, string e2, string e3, string e4, string e5, string e6, string e7)
	{
		HLog.Error(HLog.StringBuilder("[Error]", e1, e2, e3, e4, e5, e6, e7));
	}

	public static void LogError(string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8)
	{
		HLog.Error(HLog.StringBuilder(new string[] { "[Error]", e1, e2, e3, e4, e5, e6, e7, e8 }));
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	private static void Warning(string e)
	{
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogWarning(string e)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogWarning(string e1, string e2)
	{
		HLog.IsReture();
	}

	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	public static void LogWarning(string e1, string e2, string e3)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	public static void LogWarning(string e1, string e2, string e3, string e4)
	{
		HLog.IsReture();
	}

	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	public static void LogWarning(string e1, string e2, string e3, string e4, string e5)
	{
		HLog.IsReture();
	}

	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	public static void LogWarning(string e1, string e2, string e3, string e4, string e5, string e6)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogWarning(string e1, string e2, string e3, string e4, string e5, string e6, string e7)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogWarning(string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8)
	{
		HLog.IsReture();
	}

	private static StringBuilder GetClearStringBuilder()
	{
		StringBuilder threadLocalStringBuilder = HLog._threadLocalStringBuilder;
		threadLocalStringBuilder.Clear();
		return threadLocalStringBuilder;
	}

	public static string StringBuilder(string e1, string e2)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.Append(e1 ?? "NULL");
		clearStringBuilder.Append(e2 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilder(string e1, string e2, string e3)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.Append(e1 ?? "NULL");
		clearStringBuilder.Append(e2 ?? "NULL");
		clearStringBuilder.Append(e3 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilder(string e1, string e2, string e3, string e4)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.Append(e1 ?? "NULL");
		clearStringBuilder.Append(e2 ?? "NULL");
		clearStringBuilder.Append(e3 ?? "NULL");
		clearStringBuilder.Append(e4 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilder(string e1, string e2, string e3, string e4, string e5)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.Append(e1 ?? "NULL");
		clearStringBuilder.Append(e2 ?? "NULL");
		clearStringBuilder.Append(e3 ?? "NULL");
		clearStringBuilder.Append(e4 ?? "NULL");
		clearStringBuilder.Append(e5 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilder(string e1, string e2, string e3, string e4, string e5, string e6)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.Append(e1 ?? "NULL");
		clearStringBuilder.Append(e2 ?? "NULL");
		clearStringBuilder.Append(e3 ?? "NULL");
		clearStringBuilder.Append(e4 ?? "NULL");
		clearStringBuilder.Append(e5 ?? "NULL");
		clearStringBuilder.Append(e6 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilder(string e1, string e2, string e3, string e4, string e5, string e6, string e7)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.Append(e1 ?? "NULL");
		clearStringBuilder.Append(e2 ?? "NULL");
		clearStringBuilder.Append(e3 ?? "NULL");
		clearStringBuilder.Append(e4 ?? "NULL");
		clearStringBuilder.Append(e5 ?? "NULL");
		clearStringBuilder.Append(e6 ?? "NULL");
		clearStringBuilder.Append(e7 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilder(string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.Append(e1 ?? "NULL");
		clearStringBuilder.Append(e2 ?? "NULL");
		clearStringBuilder.Append(e3 ?? "NULL");
		clearStringBuilder.Append(e4 ?? "NULL");
		clearStringBuilder.Append(e5 ?? "NULL");
		clearStringBuilder.Append(e6 ?? "NULL");
		clearStringBuilder.Append(e7 ?? "NULL");
		clearStringBuilder.Append(e8 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilder(params string[] e)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		for (int i = 0; i < e.Length; i++)
		{
			clearStringBuilder.Append(e[i] ?? "NULL");
		}
		return clearStringBuilder.ToString();
	}

	public static string StringBuilderFormat(string format, string s1)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.AppendFormat(format ?? "NULL", s1 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilderFormat(string format, string s1, string s2)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.AppendFormat(format ?? "NULL", s1 ?? "NULL", s2 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilderFormat(string format, string s1, string s2, string s3)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.AppendFormat(format ?? "NULL", s1 ?? "NULL", s2 ?? "NULL", s3 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	public static string StringBuilderFormat(string format, params object[] ss)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		clearStringBuilder.AppendFormat(format ?? "NULL", ss);
		return clearStringBuilder.ToString();
	}

	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	public static void Log(string e1, string e2)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	public static void Log(string e1, string e2, string e3)
	{
		HLog.IsReture();
	}

	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	public static void Log(string e1, string e2, string e3, string e4)
	{
		HLog.IsReture();
	}

	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	public static void Log(string e1, string e2, string e3, string e4, string e5)
	{
		HLog.IsReture();
	}

	[Conditional("FirebaseLog")]
	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	public static void Log(string e1, string e2, string e3, string e4, string e5, string e6)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	public static void Log(string e1, string e2, string e3, string e4, string e5, string e6, string e7)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	public static void Log(string e1, string e2, string e3, string e4, string e5, string e6, string e7, string e8)
	{
		HLog.IsReture();
	}

	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	public static void Log(params string[] e)
	{
		HLog.IsReture();
	}

	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	public static void Log(object e)
	{
	}

	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	public static void Log(string e)
	{
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogFormat(string format, string s1)
	{
		HLog.IsReture();
	}

	[Conditional("BATTLE_SERVER")]
	[Conditional("UnityLog")]
	[Conditional("FirebaseLog")]
	public static void LogFormat(string format, string s1, string s2)
	{
		HLog.IsReture();
	}

	[Conditional("FirebaseLog")]
	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	public static void LogFormat(string format, string s1, string s2, string s3)
	{
		HLog.IsReture();
	}

	[Conditional("UnityLog")]
	[Conditional("BATTLE_SERVER")]
	[Conditional("FirebaseLog")]
	public static void LogFormat(string format, params object[] ss)
	{
		HLog.IsReture();
	}

	public static void LogException(Exception e)
	{
		Debug.LogException(e);
		SDKManager sdk = GameApp.SDK;
		if (sdk == null)
		{
			return;
		}
		SDKManager.SDKFirebase firebaseSDK = sdk.FirebaseSDK;
		if (firebaseSDK == null)
		{
			return;
		}
		firebaseSDK.ReportException(e);
	}

	public static void LogException(Exception e, string custom)
	{
		HLog.LogError(custom);
		HLog.LogException(e);
	}

	public static void FirebaseSetUserId(string identifier)
	{
		SDKManager sdk = GameApp.SDK;
		if (sdk == null)
		{
			return;
		}
		SDKManager.SDKFirebase firebaseSDK = sdk.FirebaseSDK;
		if (firebaseSDK == null)
		{
			return;
		}
		firebaseSDK.SetUserId(identifier);
	}

	public static void FirebaseSetCustomKey(string key, string value)
	{
		SDKManager sdk = GameApp.SDK;
		if (sdk == null)
		{
			return;
		}
		SDKManager.SDKFirebase firebaseSDK = sdk.FirebaseSDK;
		if (firebaseSDK == null)
		{
			return;
		}
		firebaseSDK.SetCustomKey(key, value);
	}

	public static string ToColor(this string format, DebugColor ct = DebugColor.Default)
	{
		if (HLog.ConsoleEnabled)
		{
			switch (ct)
			{
			case DebugColor.White:
				format = HLog.StringBuilder("<color=#FFFFFF>", format, "</color>");
				break;
			case DebugColor.Red:
				format = HLog.StringBuilder("<color=#ff0000>", format, "</color>");
				break;
			case DebugColor.Yellow:
				format = HLog.StringBuilder("<color=#ffff00>", format, "</color>");
				break;
			case DebugColor.Green:
				format = HLog.StringBuilder("<color=#00ff00>", format, "</color>");
				break;
			case DebugColor.GreenLight:
				format = HLog.StringBuilder("<color=#00FA9A>", format, "</color>");
				break;
			case DebugColor.GreenBright:
				format = HLog.StringBuilder("<color=#00ffffff>", format, "</color>");
				break;
			case DebugColor.Blue:
				format = HLog.StringBuilder("<color=#1E90FF>", format, "</color>");
				break;
			case DebugColor.BlueSky:
				format = HLog.StringBuilder("<color=#00BFFF>", format, "</color>");
				break;
			case DebugColor.BlueLight:
				format = HLog.StringBuilder("<color=#98F5FF>", format, "</color>");
				break;
			case DebugColor.Orange:
				format = HLog.StringBuilder("<color=#e69138>", format, "</color>");
				break;
			case DebugColor.OrangeRed:
				format = HLog.StringBuilder("<color=#FF6A6A>", format, "</color>");
				break;
			case DebugColor.Violet:
				format = HLog.StringBuilder("<color=#8A2BE2>", format, "</color>");
				break;
			}
		}
		return format;
	}

	private static string StringBuilder(string[] ss, string s1)
	{
		StringBuilder clearStringBuilder = HLog.GetClearStringBuilder();
		for (int i = 0; i < ss.Length; i++)
		{
			clearStringBuilder.Append(ss[i] ?? "NULL");
		}
		clearStringBuilder.Append(s1 ?? "NULL");
		return clearStringBuilder.ToString();
	}

	private static bool IsReture()
	{
		bool flag = false;
		return !HLog.ConsoleEnabled && flag;
	}

	public static void CheckSerializedFieldReferencesForNull(this MonoBehaviour mono)
	{
	}

	private static StringBuilder _threadLocalStringBuilder = new StringBuilder();

	private static LogType logType = 3;

	private static bool isConsole = false;

	public const string ColorWhite = "<color=#FFFFFF>";

	public const string ColorRed = "<color=#ff0000>";

	public const string ColorYellow = "<color=#ffff00>";

	public const string ColorGreen = "<color=#00ff00>";

	public const string ColorGreenBright = "<color=#00ffffff>";

	public const string ColorGreenLight = "<color=#00FA9A>";

	public const string ColorBlue = "<color=#1E90FF>";

	public const string ColorBlueSky = "<color=#00BFFF>";

	public const string ColorBlueLight = "<color=#98F5FF>";

	public const string ColorViolet = "<color=#8A2BE2>";

	public const string ColorOrange = "<color=#e69138>";

	public const string ColorOrangeRed = "<color=#FF6A6A>";

	public const string ColorSymbol = "</color>";
}
