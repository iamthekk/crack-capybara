using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgSelectSkillEnd : BaseEventArgs
	{
		public List<GameEventSkillBuildData> skills { get; private set; }

		public void SetData(List<GameEventSkillBuildData> list)
		{
			this.skills = list;
		}

		public override void Clear()
		{
		}
	}
}
