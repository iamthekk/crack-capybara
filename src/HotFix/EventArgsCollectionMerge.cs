using System;
using System.Collections.Generic;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsCollectionMerge : BaseEventArgs
	{
		public void SetData(List<CollectionDto> param)
		{
			this.collectionDtoList = param;
		}

		public override void Clear()
		{
		}

		public List<CollectionDto> collectionDtoList;
	}
}
