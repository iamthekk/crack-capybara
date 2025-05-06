using System;
using System.Collections.Generic;

namespace HotFix
{
	public class IAPBattlePassData
	{
		public int ScoreStart
		{
			get
			{
				if (this.LastScore >= this.Score)
				{
					return this.Score;
				}
				return (int)((float)(this.Score - this.LastScore) * 0.5f) + this.LastScore;
			}
		}

		public int ScoreEnd
		{
			get
			{
				if (this.NextScore <= this.Score)
				{
					return this.Score;
				}
				return (int)((float)(this.NextScore - this.Score) * 0.5f) + this.Score;
			}
		}

		public PropData FreeReward
		{
			get
			{
				if (this.FreeRewards == null || this.FreeRewards.Count <= 0)
				{
					return null;
				}
				return this.FreeRewards[0];
			}
		}

		public PropData CreateJumpShowData()
		{
			if (this.mJumpShowData != null)
			{
				return this.mJumpShowData;
			}
			PropData propData;
			if (this.PayRewards == null || this.PayRewards.Count < 0)
			{
				propData = this.FreeReward;
			}
			else
			{
				propData = this.PayRewards[0];
			}
			this.mJumpShowData = propData;
			return this.mJumpShowData;
		}

		public static int Sort(IAPBattlePassData a, IAPBattlePassData b)
		{
			int num = a.IsFinal.CompareTo(b.IsFinal);
			if (num == 0)
			{
				num = a.Score.CompareTo(b.Score);
			}
			return num;
		}

		public int ID;

		public int Level;

		public BattlePassType Type;

		public List<PropData> FreeRewards;

		public List<PropData> PayRewards;

		public int Score;

		public int LastScore;

		public int NextScore;

		public int Index;

		public bool IsFinal;

		private PropData mJumpShowData;
	}
}
