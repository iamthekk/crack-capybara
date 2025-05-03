using System;

namespace Dxx.Guild
{
	public interface IGuildModule
	{
		int ModuleName { get; }

		bool Init(GuildInitConfig config);

		void UnInit();
	}
}
