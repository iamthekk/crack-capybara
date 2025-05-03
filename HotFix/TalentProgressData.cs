using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
	public class TalentProgressData
	{
		public float GetProgress()
		{
			int num = this.maxLevel - this.startLevel;
			if (num == 0)
			{
				return 0f;
			}
			return Mathf.Clamp01((float)(this.curLevel - this.startLevel) * 1f / (float)num);
		}

		public List<int> cacheIds = new List<int>();

		public int curId;

		public int curLevel;

		public int startLevel;

		public int maxLevel;

		public List<TalentProgressRewardData> rewardList = new List<TalentProgressRewardData>();

		public bool isTheEndProgress;
	}
}
