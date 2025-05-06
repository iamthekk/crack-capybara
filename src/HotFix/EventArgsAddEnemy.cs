using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsAddEnemy : BaseEventArgs
	{
		public void SetData(int id, int atk, int hp, bool isDropBox)
		{
			this.monsterCfgId = id;
			this.atkUpgrade = atk;
			this.hpUpgrade = hp;
			this.isDropBox = isDropBox;
		}

		public override void Clear()
		{
		}

		public int monsterCfgId;

		public int atkUpgrade;

		public int hpUpgrade;

		public bool isDropBox;
	}
}
