using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventEquipList : BaseEventArgs
	{
		public void SetData(RepeatedField<EquipmentDto> list)
		{
			this.list = list;
		}

		public override void Clear()
		{
			this.list = null;
		}

		public RepeatedField<EquipmentDto> list;
	}
}
