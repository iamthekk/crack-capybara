using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class ChapterRewardData
	{
		public bool IsNeedPass
		{
			get
			{
				return this.stage == this.Config.totalStage;
			}
		}

		public Chapter_chapter Config
		{
			get
			{
				if (this._config == null)
				{
					this._config = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(this.chapterId);
				}
				return this._config;
			}
		}

		public List<PropData> GetRewardDataList()
		{
			List<PropData> list = new List<PropData>();
			Item_drop elementById = GameApp.Table.GetManager().GetItem_dropModelInstance().GetElementById(this.rewardId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[Item_drop] not found id={0}", this.rewardId));
				return list;
			}
			for (int i = 0; i < elementById.reward.Length; i++)
			{
				uint num = Convert.ToUInt32(elementById.reward[i].Split(',', StringSplitOptions.None)[0]);
				uint num2 = Convert.ToUInt32(elementById.reward[i].Split(',', StringSplitOptions.None)[1]);
				list.Add(new PropData
				{
					id = num,
					count = (ulong)num2
				});
			}
			return list;
		}

		public int chapterId;

		public int stage;

		public int rewardId;

		public ChapterRewardData.ChapterRewardState state;

		private Chapter_chapter _config;

		public enum ChapterRewardState
		{
			Lock,
			CanGet,
			Finish
		}
	}
}
