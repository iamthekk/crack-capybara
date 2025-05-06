using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix.Client
{
	public class CSkillFactory
	{
		public CMemberBase Owner { get; private set; }

		public void OnInit(CMemberBase owner)
		{
			this.Owner = owner;
		}

		public void OnUpdate(float deltaTime)
		{
			this.list.Clear();
			this.list.AddRange(this.m_allSkill.Values);
			foreach (CSkillBase cskillBase in this.list)
			{
				if (cskillBase != null)
				{
					cskillBase.OnUpdate(deltaTime);
				}
			}
		}

		public void OnDeInit()
		{
			List<CSkillBase> list = this.m_allSkill.Values.ToList<CSkillBase>();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveSkill(list[i].skillData.m_id);
			}
			this.Owner = null;
		}

		public List<CSkillBase> GetSkills()
		{
			return this.m_allSkill.Values.ToList<CSkillBase>();
		}

		public CSkillBase GetSkillByType(SkillFreedType freedType)
		{
			return this.m_allSkill.Values.FirstOrDefault((CSkillBase skill) => skill.skillData.m_freedType == freedType);
		}

		public async Task AddSkills(List<int> ids)
		{
			List<Task> list = new List<Task>();
			for (int i = 0; i < ids.Count; i++)
			{
				int num = ids[i];
				if (!this.m_allSkill.ContainsKey(num))
				{
					Task task = this.AddSkill(num);
					list.Add(task);
				}
			}
			await Task.WhenAll(list);
		}

		private async Task AddSkill(int id)
		{
			if (!this.m_allSkill.ContainsKey(id))
			{
				GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(id);
				if (elementById == null)
				{
					HLog.LogError(string.Format("SkillFactory.AddSkill   GameSkill is error.id = {0} memberId={1}", id, this.Owner.m_memberData.m_id));
				}
				else
				{
					GameSkill_skillType elementById2 = GameApp.Table.GetManager().GetGameSkill_skillTypeModelInstance().GetElementById(elementById.typeID);
					if (elementById2 == null)
					{
						HLog.LogError(string.Format("CSkillFactory.AddSkill   skillTypeTable == null   table.typeID = {0}", elementById.typeID));
					}
					else
					{
						object obj = Activator.CreateInstance(Type.GetType(elementById2.cClassName));
						CSkillBase skill = obj as CSkillBase;
						if (skill == null)
						{
							HLog.LogError("CkillFactory.AddSkill   CSkillBase == null   skillTypeTable.cClassName = " + elementById2.cClassName);
						}
						else
						{
							CSkillData cskillData = new CSkillData();
							cskillData.SetTableData(elementById);
							await skill.OnInit(this.Owner, cskillData);
							this.m_allSkill.Add(id, skill);
						}
					}
				}
			}
		}

		public void RemoveAllSkill(List<int> ids)
		{
			for (int i = 0; i < ids.Count; i++)
			{
				this.RemoveSkill(ids[i]);
			}
		}

		public void RemoveSkill(int skillID)
		{
			CSkillBase cskillBase;
			if (this.m_allSkill.TryGetValue(skillID, out cskillBase) && cskillBase != null)
			{
				cskillBase.OnDeInit();
				this.m_allSkill.Remove(skillID);
			}
		}

		public CSkillBase GetSkill(int id)
		{
			CSkillBase cskillBase;
			if (this.m_allSkill.TryGetValue(id, out cskillBase))
			{
				return cskillBase;
			}
			return null;
		}

		public async Task PlaySkill(BattleReportData_PlaySkill reportData)
		{
			if (!this.m_allSkill.ContainsKey(reportData.m_skillId))
			{
				await this.AddSkill(reportData.m_skillId);
			}
			CSkillBase cskillBase;
			if (this.m_allSkill.TryGetValue(reportData.m_skillId, out cskillBase))
			{
				cskillBase.Play(reportData);
			}
		}

		public void PlaySkillComplete(BattleReportData_PlaySkillComplete reportData)
		{
			CSkillBase cskillBase;
			if (this.m_allSkill.TryGetValue(reportData.m_skillId, out cskillBase))
			{
				cskillBase.PlayComplete(reportData);
			}
		}

		private Dictionary<int, CSkillBase> m_allSkill = new Dictionary<int, CSkillBase>();

		private List<CSkillBase> list = new List<CSkillBase>();
	}
}
