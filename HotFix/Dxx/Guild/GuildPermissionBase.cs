using System;

namespace Dxx.Guild
{
	public class GuildPermissionBase
	{
		public virtual bool HasPermission(GuildPermissionKind permission, GuildUserShareData myuser, GuildUserShareData otheruser)
		{
			if (myuser == null)
			{
				return false;
			}
			GuildPositionType guildPosition = myuser.GuildPosition;
			return guildPosition == GuildPositionType.President || (guildPosition == GuildPositionType.VicePresident && permission - GuildPermissionKind.ApplyJoin <= 2);
		}
	}
}
