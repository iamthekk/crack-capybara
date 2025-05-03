using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIChapterActivityScoreInfoCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.scoreItem.gameObject.SetActiveSafe(false);
			for (int i = 0; i < 2; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.scoreItem.gameObject);
				gameObject.SetParentNormal(this.slotScoreLayout, false);
				gameObject.SetActiveSafe(true);
				UIChapterActivityRankScoreItem component = gameObject.GetComponent<UIChapterActivityRankScoreItem>();
				component.Init();
				this.luckySlotItems.Add(component);
			}
			for (int j = 0; j < 3; j++)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.scoreItem.gameObject);
				gameObject2.SetParentNormal(this.flipCardScoreLayout, false);
				gameObject2.SetActiveSafe(true);
				UIChapterActivityRankScoreItem component2 = gameObject2.GetComponent<UIChapterActivityRankScoreItem>();
				component2.Init();
				this.flipCardItems.Add(component2);
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.luckySlotItems.Count; i++)
			{
				this.luckySlotItems[i].DeInit();
			}
			for (int j = 0; j < this.flipCardItems.Count; j++)
			{
				this.flipCardItems[j].DeInit();
			}
			this.luckySlotItems.Clear();
			this.flipCardItems.Clear();
		}

		public void SetData(ChapterActivity_RankActivity rankActivity)
		{
			if (rankActivity == null)
			{
				return;
			}
			List<int> list = null;
			List<int> list2 = null;
			if (rankActivity.parameter.Length >= 2)
			{
				list = rankActivity.parameter[0].GetListInt(',');
				list2 = rankActivity.parameter[1].GetListInt(',');
			}
			if (list == null || list2 == null)
			{
				return;
			}
			if (list.Count != this.luckySlotItems.Count || list2.Count != this.flipCardItems.Count)
			{
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(rankActivity.atlasID);
			for (int i = 0; i < this.luckySlotItems.Count; i++)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_rank_score", new object[] { i + 2 });
				this.luckySlotItems[i].SetData(atlasPath, rankActivity.itemIcon, infoByID, list[i]);
			}
			for (int j = 0; j < this.flipCardItems.Count; j++)
			{
				string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(string.Format("ui_chapter_rank_reward_{0}", j + 1));
				this.flipCardItems[j].SetData(atlasPath, rankActivity.itemIcon, infoByID2, list2[j]);
			}
		}

		public UIChapterActivityRankScoreItem scoreItem;

		public GameObject slotScoreLayout;

		public GameObject flipCardScoreLayout;

		private List<UIChapterActivityRankScoreItem> luckySlotItems = new List<UIChapterActivityRankScoreItem>();

		private List<UIChapterActivityRankScoreItem> flipCardItems = new List<UIChapterActivityRankScoreItem>();
	}
}
