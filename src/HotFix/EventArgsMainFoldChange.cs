using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsMainFoldChange : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public void SetData(int kind, int tostate)
		{
			this.Kind = kind;
			this.ToFoldState = tostate;
		}

		public int Kind;

		public int ToFoldState;
	}
}
