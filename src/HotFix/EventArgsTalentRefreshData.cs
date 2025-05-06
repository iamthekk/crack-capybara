using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsTalentRefreshData : BaseEventArgs
	{
		public void SetData(TalentInfo info)
		{
			this.talentInfo = info;
		}

		public override void Clear()
		{
		}

		public TalentInfo talentInfo;
	}
}
