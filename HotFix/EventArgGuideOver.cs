using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgGuideOver : BaseEventArgs
	{
		public int guideGroup { get; private set; }

		public int guideId { get; private set; }

		public bool isSkipGroup { get; private set; }

		public void SetData(int group, int id, bool isSkip)
		{
			this.guideGroup = group;
			this.guideId = id;
			this.isSkipGroup = isSkip;
		}

		public override void Clear()
		{
		}
	}
}
