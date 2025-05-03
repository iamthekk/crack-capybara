using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsAddCombatTipNode : BaseEventArgs
	{
		public void SetData(long from, long to)
		{
			this.m_from = from;
			this.m_to = to;
		}

		public override void Clear()
		{
		}

		public long m_from;

		public long m_to;
	}
}
