using System;

namespace Dxx.Guild
{
	[GuildInternalModule(3)]
	public class GuildConfigModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 3;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			return true;
		}

		public void UnInit()
		{
		}
	}
}
