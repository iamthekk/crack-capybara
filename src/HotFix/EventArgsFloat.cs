using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsFloat : BaseEventArgs
	{
		public float Value
		{
			get
			{
				return this.m_count;
			}
		}

		public void SetData(float count)
		{
			this.m_count = count;
		}

		public override void Clear()
		{
			this.m_count = 0f;
		}

		protected float m_count;
	}
}
