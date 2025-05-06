using System;

namespace HotFix
{
	public static class MiniGameUtils
	{
		public static NodeAttParam GetNodeAttParam(string rewardStr, ChapterDropSource source, int rate)
		{
			return MiniGameUtils.GetNodeAttParam(rewardStr.Split('|', StringSplitOptions.None), source, rate);
		}

		public static NodeAttParam GetNodeAttParam(string[] rewardParams, ChapterDropSource source, int rate)
		{
			if (rewardParams == null || rewardParams.Length != 3)
			{
				return null;
			}
			long num = long.Parse(rewardParams[2]);
			GameEventAttType gameEventAttType;
			Enum.TryParse<GameEventAttType>(rewardParams[1], out gameEventAttType);
			return new NodeAttParam(gameEventAttType, (double)num, source, rate);
		}

		public static GameEventSkillBuildData GetSkillBuildData(string rewardStr, int seed)
		{
			return MiniGameUtils.GetSkillBuildData(rewardStr.Split('|', StringSplitOptions.None), seed);
		}

		public static GameEventSkillBuildData GetSkillBuildData(string[] rewardParams, int seed)
		{
			if (rewardParams == null || rewardParams.Length != 3)
			{
				return null;
			}
			return Singleton<GameEventController>.Instance.GetRandomSkillList((SkillBuildSourceType)int.Parse(rewardParams[1]), 1, seed)[0];
		}

		public static NodeItemParam GetNodeItemParam(string rewardStr, NodeItemType type, ChapterDropSource source, int rate)
		{
			return MiniGameUtils.GetNodeItemParam(rewardStr.Split('|', StringSplitOptions.None), type, source, rate);
		}

		public static NodeItemParam GetNodeItemParam(string[] rewardParams, NodeItemType type, ChapterDropSource source, int rate)
		{
			if (rewardParams == null || rewardParams.Length != 3)
			{
				return null;
			}
			return new NodeItemParam(type, int.Parse(rewardParams[1]), long.Parse(rewardParams[2]), source, rate);
		}
	}
}
