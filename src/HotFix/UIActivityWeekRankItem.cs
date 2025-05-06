using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.ActTime;
using UnityEngine;

namespace HotFix
{
	public class UIActivityWeekRankItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.avatarRelation.Init();
			this.avatarRelation.OnClick = new Action<UIAvatarRelation>(this.OnClickAvatarRelation);
			this.scoreValueText.text = "";
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
			}
		}

		protected override void OnDeInit()
		{
			this.avatarRelation.OnClick = null;
			this.avatarRelation.DeInit();
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnClickAvatarRelation(UIAvatarRelation obj)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.rankDto.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		public void RefreshData(UIActivityWeekRankItem.RankType rankType, ActTimeRankDto rankDtoVal, int rank)
		{
			this.rankDto = rankDtoVal;
			if (rankDtoVal == null)
			{
				return;
			}
			if (this.avatarRelation != null)
			{
				this.avatarRelation.RefreshData(rankDtoVal.Avatar, rankDtoVal.AvatarFrame, -1, -1, -1);
			}
			string text = (string.IsNullOrEmpty(rankDtoVal.NickName) ? DxxTools.GetDefaultNick(rankDtoVal.UserId) : rankDtoVal.NickName);
			DxxTools.UI.SetTextWithEllipsis(this.nameText, text);
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.SetAndFresh(rankDtoVal.TitleId);
			}
			if (rankType == UIActivityWeekRankItem.RankType.Score)
			{
				this.scoreTitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID("rank_title_score");
				this.scoreValueText.text = DxxTools.FormatNumber((long)rankDtoVal.Score) ?? "";
			}
			else
			{
				this.scoreTitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID("rank_title_chapter");
				int num = Mathf.Max(1, rankDtoVal.Score / 1000);
				int num2 = rankDtoVal.Score % 1000;
				if (GameApp.Table.GetManager().GetChapter_chapter(num).totalStage == num2 && GameApp.Table.GetManager().GetChapter_chapter(num + 1) != null)
				{
					num++;
					num2 = 0;
				}
				this.scoreValueText.text = string.Format("{0} - {1}", num, num2);
			}
			this.SetRankInfo(rank);
		}

		private void SetRankInfo(int rank)
		{
			if (this.rankImage != null)
			{
				if (rank < 1)
				{
					this.rankText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_norank");
				}
				else if (rank > 3)
				{
					this.rankText.text = rank.ToString();
				}
				else if (this.showThreeRank)
				{
					this.rankText.text = rank.ToString();
				}
				else
				{
					this.rankText.text = "";
				}
				if (rank <= 0 || rank >= 4)
				{
					this.rankImage.enabled = false;
					return;
				}
				this.rankImage.enabled = true;
				if (this.changeRankImage)
				{
					this.rankImage.SetImage(GameApp.Table.GetAtlasPath(128), "rank_" + rank.ToString());
					return;
				}
			}
			else
			{
				if (rank < 1)
				{
					this.rankText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_norank");
					return;
				}
				this.rankText.text = rank.ToString();
			}
		}

		public RectTransform taskAniNode;

		public bool showThreeRank;

		public bool changeRankImage = true;

		public UIAvatarRelation avatarRelation;

		public UITitleCtrl TitleCtrl;

		public CustomText nameText;

		public CustomImage rankImage;

		public CustomText rankText;

		public CustomText scoreTitleText;

		public CustomText scoreValueText;

		private ActTimeRankDto rankDto;

		public enum RankType
		{
			Score,
			Chapter
		}
	}
}
