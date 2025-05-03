using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsCollectionLevelUp : BaseEventArgs
	{
		public void SetData(CollectionDto param)
		{
			this.collectionDto = param;
		}

		public override void Clear()
		{
		}

		public CollectionDto collectionDto;
	}
}
