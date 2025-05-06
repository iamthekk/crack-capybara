using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventArgsUpdateEquipDatas : BaseEventArgs
	{
		public void SetData(RepeatedField<EquipmentDto> datas)
		{
			this.m_datas = datas;
		}

		public override void Clear()
		{
			this.m_datas = null;
		}

		public RepeatedField<EquipmentDto> m_datas;
	}
}
