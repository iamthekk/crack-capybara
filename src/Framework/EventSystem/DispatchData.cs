using System;

namespace Framework.EventSystem
{
	public struct DispatchData
	{
		public object m_sender;

		public int m_type;

		public BaseEventArgs m_eventArgs;
	}
}
