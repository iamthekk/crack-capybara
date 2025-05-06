using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class TowerRankItemNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			this.avatarRelation.Init();
			this.avatarRelation.OnClick = new Action<UIAvatarRelation>(this.OnClickAvatarRelation);
			this.SetRankInfo(string.Empty, -1);
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
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

		private void OnClickAvatarRelation(UIAvatarRelation obj)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.rankDto.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		public void RefreshData(TowerRankDto rankDtoVal, string rankInfo, int rank)
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
				this.GuildText.gameObject.SetActive(false);
			}
			else
			{
				this.GuildText.gameObject.SetActive(true);
				DxxTools.UI.SetTextWithEllipsis(this.GuildText, rankDtoVal.GuildName);
				this.GuildText.gameObject.SetActiveSafe(!string.IsNullOrEmpty(rankDtoVal.GuildName));
			}
			this.powerText.text = DxxTools.FormatNumber(rankDtoVal.Power);
			int num = this.towerDataModule.CalculateShouldChallengeLevelID(rankDtoVal.Tower);
			TowerChallenge_Tower towerConfigByLevelId = this.towerDataModule.GetTowerConfigByLevelId(num);
			int towerConfigNum = this.towerDataModule.GetTowerConfigNum(towerConfigByLevelId);
			int levelNumByLevelId = this.towerDataModule.GetLevelNumByLevelId(num);
			this.levelText.text = string.Format("{0}-{1}", towerConfigNum, levelNumByLevelId);
			this.SetRankInfo(rankInfo, rank);
		}

		private void SetRankInfo(string rankInfo, int rank)
		{
			this.rankText.text = rankInfo;
		}

		public UIAvatarRelation avatarRelation;

		public CustomText nameText;

		public CustomText GuildText;

		public UITitleCtrl TitleCtrl;

		public CustomText powerText;

		public CustomText levelText;

		public CustomText rankText;

		private TowerRankDto rankDto;

		private TowerDataModule towerDataModule;
	}
}
