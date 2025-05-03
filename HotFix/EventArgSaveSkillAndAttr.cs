using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgSaveSkillAndAttr : BaseEventArgs
	{
		public void SetData(List<GameEventSkillBuildData> list, Dictionary<string, int> dic)
		{
			this.skills = list;
			this.attrDic = dic;
		}

		public override void Clear()
		{
		}

		public List<GameEventSkillBuildData> skills;

		public Dictionary<string, int> attrDic;
	}
}
