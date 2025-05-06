using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsBattleCrossArenaEnter : BaseEventArgs
	{
		public void SetData(PVPRecordDto record, bool isRecord = false)
		{
			this.m_record = record;
			this.m_isRecord = isRecord;
		}

		public override void Clear()
		{
		}

		public PVPRecordDto m_record;

		public bool m_isRecord;
	}
}
