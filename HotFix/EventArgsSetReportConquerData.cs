using System;
using Framework.EventSystem;
using Proto.Social;

namespace HotFix
{
	public class EventArgsSetReportConquerData : BaseEventArgs
	{
		public void SetData(SocialityInteractiveData socialityInteractiveData, InteractDetailResponse interactDetailResponse)
		{
			this.m_socialityInteractiveData = socialityInteractiveData;
			this.m_interactDetailResponse = interactDetailResponse;
		}

		public override void Clear()
		{
		}

		public SocialityInteractiveData m_socialityInteractiveData;

		public InteractDetailResponse m_interactDetailResponse;
	}
}
