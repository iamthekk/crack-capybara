using System;

namespace Dxx.Guild
{
	[GuildInternalModule(2)]
	public class GuildNetwork : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 2;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			return config != null;
		}

		public void UnInit()
		{
		}
	}
}
