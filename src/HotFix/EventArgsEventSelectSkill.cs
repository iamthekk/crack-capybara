using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsEventSelectSkill : BaseEventArgs
	{
		public GameEventSkillBuildData skillBuild { get; private set; }

		public void SetData(List<int> selectList, List<int> removeList, GameEventSkillBuildData data)
		{
			this.skills = selectList;
			this.removeSkills = removeList;
			this.skillBuild = data;
		}

		public override void Clear()
		{
		}

		public List<int> skills = new List<int>();

		public List<int> removeSkills = new List<int>();
	}
}
