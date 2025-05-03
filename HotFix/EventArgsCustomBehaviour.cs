using System;
using Framework.EventSystem;
using Framework.Logic.Component;

namespace HotFix
{
	public class EventArgsCustomBehaviour : BaseEventArgs
	{
		public void SetData(CustomBehaviour target)
		{
			this.m_target = target;
		}

		public override void Clear()
		{
		}

		public CustomBehaviour m_target;
	}
}
