using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsEventPlayMemebrAni : BaseEventArgs
	{
		public string aniName { get; private set; }

		public void SetData(string name)
		{
			this.aniName = name;
		}

		public override void Clear()
		{
		}
	}
}
