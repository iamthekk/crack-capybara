using System;
using Proto.Common;

namespace Server
{
	public class BattleServerLog
	{
		public string LogID = string.Empty;

		public int WaveIndex;

		public int Seed;

		public int CurHp;

		public int ReviveCount;

		public BattleUserDto UserInfo;
	}
}
