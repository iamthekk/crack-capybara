﻿using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem_Wait_Player : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Obj_SuperFlag.SetActive(false);
			this.Head.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildHead head = this.Head;
			if (head == null)
			{
				return;
			}
			head.DeInit();
		}

		public void SetData(GuildRaceUserVSRecord vsdata, GuildRaceMember data)
		{
			this.VSData = vsdata;
			this.Data = data;
		}

		public void RefreshUI()
		{
			if (this.Data == null || this.Data.IsEmptyUser || this.VSData == null)
			{
				this.RefreshUIAsEmpty();
				return;
			}
			this.SetFlag();
			this.Obj_EmptyHead.SetActive(false);
			this.Head.SetActive(true);
			this.Head.Refresh(this.Data.UserData.Avatar, this.Data.UserData.AvatarFrame);
			this.Head.SetDefaultClick(this.Data.UserData.UserID);
			this.Text_Name.text = this.Data.UserData.GetNick();
		}

		public void RefreshUIAsEmpty()
		{
			this.SetFlag();
			this.Obj_EmptyHead.SetActive(true);
			this.Head.SetActive(false);
			this.Text_Name.text = GuildProxy.Language.GetInfoByID_LogError(400448);
		}

		private void SetFlag()
		{
			if (this.VSData == null)
			{
				this.Obj_SuperFlag.SetActive(false);
				return;
			}
			switch (this.VSData.Position)
			{
			case GuildRaceBattlePosition.Warrior:
				this.Obj_SuperFlag.SetActive(false);
				this.Text_Super.text = GuildProxy.Language.GetInfoByID_LogError(400490);
				return;
			case GuildRaceBattlePosition.Elite:
				this.Obj_SuperFlag.SetActive(true);
				this.Text_Super.text = GuildProxy.Language.GetInfoByID_LogError(400491);
				return;
			case GuildRaceBattlePosition.General:
				this.Obj_SuperFlag.SetActive(true);
				this.Text_Super.text = GuildProxy.Language.GetInfoByID_LogError(400492);
				return;
			}
			this.Obj_SuperFlag.SetActive(false);
		}

		public void SetGuildName(string guildname)
		{
			this.Text_Guild.text = guildname;
		}

		public GameObject Obj_SuperFlag;

		public CustomText Text_Super;

		public UIGuildHead Head;

		public GameObject Obj_EmptyHead;

		public CustomText Text_Name;

		public CustomText Text_Guild;

		public GuildRaceMember Data;

		public GuildRaceUserVSRecord VSData;
	}
}
