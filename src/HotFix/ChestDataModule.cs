using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class ChestDataModule : IDataModule
	{
		public int GetName()
		{
			return 153;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
			this.curRewardType = 0;
			this.curRewardConfigId = 0;
			this.curScore = 0L;
			this.maxScore = 0L;
		}

		public bool HasRewardCanGet()
		{
			return this.curScore >= this.maxScore;
		}

		public long GetChestCount(int chestItemId)
		{
			return GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)chestItemId));
		}

		public long GetCurScore()
		{
			return this.curScore;
		}

		public void UpdateChestInfo(ChestInfo chestInfo)
		{
			int lastRewardType = (int)chestInfo.LastRewardType;
			int lastRewardConfigId = (int)chestInfo.LastRewardConfigId;
			IList<ChestList_ChestList> allElements = GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetAllElements();
			if (lastRewardConfigId <= 0)
			{
				this.curRewardConfigId = allElements[0].id;
				this.curRewardType = ((lastRewardType == 1) ? 1 : 2);
			}
			else if (lastRewardConfigId == allElements[allElements.Count - 1].id)
			{
				this.curRewardConfigId = allElements[0].id;
				this.curRewardType = 2;
			}
			else
			{
				this.curRewardConfigId = lastRewardConfigId + 1;
				this.curRewardType = lastRewardType;
			}
			ChestList_ChestList elementById = GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetElementById(this.curRewardConfigId);
			int num = this.curRewardConfigId - 1;
			ChestList_ChestList elementById2 = GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetElementById(num);
			GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.ChestScore = chestInfo.Score;
			if (elementById2 == null)
			{
				this.maxScore = (long)((this.curRewardType == 1) ? elementById.point : elementById.pointCircle);
			}
			else
			{
				this.maxScore = (long)((this.curRewardType == 1) ? (elementById.point - elementById2.point) : (elementById.pointCircle - elementById2.pointCircle));
			}
			this.UpdateCurrentScore();
		}

		public void UpdateCurrentScore()
		{
			GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetAllElements();
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid(23UL);
			int num = this.curRewardConfigId - 1;
			ChestList_ChestList elementById = GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetElementById(num);
			if (elementById == null)
			{
				this.curScore = itemDataCountByid;
				return;
			}
			this.curScore = itemDataCountByid - (long)((this.curRewardType == 1) ? elementById.point : elementById.pointCircle);
		}

		public int curRewardType;

		public int curRewardConfigId;

		public long curScore;

		public long maxScore;
	}
}
