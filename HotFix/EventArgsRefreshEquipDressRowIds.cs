using System;
using System.Collections.Generic;
using System.Linq;
using Framework.EventSystem;
using Google.Protobuf.Collections;

namespace HotFix
{
	public class EventArgsRefreshEquipDressRowIds : BaseEventArgs
	{
		public void SetData(RepeatedField<ulong> datas)
		{
			this.m_datas = datas.ToList<ulong>();
		}

		public override void Clear()
		{
			this.m_datas.Clear();
		}

		public List<ulong> m_datas;
	}
}
