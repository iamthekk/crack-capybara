using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventArgsCollectionUpdate : BaseEventArgs
	{
		public void SetData(RepeatedField<CollectionDto> param)
		{
			this.dtos = param;
		}

		public override void Clear()
		{
		}

		public RepeatedField<CollectionDto> dtos;
	}
}
