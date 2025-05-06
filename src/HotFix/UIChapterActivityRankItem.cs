using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Chapter;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIChapterActivityRankItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.avatarCtrl.Init();
			this.itemDefault.gameObject.SetActiveSafe(false);
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemDefault.gameObject);
				gameObject.SetParentNormal(this.layoutGroup.gameObject, false);
				gameObject.SetActiveSafe(true);
				UIItem component = gameObject.GetComponent<UIItem>();
				component.Init();
				this.itemList.Add(component);
			}
		}

		protected override void OnDeInit()
		{
			this.avatarCtrl.DeInit();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
		}

		public void SetData(ChapterActRankDto rankDto, string scoreAtlas, string scoreIcon, int rankId)
		{
			this.SetBG(rankDto.Rank);
			Color white = Color.white;
			switch (rankDto.Rank)
			{
			case 1:
				ColorUtility.TryParseHtmlString("#FFF589", ref white);
				break;
			case 2:
				ColorUtility.TryParseHtmlString("#F4F4FF", ref white);
				break;
			case 3:
				ColorUtility.TryParseHtmlString("#FFE3D7", ref white);
				break;
			}
			this.textRank.text = rankDto.Rank.ToString();
			this.textRank.color = white;
			this.textName.text = rankDto.NickName;
			this.avatarCtrl.RefreshData(rankDto.Avatar, rankDto.AvatarFrame);
			this.imageScoreIcon.SetImage(scoreAtlas, scoreIcon);
			this.textScore.text = "x" + DxxTools.FormatNumber((long)rankDto.Score);
			List<ItemData> rankReward = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).GetRankReward(rankDto.Rank, rankId);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				if (i < rankReward.Count)
				{
					this.itemList[i].gameObject.SetActiveSafe(true);
					PropData propData = new PropData();
					propData.id = (uint)rankReward[i].ID;
					propData.count = (ulong)rankReward[i].TotalCount;
					this.itemList[i].SetData(propData);
					this.itemList[i].OnRefresh();
					this.itemList[i].SetTextCountScale(Vector3.one * 1.5f);
				}
				else
				{
					this.itemList[i].gameObject.SetActiveSafe(false);
				}
			}
		}

		private void SetBG(int rank)
		{
			string atlasPath = GameApp.Table.GetAtlasPath(122);
			this.imageBg.SetImage(atlasPath, "activity_rank_other");
			this.imageScoreBg.SetImage(atlasPath, "activity_rank_other_reward");
		}

		public void SetSelfBG()
		{
			string atlasPath = GameApp.Table.GetAtlasPath(122);
			this.imageBg.SetImage(atlasPath, "activity_rank_first");
			this.imageScoreBg.SetImage(atlasPath, "activity_rank_first_reward");
		}

		public CustomImage imageBg;

		public CustomText textRank;

		public CustomText textName;

		public UIAvatarCtrl avatarCtrl;

		public CustomImage imageScoreIcon;

		public CustomImage imageScoreBg;

		public CustomText textScore;

		public HorizontalLayoutGroup layoutGroup;

		public UIItem itemDefault;

		private const string FIRST_RANK_COLOR = "#FFF589";

		private const string SECOND_RANK_COLOR = "#F4F4FF";

		private const string THIRD_RANK_COLOR = "#FFE3D7";

		private List<UIItem> itemList = new List<UIItem>();
	}
}
