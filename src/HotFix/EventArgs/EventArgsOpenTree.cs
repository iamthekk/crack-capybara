using System;
using Framework.EventSystem;

namespace HotFix.EventArgs
{
	public class EventArgsOpenTree : BaseEventArgs
	{
		public EventArgsOpenTree(int careerId, int talentLegacyNodeId = -1)
		{
			this.CareerId = careerId;
			this.TalentLegacyNodeId = talentLegacyNodeId;
		}

		public override void Clear()
		{
			this.CareerId = -1;
			this.TalentLegacyNodeId = -1;
		}

		public int CareerId = -1;

		public int TalentLegacyNodeId = -1;
	}
}
