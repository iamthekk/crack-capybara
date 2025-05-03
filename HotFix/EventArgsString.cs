using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsString : BaseEventArgs
	{
		public string Value
		{
			get
			{
				return this.m_info;
			}
		}

		public void SetData(string info)
		{
			this.m_info = info;
		}

		public override void Clear()
		{
			this.m_info = string.Empty;
		}

		private string m_info = string.Empty;
	}
}
