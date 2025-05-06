using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;

namespace HotFix
{
	public class EventArgsRefreshChapterRewardData : BaseEventArgs
	{
		public void SetData(RepeatedField<int> canRewardList)
		{
			this.m_canRewardList = canRewardList;
		}

		public override void Clear()
		{
		}

		public RepeatedField<int> m_canRewardList;
	}
}
