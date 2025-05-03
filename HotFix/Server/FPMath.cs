using System;

namespace Server
{
	public sealed class FPMath
	{
		public static FP Sqrt(FP number)
		{
			return FP.Sqrt(number);
		}

		public static FP Max(FP val1, FP val2)
		{
			if (!(val1 > val2))
			{
				return val2;
			}
			return val1;
		}

		public static FP Min(FP val1, FP val2)
		{
			if (!(val1 < val2))
			{
				return val2;
			}
			return val1;
		}

		public static FP Max(FP val1, FP val2, FP val3)
		{
			FP fp = ((val1 > val2) ? val1 : val2);
			if (!(fp > val3))
			{
				return val3;
			}
			return fp;
		}

		public static FP Clamp(FP value, FP min, FP max)
		{
			if (value < min)
			{
				value = min;
				return value;
			}
			if (value > max)
			{
				value = max;
			}
			return value;
		}

		public static FP Clamp01(FP value)
		{
			if (value < FP._0)
			{
				return FP._0;
			}
			if (value > FP._1)
			{
				return FP._1;
			}
			return value;
		}

		public static FP Floor(FP value)
		{
			return FP.Floor(value);
		}

		public static FP Ceiling(FP value)
		{
			return value.CeilToLong();
		}

		public static FP Round(FP value)
		{
			return FP.Round(value);
		}

		public static FP Sin(FP value)
		{
			return FP.Sin(value);
		}

		public static FP Cos(FP value)
		{
			return FP.Cos(value);
		}

		public static FP Tan(FP value)
		{
			return FP.Tan(value);
		}

		public static int Sign(FP value)
		{
			if (value > FP._0)
			{
				return 1;
			}
			if (value < FP._0)
			{
				return -1;
			}
			return 0;
		}

		public static FP Abs(FP value)
		{
			return FP.Abs(value);
		}

		public static FP Distance(FP value1, FP value2)
		{
			return FP.Abs(value1 - value2);
		}

		public static FP Lerp(FP value1, FP value2, FP amount)
		{
			return value1 + (value2 - value1) * FPMath.Clamp01(amount);
		}

		public static FP InverseLerp(FP value1, FP value2, FP amount)
		{
			if (value1 != value2)
			{
				return FPMath.Clamp01((amount - value1) / (value2 - value1));
			}
			return FP._0;
		}
	}
}
