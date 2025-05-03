using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsObject : BaseEventArgs
	{
		public object Value
		{
			get
			{
				return this.obj;
			}
		}

		public void SetData(object o)
		{
			this.obj = o;
		}

		public override void Clear()
		{
			this.obj = null;
		}

		public object obj;
	}
}
