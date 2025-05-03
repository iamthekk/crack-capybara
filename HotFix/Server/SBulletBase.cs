using System;
using System.Collections.Generic;

namespace Server
{
	public class SBulletBase
	{
		public bool m_isLastBullet { get; set; }

		public void SetSkill(SSkillBase skill)
		{
			this.m_skill = skill;
			this.m_owner = this.m_skill.Owner;
		}

		public void SetBulletData(SBulletData bulletData)
		{
			this.m_bulletData = bulletData;
		}

		public void SetSelectTargets(List<SkillSelectTargetData> selectTargets)
		{
			this.m_selectTargets = selectTargets;
		}

		public virtual void OnInit()
		{
			List<SMemberBase> list = new List<SMemberBase>();
			if (this.m_selectTargets != null)
			{
				for (int i = 0; i < this.m_selectTargets.Count; i++)
				{
					BattleReportData_CreateBullet battleReportData_CreateBullet = this.m_owner.m_controller.CreateReport<BattleReportData_CreateBullet>();
					battleReportData_CreateBullet.BulletID = this.m_bulletData.m_bulletId;
					battleReportData_CreateBullet.FireBulletID = this.m_bulletData.m_fireBulletID;
					battleReportData_CreateBullet.SkillID = this.m_skill.skillData.m_id;
					battleReportData_CreateBullet.TargetInstanceID = this.m_selectTargets[i].m_target.m_instanceId;
					battleReportData_CreateBullet.AttackerInstanceID = this.m_owner.m_instanceId;
					battleReportData_CreateBullet.IsMainTarget = i.Equals(0);
					this.m_owner.m_controller.AddReport(battleReportData_CreateBullet, 0, true);
					list.Add(this.m_selectTargets[i].m_target);
				}
				SSkillBase skill = this.m_skill;
				if (skill != null)
				{
					skill.OnHitted(list, this);
				}
			}
			this.OnDeInit();
		}

		public virtual void OnDeInit()
		{
			this.m_owner = null;
			this.m_skill = null;
			SBulletData bulletData = this.m_bulletData;
			if (bulletData != null)
			{
				bulletData.OnDispose();
			}
			this.m_bulletData = null;
			this.m_bulletFactory = null;
			this.m_selectTargets = null;
		}

		public virtual void OnReadParameters(string parameters)
		{
		}

		public virtual void OnUpdate(float deltaTime)
		{
		}

		public SMemberBase m_owner;

		public SSkillBase m_skill;

		public SBulletData m_bulletData;

		public SBulletFactory m_bulletFactory;

		private List<SkillSelectTargetData> m_selectTargets;
	}
}
