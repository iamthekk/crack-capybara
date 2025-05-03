using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class ChapterActivityRankData : ChapterActivityData
	{
		public ChapterActivity_RankActivity Config { get; private set; }

		protected override void SetTable(int actId)
		{
			this.Config = GameApp.Table.GetManager().GetChapterActivity_RankActivityModelInstance().GetElementById(actId);
			if (this.Config == null)
			{
				HLog.LogError(string.Format("未找到排行榜活动{0}", actId));
				return;
			}
			base.ScoreAtlasId = this.Config.atlasID;
			base.ScoreIcon = this.Config.itemIcon;
			base.ScoreNameId = this.Config.itemNameId;
			base.ActivityTitleId = this.Config.name;
		}

		public override List<ChapterActivity_ChapterObj> GetAllProgress()
		{
			List<ChapterActivity_ChapterObj> list = new List<ChapterActivity_ChapterObj>();
			if (this.Config == null)
			{
				return list;
			}
			IList<ChapterActivity_ChapterObj> allElements = GameApp.Table.GetManager().GetChapterActivity_ChapterObjModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].group == this.Config.group)
				{
					list.Add(allElements[i]);
				}
			}
			return list;
		}

		public override List<ItemData> GetTotalShowRewards()
		{
			List<ItemData> list = new List<ItemData>();
			if (this.Config == null)
			{
				return list;
			}
			for (int i = 0; i < this.Config.rewardShow.Length; i++)
			{
				List<int> listInt = this.Config.rewardShow[i].GetListInt(',');
				if (listInt.Count >= 2)
				{
					ItemData itemData = new ItemData(listInt[0], (long)listInt[1]);
					list.Add(itemData);
				}
			}
			return list;
		}

		public override bool IsHaveEndReward()
		{
			return this.Config != null && (base.IsEnd() && !this.isCollectReward && (ulong)base.TotalScore >= (ulong)((long)this.Config.unlockScore));
		}

		public void SetCollectedReward()
		{
			this.isCollectReward = true;
		}

		private bool isCollectReward;
	}
}
