using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsEventNpcAnimation : BaseEventArgs
	{
		public string aniName { get; private set; }

		public string idleName { get; private set; }

		public int npcId { get; private set; }

		public void SetData(string name, string idle, int npcId)
		{
			this.aniName = name;
			this.idleName = idle;
			this.npcId = npcId;
		}

		public override void Clear()
		{
		}
	}
}
