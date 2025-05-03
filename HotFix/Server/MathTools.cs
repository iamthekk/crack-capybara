using System;

namespace Server
{
	public static class MathTools
	{
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}
			return value;
		}

		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}
			return value;
		}

		public static long Clamp(long value, long min, long max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}
			return value;
		}

		public static FP Clamp(FP value, FP min, FP max)
		{
			if (value < min)
			{
				value = min;
			}
			else if (value > max)
			{
				value = max;
			}
			return value;
		}

		public static int GetSymbol(string s)
		{
			if (s == "+")
			{
				return 1;
			}
			if (!(s == "-"))
			{
				return 0;
			}
			return -1;
		}

		public static string GetSymbolString(long value)
		{
			if (value < 0L)
			{
				return "-";
			}
			return "+";
		}

		public static string GetSymbolString(float value)
		{
			if (value < 0f)
			{
				return "-";
			}
			return "+";
		}

		public static long Abs(long value)
		{
			if (value <= 0L)
			{
				return -value;
			}
			return value;
		}

		public static float Min(float a, float b)
		{
			if (a <= b)
			{
				return a;
			}
			return b;
		}

		public static int Min(int a, int b)
		{
			if (a <= b)
			{
				return a;
			}
			return b;
		}

		public static long Min(long a, long b)
		{
			if (a <= b)
			{
				return a;
			}
			return b;
		}

		public static float Max(float a, float b)
		{
			if (a >= b)
			{
				return a;
			}
			return b;
		}

		public static int Max(int a, int b)
		{
			if (a >= b)
			{
				return a;
			}
			return b;
		}

		public static long Max(long a, long b)
		{
			if (a >= b)
			{
				return a;
			}
			return b;
		}

		public static string GetTime3String(long second)
		{
			return string.Format("{0:D2}:{1:D2}:{2:D2}", second / 3600L, second % 3600L / 60L, second % 60L);
		}

		public static string GetTime2String(long second)
		{
			return string.Format("{0:D2}:{1:D2}", second / 60L, second % 60L);
		}

		public static int Random(int min, int max)
		{
			return new Random().Next(min, max);
		}

		public static int CeilToInt(float value)
		{
			return (int)Math.Ceiling((double)value);
		}

		public static int CeilToInt(double value)
		{
			return (int)Math.Ceiling(value);
		}

		public static double Pow(double x, double y)
		{
			return Math.Pow(x, y);
		}

		public static long GetValue(this float value)
		{
			return ((double)value).GetValue();
		}

		public static long GetValue(this double value)
		{
			return (long)(Math.Round(value * 10.0) / 10.0);
		}
	}
}
