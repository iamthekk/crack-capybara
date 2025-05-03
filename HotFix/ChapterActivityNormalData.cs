using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class ChapterActivityNormalData : ChapterActivityData
	{
		public ChapterActivity_ChapterActivity Config { get; private set; }

		protected override void SetTable(int actId)
		{
			this.Config = GameApp.Table.GetManager().GetChapterActivity_ChapterActivityModelInstance().GetElementById(actId);
			base.ScoreAtlasId = this.Config.atlasID;
			base.ScoreIcon = this.Config.itemIcon;
			base.ScoreNameId = this.Config.itemNameId;
			base.ActivityTitleId = this.Config.name;
		}

		public override List<ChapterActivity_ChapterObj> GetAllProgress()
		{
			List<ChapterActivity_ChapterObj> list = new List<ChapterActivity_ChapterObj>();
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
			return false;
		}
	}
}
