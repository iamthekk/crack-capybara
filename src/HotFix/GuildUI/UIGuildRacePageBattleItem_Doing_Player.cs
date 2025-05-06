using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem_Doing_Player : GuildProxy.GuildProxy_BaseBehaviour
	{
		public long UserID
		{
			get
			{
				if (this.Data == null || this.Data.UserData == null)
				{
					return 0L;
				}
				return this.Data.UserData.UserID;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.Head.Init();
			this.UIScore.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildHead head = this.Head;
			if (head != null)
			{
				head.DeInit();
			}
			UIGuildRacePageBattleItem_Doing_Score uiscore = this.UIScore;
			if (uiscore == null)
			{
				return;
			}
			uiscore.DeInit();
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
			this.Obj_EmptyHead.SetActive(false);
			this.Head.SetActive(true);
			this.Head.Refresh(this.Data.UserData.Avatar, this.Data.UserData.AvatarFrame);
			this.Head.SetDefaultClick(this.Data.UserData.UserID);
			this.Text_Name.text = this.Data.UserData.GetNick();
			this.Text_Power.text = GuildProxy.Language.GetInfoByID1("400416", GuildProxy.Language.FormatNumber((long)this.Data.UserData.Power));
		}

		public void SetScore(int index, bool sel)
		{
			UIGuildRacePageBattleItem_Doing_Score uiscore = this.UIScore;
			if (uiscore == null)
			{
				return;
			}
			uiscore.SetScore(index, sel);
		}

		public void RevertScore(int startindex)
		{
			UIGuildRacePageBattleItem_Doing_Score uiscore = this.UIScore;
			if (uiscore == null)
			{
				return;
			}
			uiscore.RevertScore(startindex);
		}

		public void RefreshUIAsEmpty()
		{
			this.Obj_EmptyHead.SetActive(true);
			this.Head.SetActive(false);
			this.Text_Name.text = GuildProxy.Language.GetInfoByID_LogError(400448);
			this.Text_Power.text = GuildProxy.Language.GetInfoByID1("400416", "???");
		}

		public UIGuildHead Head;

		public GameObject Obj_EmptyHead;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public UIGuildRacePageBattleItem_Doing_Score UIScore;

		public GuildRaceMember Data;

		public GuildRaceUserVSRecord VSData;
	}
}
