using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;

namespace HotFix.Client
{
	public class CSkillData
	{
		public int m_startPrefabID { get; set; }

		public int m_legacyBindmodelId { get; set; }

		public MemberBodyPosType m_startPosID { get; set; } = 4;

		public PointRotationDirection m_startDirectionID { get; set; }

		public int m_sagecraftType { get; set; }

		public void SetTableData(GameSkill_skill skill)
		{
			this.m_id = skill.id;
			this.m_initCD = skill.initCD;
			this.m_maxCD = skill.CD;
			this.m_soundID = skill.soundID;
			this.m_freedType = (SkillFreedType)skill.freedType;
			int animID = skill.animID;
			if (!animID.Equals(0))
			{
				GameSkill_skillAnimation elementById = GameApp.Table.GetManager().GetGameSkill_skillAnimationModelInstance().GetElementById(animID);
				this.m_AnimationEnterName = elementById.animationEnterName;
				this.m_AnimationAttackName = elementById.animationAttackName;
				this.m_AnimationExitName = elementById.animationExitName;
			}
			this.m_fireBullets = skill.fireBullets;
			this.m_startPrefabID = skill.startPrefabID;
			this.m_legacyBindmodelId = skill.legacyBindmodelId;
			List<int> listInt = skill.startPosID.GetListInt(',');
			if (listInt.Count == 2)
			{
				this.m_startPosID = listInt[0];
				this.m_startDirectionID = (PointRotationDirection)listInt[1];
			}
			this.m_sagecraftType = skill.sagecraftType;
			this.isBlack = skill.isBlack > 0;
			this.skillStartOwnerAddBuffs = skill.skillStartOwnerAddBuffs;
			this.skillEndOwnerAddBuffs = skill.skillEndOwnerAddBuffs;
			this.skillStartTargetAddBuffs = skill.skillStartTargetAddBuffs;
			this.skillEndTargetAddBuffs = skill.skillEndTargetAddBuffs;
			this.skillStartFriendAddBuffs = skill.skillStartFriendAddBuffs;
			this.skillEndFriendAddBuffs = skill.skillEndFriendAddBuffs;
		}

		public int m_id;

		public int m_initCD;

		public int m_maxCD;

		public int m_soundID;

		public SkillFreedType m_freedType;

		protected string m_AnimationEnterName = string.Empty;

		public string m_AnimationAttackName = string.Empty;

		protected string m_AnimationExitName = string.Empty;

		public int[] m_fireBullets;

		public bool isBlack;

		public string skillStartOwnerAddBuffs;

		public string skillEndOwnerAddBuffs;

		public string skillStartTargetAddBuffs;

		public string skillEndTargetAddBuffs;

		public string skillStartFriendAddBuffs;

		public string skillEndFriendAddBuffs;
	}
}
