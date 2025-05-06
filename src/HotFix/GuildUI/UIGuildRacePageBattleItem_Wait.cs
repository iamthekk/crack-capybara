using System;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem_Wait : UIGuildRacePageBattleItem
	{
		protected override void GuildUI_OnInit()
		{
			this.Player1.Init();
			this.Player2.Init();
			this.VS.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildRacePageBattleItem_Wait_Player player = this.Player1;
			if (player != null)
			{
				player.DeInit();
			}
			UIGuildRacePageBattleItem_Wait_Player player2 = this.Player2;
			if (player2 != null)
			{
				player2.DeInit();
			}
			UIGuildRacePageBattleItem_Wait_VS vs = this.VS;
			if (vs == null)
			{
				return;
			}
			vs.DeInit();
		}

		public override void RefreshUI()
		{
			this.Player1.SetData(this.Data, this.Data.User1);
			this.Player1.RefreshUI();
			this.Player2.SetData(this.Data, this.Data.User2);
			this.Player2.RefreshUI();
			this.VS.SetData(this.Data);
			this.VS.RefreshUI();
		}

		public override void SetGuildName(string guildName1, string guildName2)
		{
			this.Player1.SetGuildName(guildName1);
			this.Player2.SetGuildName(guildName2);
		}

		public UIGuildRacePageBattleItem_Wait_Player Player1;

		public UIGuildRacePageBattleItem_Wait_Player Player2;

		public UIGuildRacePageBattleItem_Wait_VS VS;
	}
}
