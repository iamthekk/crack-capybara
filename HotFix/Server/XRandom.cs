using System;

namespace Server
{
	public class XRandom
	{
		public XRandom(int seed)
		{
			this._randSeed = (long)seed;
		}

		private uint Next()
		{
			this._randSeed = (this._randSeed * 25214903917L + 11L) & 281474976710655L;
			return (uint)(this._randSeed >> 16);
		}

		private uint Next(uint max)
		{
			return this.Next() % max;
		}

		private int Next(int max)
		{
			return (int)((ulong)this.Next() % (ulong)((long)max));
		}

		public uint Range(uint min, uint max)
		{
			uint num = max - min;
			return this.Next(num) + min;
		}

		public int Range(int min, int max)
		{
			if (min >= max - 1)
			{
				return min;
			}
			int num = max - min;
			return this.Next(num) + min;
		}

		public int NextInt()
		{
			return (int)this.Next();
		}

		public void SetSeed(uint seed)
		{
			this._randSeed = (long)((ulong)seed);
		}

		private const long multiplier = 25214903917L;

		private const long addend = 11L;

		private const long mask = 281474976710655L;

		private const int bits = 32;

		private const int maskBit = 48;

		private long _randSeed = 1L;
	}
}
