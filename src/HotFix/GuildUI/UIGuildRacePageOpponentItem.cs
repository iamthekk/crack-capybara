using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageOpponentItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Avatar.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildHead avatar = this.Avatar;
			if (avatar == null)
			{
				return;
			}
			avatar.DeInit();
		}

		public void SetData(GuildRaceMember data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			this.Text_Index.text = "??";
			this.Text_Name.text = this.mData.UserData.GetNick();
			this.Text_Power.text = GuildProxy.Language.GetInfoByID1("400416", GuildProxy.Language.FormatNumber((long)this.mData.Power));
			this.Text_AP.text = GuildProxy.Language.GetInfoByID1("400417", this.mData.ActivityPoint);
			this.Text_Score.text = GuildProxy.Language.GetInfoByID("400415") + "\r\n??";
			this.Avatar.Refresh(this.mData.UserData.Avatar, this.mData.UserData.AvatarFrame);
			this.Avatar.SetDefaultClick(this.mData.UserData.UserID);
			this.Obj_Flag.SetActive(false);
		}

		public CustomText Text_Index;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public CustomText Text_AP;

		public CustomText Text_Score;

		public GameObject Obj_Flag;

		public UIGuildHead Avatar;

		private GuildRaceMember mData;
	}
}
