using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class EventArgsMiningBomb : BaseEventArgs
	{
		public RepeatedField<GridDto> bombGrids { get; private set; }

		public void SetData(RepeatedField<GridDto> grids)
		{
			this.bombGrids = grids;
		}

		public override void Clear()
		{
		}
	}
}
