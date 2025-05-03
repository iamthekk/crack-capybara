using System;

namespace HotFix
{
	public class HoverSkillBuildData
	{
		public GameEventSkillBuildData data { get; private set; }

		public void SetData(GameEventSkillBuildData data)
		{
			this.data = data;
		}
	}
}
