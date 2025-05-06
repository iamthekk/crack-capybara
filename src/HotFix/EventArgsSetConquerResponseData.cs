using System;
using Framework.EventSystem;
using Proto.Conquer;

namespace HotFix
{
	public class EventArgsSetConquerResponseData : BaseEventArgs
	{
		public void SetData(ConquerListResponse response)
		{
			this.m_response = response;
		}

		public override void Clear()
		{
		}

		public ConquerListResponse m_response;
	}
}
