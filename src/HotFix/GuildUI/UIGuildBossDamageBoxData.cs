using System;
using LocalModels.Bean;

namespace HotFix.GuildUI
{
	public class UIGuildBossDamageBoxData
	{
		public int ID
		{
			get
			{
				return this.Table.ID;
			}
		}

		public long Damage
		{
			get
			{
				return this.Table.Damage;
			}
		}

		public int BoxImageAtlas
		{
			get
			{
				return this.Table.ShowBoxAtlas;
			}
		}

		public string BoxImage
		{
			get
			{
				return this.Table.ShowBoxIconIdle;
			}
		}

		public string BoxImageOpen
		{
			get
			{
				return this.Table.ShowBoxIconOpen;
			}
		}

		public static UIGuildBossDamageBoxData Get(GuildBOSS_guildBossBox table)
		{
			return new UIGuildBossDamageBoxData
			{
				Table = table
			};
		}

		public GuildBOSS_guildBossBox Table;

		public bool IsOpen;
	}
}
