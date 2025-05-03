using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class NodeSkillParam : NodeParamBase
	{
		public override NodeKind GetNodeKind()
		{
			return NodeKind.EventSkill;
		}

		public static List<SkillTypeData> GetSkillParamList(List<NodeSkillParam> skills)
		{
			List<SkillTypeData> list = new List<SkillTypeData>();
			for (int i = 0; i < skills.Count; i++)
			{
				int num = skills[i].skillBuildId;
				GameSkillBuild_skillBuild elementById = GameApp.Table.GetManager().GetGameSkillBuild_skillBuildModelInstance().GetElementById(num);
				if (elementById == null)
				{
					HLog.LogError(string.Format("Table [GameSkillBuild] not found id={0}", num));
				}
				else
				{
					GameSkill_skill elementById2 = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(elementById.skillId);
					if (elementById2 == null)
					{
						HLog.LogError(string.Format("Table [GameSkill] not found id={0}", elementById.skillId));
					}
					else
					{
						SkillTypeData skillTypeData = new SkillTypeData();
						skillTypeData.atlas = elementById2.iconAtlasID;
						skillTypeData.icon = elementById2.icon;
						string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.nameID);
						if (skills[i].isLost)
						{
							skillTypeData.m_value = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_165", new object[] { "<color=#FF604D>" + infoByID + "</color>" });
							skillTypeData.m_tgaValue = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_165", new object[] { infoByID ?? "" });
						}
						else
						{
							skillTypeData.m_value = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_153", new object[] { "<color=#D3F24E>" + infoByID + "</color>" });
							skillTypeData.m_tgaValue = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_153", new object[] { infoByID ?? "" });
						}
						list.Add(skillTypeData);
					}
				}
			}
			return list;
		}

		public int skillBuildId;

		public bool isLost;
	}
}
