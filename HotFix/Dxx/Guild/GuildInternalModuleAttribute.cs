using System;

namespace Dxx.Guild
{
	internal class GuildInternalModuleAttribute : Attribute
	{
		public GuildInternalModuleAttribute(int index)
		{
			this.Index = index;
		}

		public int Index;
	}
}
