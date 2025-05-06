using System;
using Dxx.Guild;
using LocalModels.Bean;

namespace HotFix.GuildUI
{
	public class GuildRacePositionSetItemData
	{
		public bool IsInBattle
		{
			get
			{
				return this.User.Position > GuildRaceBattlePosition.None;
			}
		}

		public bool IsSpecailPos
		{
			get
			{
				return this.User.Position > GuildRaceBattlePosition.None;
			}
		}

		public bool IsAPEngough
		{
			get
			{
				return this.User != null && this.NeedAP <= this.User.ActivityPoint;
			}
		}

		public void SetData(GuildRaceMember member)
		{
			this.User = member;
			this.PositionTab = GuildProxy.Table.GetRaceBaseTable(this.User.Position);
		}

		public string GetSpecailPosName()
		{
			return GuildProxy.Language.GetInfoByID_LogError(400490 + this.User.Position - GuildRaceBattlePosition.Warrior);
		}

		public GuildRaceMember User;

		public GuildRace_baseRace PositionTab;

		public int NeedAP;
	}
}
