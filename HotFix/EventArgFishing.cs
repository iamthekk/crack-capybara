using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgFishing : BaseEventArgs
	{
		public int npcId { get; private set; }

		public FishType fishType { get; private set; }

		public int atkUpgrade { get; private set; }

		public int hpUpgrade { get; private set; }

		public void SetData(int id, FishType type, int atkUpgrade, int hpUpgrade)
		{
			this.npcId = id;
			this.fishType = type;
			this.atkUpgrade = atkUpgrade;
			this.hpUpgrade = hpUpgrade;
		}

		public override void Clear()
		{
		}
	}
}
