using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Common;
using UnityEngine.Events;

namespace HotFix
{
	public class SocialityRankNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_avatarRelation.Init();
			this.m_avatarRelation.OnClick = new Action<UIAvatarRelation>(this.OnClickAvatarRelation);
			if (this.m_visitBt != null)
			{
				this.m_visitBt.onClick.AddListener(new UnityAction(this.OnClickVisitBt));
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_avatarRelation != null)
			{
				this.m_avatarRelation.OnClick = null;
			}
			if (this.m_visitBt != null)
			{
				this.m_visitBt.onClick.RemoveAllListeners();
			}
			if (this.m_avatarRelation != null)
			{
				this.m_avatarRelation.DeInit();
			}
			this.m_rankDto = null;
		}

		private void OnClickAvatarRelation(UIAvatarRelation obj)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_rankDto.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		private void OnClickVisitBt()
		{
			OtherMainCityViewModule.OpenData openData = new OtherMainCityViewModule.OpenData();
			openData.m_userID = this.m_rankDto.UserId;
			if (GameApp.View.IsOpened(ViewName.OtherMainCityViewModule))
			{
				GameApp.View.CloseView(ViewName.OtherMainCityViewModule, null);
			}
			GameApp.View.OpenView(ViewName.OtherMainCityViewModule, openData, 1, null, null);
		}

		public void RefreshData(PowerRankDto rankDto, int index)
		{
			this.m_rankDto = rankDto;
			if (rankDto == null)
			{
				return;
			}
			int num = -1;
			int num2 = -1;
			if (rankDto.Extra != null && rankDto.Extra.LordUid > 0L)
			{
				num = rankDto.Extra.LordAvatar;
				num2 = rankDto.Extra.LordAvatarFrame;
			}
			if (this.m_avatarRelation != null)
			{
				this.m_avatarRelation.RefreshData(rankDto.Avatar, rankDto.AvatarFrame, rankDto.Level, num, num2);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = (string.IsNullOrEmpty(this.m_rankDto.NickName) ? DxxTools.GetDefaultNick(this.m_rankDto.UserId) : this.m_rankDto.NickName);
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9205", new object[] { DxxTools.FormatNumber(this.m_rankDto.Power) });
			}
		}

		public UIAvatarRelation m_avatarRelation;

		public CustomText m_nameTxt;

		public CustomText m_powerTxt;

		public CustomButton m_visitBt;

		public PowerRankDto m_rankDto;
	}
}
