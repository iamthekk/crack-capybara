using System;
using LocalModels.Bean;

namespace Server
{
	public class SBulletFactory
	{
		public void OnInit()
		{
		}

		public void OnDeInit()
		{
		}

		public void SetSkill(SSkillBase skill)
		{
			this.m_skill = skill;
		}

		public void OnUpdate(float deltaTime)
		{
		}

		public SBulletBase CreateBullet(CreateBulletData createBulletData)
		{
			int bulletID = createBulletData.m_bulletID;
			if (bulletID == 0)
			{
				return null;
			}
			GameBullet_bullet elementById = this.m_skill.Owner.m_controller.Table.GetGameBullet_bulletModelInstance().GetElementById(bulletID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("BulletFactory CreateBullet bulletId:{0} is null", bulletID));
				return null;
			}
			GameBullet_bulletType elementById2 = this.m_skill.Owner.m_controller.Table.GetGameBullet_bulletTypeModelInstance().GetElementById(elementById.bulletType);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("Tabla[GameBullet_bulletType] is Error.bulletTable.bulletType = {0}", elementById.bulletType));
				return null;
			}
			SBulletBase sbulletBase = Activator.CreateInstance(Type.GetType(elementById2.sClassName)) as SBulletBase;
			if (sbulletBase == null)
			{
				HLog.LogError("SBulletFactory.AddBullet   SBulletBase == null   cClassName = " + elementById2.sClassName);
				return null;
			}
			SBulletData sbulletData = new SBulletData();
			sbulletData.SetController(this.m_skill.Owner.m_controller);
			sbulletData.SetTableData(elementById);
			sbulletData.SetFireBulletID(createBulletData.m_fireBulletID);
			sbulletBase.SetBulletData(sbulletData);
			sbulletBase.OnReadParameters(elementById.parameters);
			sbulletBase.SetSkill(createBulletData.m_skill);
			sbulletBase.SetSelectTargets(createBulletData.m_selectTargets);
			sbulletBase.m_isLastBullet = createBulletData.m_isLastBullet;
			sbulletBase.OnInit();
			return sbulletBase;
		}

		public SSkillBase m_skill;
	}
}
