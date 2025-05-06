using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;

namespace HotFix
{
	public class EventArgsRemoveEquipDatas : BaseEventArgs
	{
		public void SetData(RepeatedField<long> datas)
		{
			this.m_datas = datas;
		}

		public override void Clear()
		{
			this.m_datas = null;
		}

		public RepeatedField<long> m_datas;
	}
}
