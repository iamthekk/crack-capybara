using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildCheckMemberItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.buttonBorder.onClick.AddListener(new UnityAction(this.OnClickUser));
			this.headIcon.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonBorder != null)
			{
				this.buttonBorder.onClick.RemoveListener(new UnityAction(this.OnClickUser));
			}
			this.headIcon.DeInit();
		}

		public void Refresh(GuildUserShareData data)
		{
			this.userShareData = data;
			if (data == null)
			{
				return;
			}
			this.textName.text = data.GetNick();
			string infoByID = GuildProxy.Language.GetInfoByID1("400059", data.Level);
			this.textLevel.text = infoByID;
			GuildProxy.Language.GetInfoByID1("400058", data.WeeklyActive);
			this.textDevote.text = data.WeeklyActive.ToString();
			this.textPosition.text = (this.textPosition.text = data.GetPositionLanguage());
			this.headIcon.Refresh(data.Avatar, data.AvatarFrame, new Action<object>(this.OnClickUser));
			this.textPower.text = DxxTools.FormatNumber((long)data.Power);
			switch (data.GuildPosition)
			{
			case GuildPositionType.President:
				this.Image_Position.sprite = this.PositionSprite_President;
				return;
			case GuildPositionType.VicePresident:
			case GuildPositionType.Manager:
				this.Image_Position.sprite = this.PositionSprite_VicePresident;
				return;
			case GuildPositionType.Member:
				this.Image_Position.sprite = this.PositionSprite_Member;
				return;
			default:
				return;
			}
		}

		private void RefreshTime()
		{
			if (this.userShareData == null)
			{
				return;
			}
			GuildProxy.Net.ServerTime();
			ulong lastOnlineTime = this.userShareData.LastOnlineTime;
			GuildProxy.UI.OfflineSec();
		}

		private void OnClickUser()
		{
			if (this.userShareData == null)
			{
				return;
			}
			GuildProxy.UI.OpenUserDetailUI(this.userShareData.UserID);
		}

		private void OnClickUser(object obj)
		{
			this.OnClickUser();
		}

		[SerializeField]
		private CustomText textName;

		[SerializeField]
		private CustomText textLevel;

		[SerializeField]
		private CustomButton buttonBorder;

		[SerializeField]
		private CustomText textDevote;

		[SerializeField]
		private CustomText textPosition;

		[SerializeField]
		private UIGuildHead headIcon;

		[SerializeField]
		private CustomText textPower;

		public Sprite PositionSprite_President;

		public Sprite PositionSprite_VicePresident;

		public Sprite PositionSprite_Member;

		public CustomImage Image_Position;

		private GuildUserShareData userShareData;
	}
}
