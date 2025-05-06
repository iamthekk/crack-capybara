using System;

namespace Dxx.Guild
{
	[GuildInternalModule(13)]
	public class GuildLogModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 13;
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
