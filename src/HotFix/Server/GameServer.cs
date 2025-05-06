using System;
using LocalModels;

namespace Server
{
	public static class GameServer
	{
		public static LocalModelManager LocalModelManager { get; private set; }

		public static void Init(LocalModelManager localModelManager)
		{
			GameServer.LocalModelManager = localModelManager;
		}
	}
}
