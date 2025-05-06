using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillHelper
	{
		public static List<int> GetSkillAddBuffs(string json)
		{
			List<int> list = new List<int>();
			if (json.Equals(string.Empty))
			{
				return list;
			}
			SBulletData.TriggerBuffData triggerBuffData = JsonManager.ToObject<SBulletData.TriggerBuffData>(json);
			if (triggerBuffData == null)
			{
				HLog.LogError("Json format is error. json:" + json);
			}
			for (int i = 0; i < triggerBuffData.buffIDs.Count; i++)
			{
				int num = triggerBuffData.buffIDs[i];
				list.Add(num);
			}
			return list;
		}
	}
}
