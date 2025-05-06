using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgRemoveSkills : BaseEventArgs
	{
		public List<int> skills { get; private set; }

		public void SetData(List<int> ids)
		{
			this.skills = ids;
		}

		public override void Clear()
		{
		}
	}
}
