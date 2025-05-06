using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgClickSkill : BaseEventArgs
	{
		public UIGameEventSkillItem skillItem { get; private set; }

		public void SetData(UIGameEventSkillItem item)
		{
			this.skillItem = item;
		}

		public override void Clear()
		{
		}
	}
}
