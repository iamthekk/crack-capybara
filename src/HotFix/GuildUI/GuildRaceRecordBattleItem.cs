using System;
using Dxx.Guild;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordBattleItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Player1.Init();
			this.Player2.Init();
			this.VS.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			GuildRaceRecordBattleItemPlayerUI player = this.Player1;
			if (player != null)
			{
				player.DeInit();
			}
			GuildRaceRecordBattleItemPlayerUI player2 = this.Player2;
			if (player2 != null)
			{
				player2.DeInit();
			}
			GuildRaceRecordBattleItemVSUI vs = this.VS;
			if (vs == null)
			{
				return;
			}
			vs.DeInit();
		}

		public void SetData(GuildRaceUserVSRecord data, int index)
		{
			this.VS.SetData(data, index);
			GuildRaceUserVSRecordResult result = data.GetResult(index);
			if (result != null)
			{
				if (data.User2 != null && !data.User2.IsEmptyUser && result.HomeUserID == data.User2.UserData.UserID)
				{
					this.Player1.SetData(data, data.User2, index);
					this.Player2.SetData(data, data.User1, index);
					return;
				}
				this.Player1.SetData(data, data.User1, index);
				this.Player2.SetData(data, data.User2, index);
			}
		}

		public void RefreshUI()
		{
			this.VS.RefreshUI();
			this.Player1.RefreshUI();
			this.Player2.RefreshUI();
		}

		public GuildRaceRecordBattleItemPlayerUI Player1;

		public GuildRaceRecordBattleItemPlayerUI Player2;

		public GuildRaceRecordBattleItemVSUI VS;
	}
}
