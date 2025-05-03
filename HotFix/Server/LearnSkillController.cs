using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public class LearnSkillController
	{
		public void OnInit(LocalModelManager tableManager)
		{
			this.m_tableManager = tableManager;
			IList<BattleMain_skill> allElements = this.m_tableManager.GetBattleMain_skillModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				BattleMain_skill battleMain_skill = allElements[i];
				LearnSkillData learnSkillData = new LearnSkillData();
				learnSkillData.SetTableData(battleMain_skill);
				this.InitAddSkill(learnSkillData);
			}
		}

		private void InitAddSkill(LearnSkillData skillData)
		{
			Dictionary<int, LearnSkillData> dictionary;
			if (!this.m_skillTypeDic.TryGetValue(skillData.m_skillType, out dictionary))
			{
				dictionary = new Dictionary<int, LearnSkillData>();
				this.m_skillTypeDic.Add(skillData.m_skillType, dictionary);
			}
			dictionary.Add(skillData.m_id, skillData);
		}

		private Dictionary<int, LearnSkillData> GetSkillListByType(LearnSkillType skillType)
		{
			Dictionary<int, LearnSkillData> dictionary;
			if (!this.m_skillTypeDic.TryGetValue(skillType, out dictionary))
			{
				HLog.LogError(string.Format("LearnSkillController.GetSkillListByType() error, skillType:{0}", skillType));
				return null;
			}
			return dictionary;
		}

		public void UpdateByServer()
		{
			List<int> list = new List<int>();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int i = 0; i < list.Count; i++)
			{
				int num = list[i];
				if (dictionary.ContainsKey(num))
				{
					Dictionary<int, int> dictionary2 = dictionary;
					int num2 = num;
					int num3 = dictionary2[num2];
					dictionary2[num2] = num3 + 1;
				}
				else
				{
					dictionary.Add(num, 1);
				}
			}
			foreach (KeyValuePair<int, int> keyValuePair in dictionary)
			{
				int value = keyValuePair.Value;
				BattleMain_skill elementById = this.m_tableManager.GetBattleMain_skillModelInstance().GetElementById(keyValuePair.Key);
				LearnSkillData learnSkillData;
				if (this.GetSkillListByType((LearnSkillType)elementById.skillType).TryGetValue(keyValuePair.Key, out learnSkillData))
				{
					learnSkillData.SetLevel(value);
				}
			}
		}

		public void LearnSkill(LearnSkillData skillData)
		{
			if (skillData == null)
			{
				HLog.LogError("BattleMainSkillController.LearnSkill() error, skillData is null");
				return;
			}
			skillData.LearnSkill();
		}

		public List<LearnSkillData> GetRandomSkills(LearnSkillType skillType, int count)
		{
			Dictionary<int, LearnSkillData> skillListByType = this.GetSkillListByType(skillType);
			List<LearnSkillData> list = new List<LearnSkillData>();
			foreach (KeyValuePair<int, LearnSkillData> keyValuePair in skillListByType)
			{
				if (!keyValuePair.Value.IsAlreadyBreak())
				{
					list.Add(keyValuePair.Value);
				}
			}
			if (list.Count < count)
			{
				HLog.LogError(string.Format("!!!!BattleMainSkillController!!!!.GetRandomSkills() error, skills.Count:{0} < count:{1}!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", list.Count, count));
			}
			else if (list.Count != count)
			{
				Random random = new Random();
				while (list.Count > count)
				{
					int num = random.Next(0, list.Count);
					list.RemoveAt(num);
				}
			}
			return list;
		}

		public List<int> GetLearnedSkills()
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<LearnSkillType, Dictionary<int, LearnSkillData>> keyValuePair in this.m_skillTypeDic)
			{
				foreach (KeyValuePair<int, LearnSkillData> keyValuePair2 in keyValuePair.Value)
				{
					List<int> learnedSkills = keyValuePair2.Value.GetLearnedSkills();
					list.AddRange(learnedSkills);
				}
			}
			return list;
		}

		private Dictionary<LearnSkillType, Dictionary<int, LearnSkillData>> m_skillTypeDic = new Dictionary<LearnSkillType, Dictionary<int, LearnSkillData>>();

		private LocalModelManager m_tableManager;
	}
}
