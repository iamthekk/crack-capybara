using System;
using System.Collections.Generic;
using Habby.Event;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class CMemberData : IDisposable
	{
		public int InstanceID
		{
			get
			{
				return this.m_cardData.m_instanceID;
			}
		}

		public MemberCamp Camp
		{
			get
			{
				return this.m_cardData.m_camp;
			}
		}

		public MemberPos PosIndex
		{
			get
			{
				return this.m_cardData.m_posIndex;
			}
		}

		public bool IsMainMember
		{
			get
			{
				return this.m_cardData.m_isMainMember;
			}
		}

		public CardData cardData
		{
			get
			{
				return this.m_cardData;
			}
		}

		public MemberRoleType RoleType { get; private set; }

		public MemberMeatType MeatType { get; private set; }

		public bool IsEnemyPlayer { get; set; }

		public CMemberData()
		{
		}

		public CMemberData(CardData cardData)
		{
			this.m_cardData = new CardData();
			this.m_cardData.CloneFrom(cardData);
			this.SetMaxHp(-1);
			this.SetCurHp(-1);
		}

		public void SetMemberData(int instanceID, MemberCamp camp, bool isMainMembmer, MemberPos posIndex, FP curHp, FP maxHp)
		{
			this.m_cardData = new CardData(instanceID, this.m_id, camp, posIndex, isMainMembmer)
			{
				m_curHp = curHp
			};
			this.SetMaxHp(maxHp);
			this.SetCurHp(curHp);
		}

		public void SetTableData(GameMember_member member)
		{
			this.m_id = member.id;
			this.m_modelId = member.modelID;
			this.MeatType = (MemberMeatType)member.meatType;
			this.RoleType = (MemberRoleType)member.roleType;
			this.m_skillIDs = member.skillIDs.GetListInt('|');
			this.m_hitEffectID = member.hitEffectID;
			this.m_initSkinID = member.initSkinID;
			this.m_npcFunction = member.npcFunction;
			this.m_appearSoundID = member.appearSoundID;
			this.m_hitSoundID = member.hitSoundID;
			this.m_dieSoundID = member.dieSoundID;
			this.m_isShowRecharge = member.isShowRecharge > 0;
			if (!member.modelSacle.Equals(string.Empty))
			{
				string[] array = member.modelSacle.Replace("\n", "").Replace(" ", "").Replace("\t", "")
					.Replace("\r", "")
					.Split(',', StringSplitOptions.None);
				if (array.Length != 2)
				{
					HLog.LogError(HLog.ToColor("GameMember_member modelSacle is error.", 3));
					return;
				}
				float num = float.Parse(array[0]);
				float num2 = float.Parse(array[1]);
				this.m_RootScale = new Vector3(num, num2, 0f);
			}
		}

		public void SetSkills(List<int> skillIDs)
		{
			this.m_skillIDs = skillIDs;
		}

		public void SetCurHp(FP value)
		{
			this.m_curHp = MathTools.Clamp(value, FP._0, this.m_maxHp);
		}

		public void SetMaxHp(FP value)
		{
			this.m_maxHp = value;
		}

		public void SetCurRecharge(FP value)
		{
			this.m_curRecharge = value;
		}

		public void SetMaxRecharge(FP value)
		{
			this.m_maxRecharge = value;
		}

		public void AddCurRecharge(FP value)
		{
			this.m_curRecharge = FPMath.Clamp(this.m_curRecharge + value, FP._0, 100);
		}

		public void SetCurLegacyPower(int skillId, FP value)
		{
			this.m_curLegacyPowerDict[skillId] = value;
		}

		public void SetMaxLegacyPower(int skillId, FP value)
		{
			this.m_maxLegacyPowerDict[skillId] = value;
		}

		public void AddCurLegacyPower(int skillId, FP value)
		{
			this.m_curLegacyPowerDict[skillId] = FPMath.Clamp(this.m_curLegacyPowerDict[skillId] + value, FP._0, this.m_maxLegacyPowerDict[skillId]);
		}

		public void SetCurShield(FP value, bool isThunderShield = false, bool isDurianShield = false)
		{
			long num = this.m_curShield.AsLong();
			this.m_curShield = value;
			if (!num.Equals(this.m_curShield.AsLong()))
			{
				Action<FP, FP, bool, bool> curShieldChanged = this.m_curShieldChanged;
				if (curShieldChanged == null)
				{
					return;
				}
				curShieldChanged(num, this.m_curShield, isThunderShield, isDurianShield);
			}
		}

		public void SetIsUsedRevive(bool isUsed)
		{
			this.m_isUsedRevive = isUsed;
		}

		public void RefreshCardData(List<int> addSkillIDs, MemberAttributeData attributes, FP curHp, FP curRecharge, Dictionary<int, FP> curLegacyPower, bool isUsedRevive)
		{
			if (this.m_cardData == null)
			{
				return;
			}
			this.m_cardData.RefreshCardData(addSkillIDs, attributes, curHp, curRecharge, curLegacyPower, isUsedRevive);
		}

		public void SetAttack(FP attack)
		{
			this.m_attack = attack;
		}

		public void SetDefense(FP defense)
		{
			this.m_defense = defense;
		}

		public void Dispose()
		{
			Action<FP, FP, bool, bool> curShieldChanged = this.m_curShieldChanged;
			if (curShieldChanged != null)
			{
				EventStaticClass.UnRegAllEvent(curShieldChanged);
			}
			this.m_curShieldChanged = null;
			this.m_skillIDs = null;
			this.m_cardData = null;
		}

		private CardData m_cardData;

		public FP m_curHp;

		public FP m_maxHp;

		public int m_curCD;

		public int m_maxCD;

		public FP m_curRecharge;

		public FP m_maxRecharge = new FP(100);

		public Dictionary<int, FP> m_curLegacyPowerDict = new Dictionary<int, FP>();

		public Dictionary<int, FP> m_maxLegacyPowerDict = new Dictionary<int, FP>();

		public FP m_attack;

		public FP m_defense;

		public FP m_curShield = FP._0;

		public bool m_isUsedRevive;

		public Action<FP, FP, bool, bool> m_curShieldChanged;

		public int m_id;

		public int m_modelId;

		public Vector3 m_RootScale = Vector3.one;

		public List<int> m_skillIDs;

		public int m_hitEffectID;

		public int m_initSkinID;

		public int m_npcFunction;

		public int m_appearSoundID;

		public int m_hitSoundID;

		public int m_dieSoundID;

		public bool m_isShowRecharge;
	}
}
