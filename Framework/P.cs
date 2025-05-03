using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;

public static class P
{
	[Conditional("ProfilerControl")]
	public static void OpenProfiler(string profilerName)
	{
		Profiler.enabled = true;
	}

	[Conditional("ProfilerControl")]
	public static void CloseProfiler()
	{
		Profiler.enabled = false;
	}

	[Conditional("ProfilerBinaryLog")]
	public static void OpenBinaryLog()
	{
		Profiler.logFile = Application.persistentDataPath + "/profilerLog.raw";
		Profiler.enableBinaryLog = true;
	}

	[Conditional("ProfilerBinaryLog")]
	public static void CloseBinaryLog()
	{
		Profiler.enableBinaryLog = false;
		Profiler.logFile = null;
	}

	[Conditional("Profiler")]
	public static void BeginSample(string name)
	{
	}

	[Conditional("Profiler")]
	public static void EndSample()
	{
	}

	[Conditional("ProfilerGetType")]
	public static void BeginSampleGetType(string title, object o, string tag)
	{
	}

	[Conditional("ProfilerGetType")]
	public static void BeginSampleGetType(object o, string tag)
	{
	}

	[Conditional("ProfilerGetType")]
	public static void EndSampleGetType()
	{
	}

	[Conditional("ProfilerGetType")]
	public static void BeginSampleDelegate(string tag, Delegate handler)
	{
		if (handler == null)
		{
			return;
		}
		MethodInfo method = handler.Method;
		object target = handler.Target;
		Type type = ((target != null) ? target.GetType() : null);
		string text = ((method != null) ? method.Name : null);
		string text2;
		if (type != null)
		{
			text2 = type.Name;
		}
		else
		{
			text2 = "Static";
		}
		text2 = HLog.StringBuilder(text2, ":", text);
	}

	[Conditional("ProfilerGetType")]
	public static void EndSampleDelegate()
	{
	}
}
