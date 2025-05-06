using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Dxx.Guild
{
	public class GuildPermissionSet : GuildPermissionBase
	{
		private void CheckPositionPermission()
		{
			if (this.mPositionPermission.Count > 0)
			{
				return;
			}
			List<Guild_guildPower> guildPowerTableAll = GuildProxy.Table.GetGuildPowerTableAll();
			for (int i = 0; i < guildPowerTableAll.Count; i++)
			{
				Guild_guildPower guild_guildPower = guildPowerTableAll[i];
				if (guild_guildPower != null)
				{
					List<GuildPermissionKind> list = new List<GuildPermissionKind>();
					foreach (GuildPermissionKind guildPermissionKind in guild_guildPower.Power)
					{
						list.Add(guildPermissionKind);
						if ((guildPermissionKind == GuildPermissionKind.ChangeManager || guildPermissionKind == GuildPermissionKind.ChangeVicePresident) && !list.Contains(GuildPermissionKind.ChangePosition))
						{
							list.Add(GuildPermissionKind.ChangePosition);
						}
					}
					this.mPositionPermission[guild_guildPower.ID] = list;
				}
			}
		}

		public override bool HasPermission(GuildPermissionKind permission, GuildUserShareData myuser, GuildUserShareData otheruser)
		{
			if (myuser == null)
			{
				return false;
			}
			GuildPositionType guildPosition = myuser.GuildPosition;
			this.CheckPositionPermission();
			List<GuildPermissionKind> list;
			return this.CheckPosition(permission, myuser, otheruser) && this.mPositionPermission.TryGetValue((int)guildPosition, out list) && list.Contains(permission);
		}

		public bool CheckPosition(GuildPermissionKind permission, GuildUserShareData myuser, GuildUserShareData otheruser)
		{
			if (myuser == null)
			{
				return false;
			}
			GuildPositionType guildPosition = myuser.GuildPosition;
			GuildPositionType guildPositionType = GuildPositionType.Member;
			if (otheruser != null)
			{
				guildPositionType = otheruser.GuildPosition;
			}
			return (permission - GuildPermissionKind.ChangeManager > 1 && permission != GuildPermissionKind.KickMember && permission != GuildPermissionKind.ChangePosition) || this.IsPositionBiggerThan(guildPosition, guildPositionType);
		}

		public bool IsPositionBiggerThan(GuildPositionType pos1, GuildPositionType pos2)
		{
			return pos1 < pos2;
		}

		private Dictionary<int, List<GuildPermissionKind>> mPositionPermission = new Dictionary<int, List<GuildPermissionKind>>();
	}
}
