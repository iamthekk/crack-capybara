using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgGuideTrigger : BaseEventArgs
	{
		public GuideTriggerKind triggerKind { get; private set; }

		public string triggerParam { get; private set; }

		public void SetData(GuideTriggerKind kind, string param)
		{
			this.triggerKind = kind;
			this.triggerParam = param;
		}

		public override void Clear()
		{
		}
	}
}
