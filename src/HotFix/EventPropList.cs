using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventPropList : BaseEventArgs
	{
		public void SetData(RepeatedField<ItemDto> list)
		{
			this.list = list;
		}

		public override void Clear()
		{
			this.list = null;
		}

		public RepeatedField<ItemDto> list;
	}
}
