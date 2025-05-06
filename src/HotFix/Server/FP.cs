using System;
using System.Globalization;

namespace Server
{
	[Serializable]
	public struct FP : IComparable<FP>, IEquatable<FP>
	{
		public int Sign
		{
			get
			{
				if (this.integerPart < 0L || (this.integerPart == 0L && this.fractionalPart < 0L))
				{
					return -1;
				}
				if (this.integerPart != 0L || this.fractionalPart != 0L)
				{
					return 1;
				}
				return 0;
			}
		}

		public static FP PI
		{
			get
			{
				return new FP(3.1415926535897931);
			}
		}

		private static FP Epsilon
		{
			get
			{
				return new FP(0.25f);
			}
		}

		private static FP MathMaxValue
		{
			get
			{
				return new FP(long.MaxValue / FP.FractionalBase, 1L);
			}
		}

		private static FP MathMinValue
		{
			get
			{
				return new FP(long.MinValue / FP.FractionalBase, 1L);
			}
		}

		public static FP MaxValue
		{
			get
			{
				return new FP(long.MaxValue, 1L)
				{
					fractionalPart = FP.FractionalBase - 1L
				};
			}
		}

		public FP(long numerator, long denominator)
		{
			if (denominator == 0L)
			{
				throw new DivideByZeroException("Denominator cannot be zero.");
			}
			this.integerPart = numerator / denominator;
			this.fractionalPart = numerator % denominator * FP.FractionalBase / denominator;
			if (numerator < 0L)
			{
				this.integerPart = -Math.Abs(this.integerPart);
				this.fractionalPart = -Math.Abs(this.fractionalPart);
			}
		}

		public FP(int value)
		{
			this = new FP((long)value, 1L);
		}

		public FP(long value)
		{
			this = new FP(value, 1L);
		}

		public FP(ulong value)
		{
			this = new FP((long)value, 1L);
		}

		public FP(float value)
		{
			this = new FP(value.ToString(CultureInfo.InvariantCulture));
		}

		public FP(double value)
		{
			this = new FP(value.ToString(CultureInfo.InvariantCulture));
		}

		public FP(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new FormatException("Invalid string format.");
			}
			bool flag = value[0] == '-';
			if (flag)
			{
				value = value.Substring(1);
			}
			string[] array = value.Split('.', StringSplitOptions.None);
			if (array.Length > 2)
			{
				throw new FormatException("Invalid string format.");
			}
			this.integerPart = long.Parse(array[0]);
			if (array.Length == 2)
			{
				string text = array[1].PadRight(4, '0').Substring(0, 4);
				this.fractionalPart = long.Parse(text);
			}
			else
			{
				this.fractionalPart = 0L;
			}
			if (flag)
			{
				this.integerPart = -Math.Abs(this.integerPart);
				this.fractionalPart = -Math.Abs(this.fractionalPart);
			}
		}

		public static implicit operator FP(int value)
		{
			return new FP(value);
		}

		public static implicit operator FP(long value)
		{
			return new FP(value);
		}

		public static implicit operator FP(ulong value)
		{
			return new FP(value);
		}

		public static implicit operator FP(float value)
		{
			return new FP(value);
		}

		public static implicit operator FP(double value)
		{
			return new FP(value.ToString(CultureInfo.InvariantCulture));
		}

		public static explicit operator double(FP value)
		{
			return (double)value.integerPart + (double)value.fractionalPart / (double)FP.FractionalBase;
		}

		public static explicit operator int(FP value)
		{
			return (int)(value.integerPart + ((value.fractionalPart >= FP.FractionalBase / 2L) ? 1L : 0L));
		}

		public static explicit operator long(FP value)
		{
			return value.integerPart + ((value.fractionalPart >= FP.FractionalBase / 2L) ? 1L : 0L);
		}

		public static explicit operator float(FP value)
		{
			return (float)((double)value.integerPart + (double)value.fractionalPart / (double)FP.FractionalBase);
		}

		public override string ToString()
		{
			return ((double)this).ToString("0.####");
		}

		public string ToString(IFormatProvider provider)
		{
			return ((double)this).ToString(provider);
		}

		public string ToString(string format)
		{
			return ((double)this).ToString(format);
		}

		public static FP FromString(string value)
		{
			return new FP(value);
		}

		public static bool TryFromString(string value, out FP result)
		{
			result = FP._0;
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			bool flag = value[0] == '-';
			if (flag)
			{
				value = value.Substring(1);
			}
			string[] array = value.Split('.', StringSplitOptions.None);
			if (array.Length > 2)
			{
				return false;
			}
			long num = long.Parse(array[0]);
			long num2;
			if (array.Length == 2)
			{
				num2 = long.Parse(array[1].PadRight(4, '0').Substring(0, 4));
			}
			else
			{
				num2 = 0L;
			}
			if (flag)
			{
				num = -Math.Abs(num);
				num2 = -Math.Abs(num2);
			}
			result.integerPart = num;
			result.fractionalPart = num2;
			return true;
		}

		public static FP operator +(FP a, FP b)
		{
			long num = a.integerPart + b.integerPart;
			long num2 = a.fractionalPart + b.fractionalPart;
			if (num2 >= FP.FractionalBase)
			{
				num += 1L;
				num2 -= FP.FractionalBase;
			}
			if (num2 <= -FP.FractionalBase)
			{
				num -= 1L;
				num2 += FP.FractionalBase;
			}
			return FP.GenerateNewFP(num, num2);
		}

		public static FP operator -(FP a, FP b)
		{
			long num = a.integerPart + -b.integerPart;
			long num2 = a.fractionalPart + -b.fractionalPart;
			if (num2 >= FP.FractionalBase)
			{
				num += 1L;
				num2 -= FP.FractionalBase;
			}
			if (num2 <= -FP.FractionalBase)
			{
				num -= 1L;
				num2 += FP.FractionalBase;
			}
			return FP.GenerateNewFP(num, num2);
		}

		public static FP operator *(FP a, FP b)
		{
			long num = a.integerPart * b.integerPart;
			long num2 = a.integerPart * b.fractionalPart + a.fractionalPart * b.integerPart;
			long num3 = a.fractionalPart * b.fractionalPart / FP.FractionalBase;
			num += num2 / FP.FractionalBase;
			long num4 = num2 % FP.FractionalBase + num3;
			if (num4 >= FP.FractionalBase)
			{
				num += 1L;
				num4 -= FP.FractionalBase;
			}
			if (num3 <= -FP.FractionalBase)
			{
				num -= 1L;
				num4 += FP.FractionalBase;
			}
			if (a.Sign * b.Sign < 0)
			{
				num = -Math.Abs(num);
				num4 = -Math.Abs(num4);
			}
			else
			{
				num = Math.Abs(num);
				num4 = Math.Abs(num4);
			}
			return FP.GenerateNewFP(num, num4);
		}

		public static FP operator /(FP a, FP b)
		{
			if (b.integerPart == 0L && b.fractionalPart == 0L)
			{
				throw new DivideByZeroException("Denominator cannot be zero.");
			}
			long num = a.integerPart * FP.FractionalBase + a.fractionalPart;
			long num2 = b.integerPart * FP.FractionalBase + b.fractionalPart;
			long num3 = num / num2;
			long num4 = num % num2 * FP.FractionalBase / num2;
			if (a.Sign * b.Sign < 0)
			{
				num3 = -Math.Abs(num3);
				num4 = -Math.Abs(num4);
			}
			else
			{
				num3 = Math.Abs(num3);
				num4 = Math.Abs(num4);
			}
			return FP.GenerateNewFP(num3, num4);
		}

		public static FP operator ^(FP baseValue, int exponent)
		{
			return FP.Pow(baseValue, exponent);
		}

		public static FP operator %(FP a, FP b)
		{
			if (b.integerPart == 0L && b.fractionalPart == 0L)
			{
				throw new DivideByZeroException("Denominator cannot be zero.");
			}
			long num = a.integerPart * FP.FractionalBase + a.fractionalPart;
			long num2 = b.integerPart * FP.FractionalBase + b.fractionalPart;
			long num3 = num % num2;
			long num4;
			if (a.Sign < 0)
			{
				num3 = -Math.Abs(num3);
				num4 = -Math.Abs(num3 % FP.FractionalBase);
			}
			else
			{
				num3 = Math.Abs(num3);
				num4 = Math.Abs(num3 % FP.FractionalBase);
			}
			num3 /= FP.FractionalBase;
			return FP.GenerateNewFP(num3, num4);
		}

		public static FP operator -(FP value)
		{
			return new FP(-value.integerPart, 1L)
			{
				fractionalPart = -value.fractionalPart
			};
		}

		public static FP operator ++(FP a)
		{
			return a + FP._1;
		}

		public static FP operator --(FP a)
		{
			return a - FP._1;
		}

		public static bool operator <(FP a, FP b)
		{
			if (a.Sign != b.Sign)
			{
				return a.Sign < b.Sign;
			}
			if (a.integerPart != b.integerPart)
			{
				return a.integerPart < b.integerPart;
			}
			return a.fractionalPart < b.fractionalPart;
		}

		public static bool operator <=(FP a, FP b)
		{
			return a < b || a == b;
		}

		public static bool operator >(FP a, FP b)
		{
			return !(a <= b);
		}

		public static bool operator >=(FP a, FP b)
		{
			return !(a < b);
		}

		public static bool operator ==(FP a, FP b)
		{
			return a.integerPart == b.integerPart && a.fractionalPart == b.fractionalPart;
		}

		public static bool operator !=(FP a, FP b)
		{
			return !(a == b);
		}

		public int CompareTo(FP other)
		{
			if (this < other)
			{
				return -1;
			}
			if (!(this > other))
			{
				return 0;
			}
			return 1;
		}

		public bool Equals(FP other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (obj is FP)
			{
				FP fp = (FP)obj;
				return this == fp;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return new ValueTuple<long, long>(this.integerPart, this.fractionalPart).GetHashCode();
		}

		public FP Abs()
		{
			this.integerPart = Math.Abs(this.integerPart);
			this.fractionalPart = Math.Abs(this.fractionalPart);
			return this;
		}

		public static FP Abs(FP value)
		{
			return new FP
			{
				integerPart = Math.Abs(value.integerPart),
				fractionalPart = Math.Abs(value.fractionalPart)
			};
		}

		public static FP Pow(FP baseValue, int exponent)
		{
			if (exponent < 0)
			{
				throw new ArgumentOutOfRangeException("Exponent cannot be negative.");
			}
			FP fp = new FP(1);
			FP fp2 = baseValue;
			while (exponent != 0)
			{
				if ((exponent & 1) != 0)
				{
					fp *= fp2;
				}
				fp2 *= fp2;
				exponent >>= 1;
			}
			return fp;
		}

		public static FP Sqrt(FP value)
		{
			if (value.Sign < 0)
			{
				throw new ArgumentException("Cannot calculate square root of a negative value.");
			}
			FP fp = new FP(1);
			while (FP.Abs(fp * fp - value) > FP.Epsilon)
			{
				fp = (fp + value / fp) / 2;
			}
			return fp;
		}

		public static FP Sin(FP value)
		{
			FP fp = value;
			FP fp2 = value;
			FP fp3 = value * value;
			bool flag = false;
			for (int i = 3; i < 9; i += 2)
			{
				fp2 *= fp3;
				fp2 /= new FP((long)(i * (i - 1)), 1L);
				if (flag)
				{
					fp += fp2;
				}
				else
				{
					fp -= fp2;
				}
				flag = !flag;
			}
			return fp;
		}

		public static FP Cos(FP value)
		{
			FP fp = FP._1;
			FP fp2 = FP._1;
			FP fp3 = value * value;
			bool flag = false;
			for (int i = 2; i < 9; i += 2)
			{
				fp2 *= fp3;
				fp2 /= new FP((long)(i * (i - 1)), 1L);
				if (flag)
				{
					fp += fp2;
				}
				else
				{
					fp -= fp2;
				}
				flag = !flag;
			}
			return fp;
		}

		public static FP Tan(FP value)
		{
			return FP.Sin(value) / FP.Cos(value);
		}

		public static FP Ceil(FP value)
		{
			return new FP(value.integerPart + ((value.fractionalPart > 0L) ? 1L : 0L));
		}

		public static FP Floor(FP value)
		{
			return new FP(value.integerPart + ((value.fractionalPart < 0L) ? (-1L) : 0L));
		}

		public static FP Round(FP value)
		{
			return new FP(value.integerPart + ((Math.Abs(value.fractionalPart) > FP.FractionalBase / 2L) ? ((value.fractionalPart <= 0L) ? (-1L) : 1L) : 0L));
		}

		public long RoundToLong()
		{
			return (long)FP.Round(this);
		}

		public int RoundToInt()
		{
			return (int)FP.Round(this);
		}

		public long CeilToLong()
		{
			return (long)FP.Ceil(this);
		}

		public int CeilToInt()
		{
			return (int)FP.Ceil(this);
		}

		public long FloorToLong()
		{
			return (long)FP.Floor(this);
		}

		public int FloorToInt()
		{
			return (int)FP.Floor(this);
		}

		public float NormalizeToFloat(int floatCount = 2)
		{
			return (float)Math.Round((double)(float)this, floatCount, MidpointRounding.AwayFromZero);
		}

		public long AsLong()
		{
			return (long)this;
		}

		public int AsInt()
		{
			return (int)this;
		}

		public float AsFloat()
		{
			return (float)this;
		}

		public double AsDouble()
		{
			return (double)this;
		}

		public static FP GenerateNewFP(long integerPart, long fractionalPart)
		{
			if (integerPart > 0L && fractionalPart < 0L)
			{
				integerPart -= 1L;
				fractionalPart += FP.FractionalBase;
			}
			else if (integerPart < 0L && fractionalPart > 0L)
			{
				integerPart += 1L;
				fractionalPart -= FP.FractionalBase;
			}
			return new FP
			{
				integerPart = integerPart,
				fractionalPart = fractionalPart
			};
		}

		private const int FractionalDigits = 4;

		public static readonly long FractionalBase = (long)Math.Pow(10.0, 4.0);

		private long integerPart;

		private long fractionalPart;

		public static readonly FP _0 = 0;

		public static readonly FP _001 = new FP(1L, 100L);

		public static readonly FP _002 = new FP(2L, 100L);

		public static readonly FP _003 = new FP(3L, 100L);

		public static readonly FP _004 = new FP(4L, 100L);

		public static readonly FP _005 = new FP(5L, 100L);

		public static readonly FP _006 = new FP(6L, 100L);

		public static readonly FP _007 = new FP(7L, 100L);

		public static readonly FP _008 = new FP(8L, 100L);

		public static readonly FP _009 = new FP(9L, 100L);

		public static readonly FP _010 = new FP(10L, 100L);

		public static readonly FP _011 = new FP(11L, 100L);

		public static readonly FP _012 = new FP(12L, 100L);

		public static readonly FP _013 = new FP(13L, 100L);

		public static readonly FP _014 = new FP(14L, 100L);

		public static readonly FP _015 = new FP(15L, 100L);

		public static readonly FP _016 = new FP(16L, 100L);

		public static readonly FP _017 = new FP(17L, 100L);

		public static readonly FP _018 = new FP(18L, 100L);

		public static readonly FP _019 = new FP(19L, 100L);

		public static readonly FP _020 = new FP(20L, 100L);

		public static readonly FP _021 = new FP(21L, 100L);

		public static readonly FP _022 = new FP(22L, 100L);

		public static readonly FP _023 = new FP(23L, 100L);

		public static readonly FP _024 = new FP(24L, 100L);

		public static readonly FP _025 = new FP(25L, 100L);

		public static readonly FP _026 = new FP(26L, 100L);

		public static readonly FP _027 = new FP(27L, 100L);

		public static readonly FP _028 = new FP(28L, 100L);

		public static readonly FP _029 = new FP(29L, 100L);

		public static readonly FP _030 = new FP(30L, 100L);

		public static readonly FP _031 = new FP(31L, 100L);

		public static readonly FP _032 = new FP(32L, 100L);

		public static readonly FP _033 = new FP(33L, 100L);

		public static readonly FP _034 = new FP(34L, 100L);

		public static readonly FP _035 = new FP(35L, 100L);

		public static readonly FP _036 = new FP(36L, 100L);

		public static readonly FP _037 = new FP(37L, 100L);

		public static readonly FP _038 = new FP(38L, 100L);

		public static readonly FP _039 = new FP(39L, 100L);

		public static readonly FP _040 = new FP(40L, 100L);

		public static readonly FP _041 = new FP(41L, 100L);

		public static readonly FP _042 = new FP(42L, 100L);

		public static readonly FP _043 = new FP(43L, 100L);

		public static readonly FP _044 = new FP(44L, 100L);

		public static readonly FP _045 = new FP(45L, 100L);

		public static readonly FP _046 = new FP(46L, 100L);

		public static readonly FP _047 = new FP(47L, 100L);

		public static readonly FP _048 = new FP(48L, 100L);

		public static readonly FP _049 = new FP(49L, 100L);

		public static readonly FP _050 = new FP(50L, 100L);

		public static readonly FP _051 = new FP(51L, 100L);

		public static readonly FP _052 = new FP(52L, 100L);

		public static readonly FP _053 = new FP(53L, 100L);

		public static readonly FP _054 = new FP(54L, 100L);

		public static readonly FP _055 = new FP(55L, 100L);

		public static readonly FP _056 = new FP(56L, 100L);

		public static readonly FP _057 = new FP(57L, 100L);

		public static readonly FP _058 = new FP(58L, 100L);

		public static readonly FP _059 = new FP(59L, 100L);

		public static readonly FP _060 = new FP(60L, 100L);

		public static readonly FP _061 = new FP(61L, 100L);

		public static readonly FP _062 = new FP(62L, 100L);

		public static readonly FP _063 = new FP(63L, 100L);

		public static readonly FP _064 = new FP(64L, 100L);

		public static readonly FP _065 = new FP(65L, 100L);

		public static readonly FP _066 = new FP(66L, 100L);

		public static readonly FP _067 = new FP(67L, 100L);

		public static readonly FP _068 = new FP(68L, 100L);

		public static readonly FP _069 = new FP(69L, 100L);

		public static readonly FP _070 = new FP(70L, 100L);

		public static readonly FP _071 = new FP(71L, 100L);

		public static readonly FP _072 = new FP(72L, 100L);

		public static readonly FP _073 = new FP(73L, 100L);

		public static readonly FP _074 = new FP(74L, 100L);

		public static readonly FP _075 = new FP(75L, 100L);

		public static readonly FP _076 = new FP(76L, 100L);

		public static readonly FP _077 = new FP(77L, 100L);

		public static readonly FP _078 = new FP(78L, 100L);

		public static readonly FP _079 = new FP(79L, 100L);

		public static readonly FP _080 = new FP(80L, 100L);

		public static readonly FP _081 = new FP(81L, 100L);

		public static readonly FP _082 = new FP(82L, 100L);

		public static readonly FP _083 = new FP(83L, 100L);

		public static readonly FP _084 = new FP(84L, 100L);

		public static readonly FP _085 = new FP(85L, 100L);

		public static readonly FP _086 = new FP(86L, 100L);

		public static readonly FP _087 = new FP(87L, 100L);

		public static readonly FP _088 = new FP(88L, 100L);

		public static readonly FP _089 = new FP(89L, 100L);

		public static readonly FP _090 = new FP(90L, 100L);

		public static readonly FP _091 = new FP(91L, 100L);

		public static readonly FP _092 = new FP(92L, 100L);

		public static readonly FP _093 = new FP(93L, 100L);

		public static readonly FP _094 = new FP(94L, 100L);

		public static readonly FP _095 = new FP(95L, 100L);

		public static readonly FP _096 = new FP(96L, 100L);

		public static readonly FP _097 = new FP(97L, 100L);

		public static readonly FP _098 = new FP(98L, 100L);

		public static readonly FP _099 = new FP(99L, 100L);

		public static readonly FP _1 = 1;

		public static readonly FP _2 = 2;

		public static readonly FP _3 = 3;

		public static readonly FP _4 = 4;

		public static readonly FP _5 = 5;

		public static readonly FP _6 = 6;

		public static readonly FP _7 = 7;

		public static readonly FP _8 = 8;

		public static readonly FP _9 = 9;

		public static readonly FP _10 = 10;

		public static readonly FP _1_5 = new FP(15L, 10L);

		public static readonly FP _9_99 = new FP(999L, 100L);

		public static readonly FP _100 = 100;

		public static readonly FP _1000 = 1000;

		public static readonly FP _10000 = 10000;
	}
}
