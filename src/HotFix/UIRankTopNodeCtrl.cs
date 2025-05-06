using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Proto.LeaderBoard;
using UnityEngine;

namespace HotFix
{
	public class UIRankTopNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			foreach (UIBaseRankItemCtrl uibaseRankItemCtrl in this.style1TopItems)
			{
				uibaseRankItemCtrl.Init();
			}
			foreach (UIRankTopItem uirankTopItem in this.style2TopItems)
			{
				uirankTopItem.Init();
				uirankTopItem.OnShow();
			}
		}

		protected override void OnDeInit()
		{
			foreach (UIBaseRankItemCtrl uibaseRankItemCtrl in this.style1TopItems)
			{
				uibaseRankItemCtrl.DeInit();
			}
			foreach (UIRankTopItem uirankTopItem in this.style2TopItems)
			{
				uirankTopItem.OnHide();
				uirankTopItem.DeInit();
			}
		}

		public void Refresh(RankType rankType, List<BaseRankData> rankDataList)
		{
			for (int i = 0; i < this.style1TopItems.Count; i++)
			{
				UIBaseRankItemCtrl uibaseRankItemCtrl = this.style1TopItems[i];
				bool flag = i >= rankDataList.Count;
				uibaseRankItemCtrl.SetActive(!flag);
				if (!flag)
				{
					int num = i;
					uibaseRankItemCtrl.SetFresh(rankType, rankDataList[i], num + 1);
				}
			}
			List<RankUserDto> lastTop = GameApp.Data.GetDataModule(DataName.RankDataModule).GetLastTop3(rankType);
			for (int j = 0; j < this.style2TopItems.Count; j++)
			{
				UIRankTopItem uirankTopItem = this.style2TopItems[j];
				RankUserDto rankUserDto = null;
				if (j < lastTop.Count && lastTop[j] != null)
				{
					rankUserDto = lastTop[j];
				}
				uirankTopItem.SetData(rankType, rankUserDto, string.Format("RankTopPlayer_{0}", j + 1));
			}
		}

		[SerializeField]
		private List<UIBaseRankItemCtrl> style1TopItems;

		[SerializeField]
		private List<UIRankTopItem> style2TopItems;
	}
}
