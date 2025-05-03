using System;

namespace HotFix
{
	public class BattleClientController_DungeonDragonLair : BattleClientController_Dungeon
	{
		protected override string GetPath()
		{
			return "Assets/_Resources/Prefab/Map/BattleRootDragonLair.prefab";
		}
	}
}
