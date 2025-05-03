using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshMainOpenData : BaseEventArgs
	{
		public void SetData(MainOpenData openData)
		{
			this.m_openData = openData;
		}

		public override void Clear()
		{
			this.m_openData = null;
		}

		public MainOpenData m_openData;
	}
}
