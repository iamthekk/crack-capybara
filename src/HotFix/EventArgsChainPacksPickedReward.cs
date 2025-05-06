using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsChainPacksPickedReward : BaseEventArgs
	{
		public int CfgID { get; private set; }

		public void SetData(int cfgId)
		{
			this.CfgID = cfgId;
		}

		public override void Clear()
		{
			this.CfgID = 0;
		}
	}
}
