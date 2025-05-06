using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgHangGuideUI : BaseEventArgs
	{
		public int guideGroup { get; private set; }

		public int guideId { get; private set; }

		public void SetData(int group, int id)
		{
			this.guideGroup = group;
			this.guideId = id;
		}

		public override void Clear()
		{
		}
	}
}
