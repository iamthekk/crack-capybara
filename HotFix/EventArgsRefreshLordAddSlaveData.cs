using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsRefreshLordAddSlaveData : BaseEventArgs
	{
		public void SetData(LordDto lordDto, int slaveCount)
		{
			this.m_lordDto = lordDto;
			this.m_slaveCount = slaveCount;
		}

		public override void Clear()
		{
			this.m_lordDto = null;
			this.m_slaveCount = 0;
		}

		public LordDto m_lordDto;

		public int m_slaveCount;
	}
}
