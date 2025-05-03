using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventArgsUpdateRelicData : BaseEventArgs
	{
		public void SetData(RepeatedField<RelicDto> datas)
		{
			this.m_datas = datas;
		}

		public override void Clear()
		{
			this.m_datas = null;
		}

		public RepeatedField<RelicDto> m_datas;
	}
}
