using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsBool : BaseEventArgs
	{
		public bool Value
		{
			get
			{
				return this.m_value;
			}
		}

		public EventArgsBool SetData(bool value)
		{
			this.m_value = value;
			return this;
		}

		public override void Clear()
		{
			this.m_value = false;
		}

		protected bool m_value;
	}
}
