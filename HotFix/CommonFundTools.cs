using System;
using System.Collections.Generic;

namespace HotFix
{
	public static class CommonFundTools
	{
		public static int GetStageScore(List<CommonFundUIData> list, int totalScore, int getFinalCount)
		{
			if (list == null)
			{
				return 0;
			}
			if (totalScore == 0)
			{
				return 0;
			}
			int loopStartScore = CommonFundTools.GetLoopStartScore(list);
			if (loopStartScore > 0 && totalScore > loopStartScore)
			{
				int num = totalScore - loopStartScore;
				CommonFundUIData finalLoopData = CommonFundTools.GetFinalLoopData(list);
				if (finalLoopData != null)
				{
					num -= finalLoopData.Score * getFinalCount;
				}
				return num;
			}
			CommonFundUIData stageData = CommonFundTools.GetStageData(list, totalScore);
			if (stageData != null)
			{
				return totalScore - stageData.PreviousScore;
			}
			return totalScore;
		}

		public static CommonFundUIData GetStageData(List<CommonFundUIData> list, int totalScore)
		{
			if (list == null)
			{
				return null;
			}
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsLoopReward)
				{
					flag = true;
					break;
				}
			}
			int loopStartScore = CommonFundTools.GetLoopStartScore(list);
			if (flag && loopStartScore > 0 && totalScore >= loopStartScore)
			{
				return CommonFundTools.GetFinalLoopData(list);
			}
			for (int j = 0; j < list.Count; j++)
			{
				CommonFundUIData commonFundUIData = list[j];
				if (totalScore < commonFundUIData.Score)
				{
					return commonFundUIData;
				}
			}
			if (!flag)
			{
				return list[list.Count - 1];
			}
			return null;
		}

		public static int GetLoopStartScore(List<CommonFundUIData> list)
		{
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				CommonFundUIData commonFundUIData = list[i];
				if (!commonFundUIData.IsLoopReward && commonFundUIData.Score > num)
				{
					num = commonFundUIData.Score;
				}
			}
			return num;
		}

		public static CommonFundUIData GetLastNormalData(List<CommonFundUIData> list)
		{
			if (list == null)
			{
				return null;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				CommonFundUIData commonFundUIData = list[i];
				if (!commonFundUIData.IsLoopReward)
				{
					return commonFundUIData;
				}
			}
			return null;
		}

		public static CommonFundUIData GetFinalLoopData(List<CommonFundUIData> list)
		{
			if (list == null)
			{
				return null;
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				CommonFundUIData commonFundUIData = list[i];
				if (commonFundUIData.IsLoopReward)
				{
					return commonFundUIData;
				}
			}
			return null;
		}
	}
}
