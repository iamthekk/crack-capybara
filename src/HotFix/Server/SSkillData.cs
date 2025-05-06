using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class SSkillData
	{
		public bool IsHaveAnimation { get; private set; }

		public bool isSkillMove { get; private set; }

		public void SetMember(SMemberBase member)
		{
			this.m_owner = member;
		}

		public void SetController(BaseBattleServerController controller)
		{
			this.m_controller = controller;
		}

		public void SetTableData(GameSkill_skill skill)
		{
			this.m_id = skill.id;
			this.m_parameters = skill.parameters;
			this.m_initCD = skill.initCD;
			this.m_maxCD = skill.CD;
			this.m_recharge = skill.recharge;
			this.m_legacyPower = skill.legacyPower;
			this.m_legacyPowerMax = skill.legacyPowerMax;
			this.m_triggerContitions = skill.triggerConditions;
			this.m_selectIDs = skill.selectIDs;
			this.skillTypeId = skill.typeID;
			this.skillTag = skill.tag;
			this.skillTypeDamageAddParam = skill.skillTypeDamageAddParam;
			this.m_freedType = (SkillFreedType)skill.freedType;
			this.m_rangeType = (SkillRangeType)skill.rangeType;
			this.m_groupSelectMaxCount = skill.groupSelectMaxCount;
			this.IsHaveAnimation = !skill.animID.Equals(0);
			this.RefreshEventNodes(skill.animEventNodes);
			this.m_fireBullets = skill.fireBullets;
			this.isSkillMove = skill.moveType != 0;
			this.skillStartOwnerAddBuffs = skill.skillStartOwnerAddBuffs;
			this.skillEndOwnerAddBuffs = skill.skillEndOwnerAddBuffs;
			this.skillStartTargetAddBuffs = skill.skillStartTargetAddBuffs;
			this.skillEndTargetAddBuffs = skill.skillEndTargetAddBuffs;
			this.skillStartFriendAddBuffs = skill.skillStartFriendAddBuffs;
			this.skillEndFriendAddBuffs = skill.skillEndFriendAddBuffs;
			this.m_baseAttributes = skill.baseAttributes;
			this.m_hurtAttributes = skill.hurtAttributes;
			this.m_hurtAttributeDatas = this.m_hurtAttributes.GetListString('|');
			this.m_effectType = (SkillEffectType)skill.effectType;
			this.m_legacyAppearFrame = skill.lagacyAppearFrame;
			this.InitFireBullets();
		}

		public void OnDispose()
		{
			this.m_owner = null;
			this.m_parameters = null;
			this.m_selectIDs = null;
			this.m_animEventNodes.Clear();
			this.m_animEventNodes = null;
			this.m_baseAttributes = null;
			this.m_hurtAttributeDatas = null;
			this.skillStartOwnerAddBuffs = null;
			this.skillEndOwnerAddBuffs = null;
			this.skillStartTargetAddBuffs = null;
			this.skillEndTargetAddBuffs = null;
			this.fireBulletDataArr = null;
			this.m_controller = null;
		}

		public void SetMaxInitCD(int initCD, int minInitCD)
		{
			int num = this.m_initCD - initCD;
			this.m_initCD = MathTools.Clamp(num, minInitCD, num);
		}

		public void SetMaxCD(int CD, int minCD)
		{
			int num = this.m_maxCD - CD;
			this.m_maxCD = MathTools.Clamp(num, minCD, num);
		}

		public void RefreshEventNodes(string[] nodes)
		{
			this.m_animEventNodes = new List<SSkillData.EventNode>(nodes.Length);
			for (int i = 0; i < nodes.Length; i++)
			{
				string[] array = nodes[i].Split(',', StringSplitOptions.None);
				if (array.Length == 2)
				{
					SSkillData.EventNode eventNode = new SSkillData.EventNode(int.Parse(array[0]), array[1]);
					this.m_animEventNodes.Add(eventNode);
				}
			}
			for (int j = 0; j < this.m_animEventNodes.Count; j++)
			{
				if (!j.Equals(0))
				{
					this.m_animEventNodes[j].SetFrame(this.m_animEventNodes[j - 1].absFrame);
				}
			}
		}

		public void InitFireBullets()
		{
			int[] fireBullets = this.m_fireBullets;
			this.fireBulletDataArr = new SkillFireBulletData[fireBullets.Length];
			for (int i = 0; i < fireBullets.Length; i++)
			{
				SkillFireBulletData skillFireBulletData = new SkillFireBulletData();
				GameSkill_fireBullet elementById = this.m_owner.m_controller.Table.GetGameSkill_fireBulletModelInstance().GetElementById(fireBullets[i]);
				if (elementById == null)
				{
					HLog.LogError(string.Format("GameSkill_fireBulletModel is Error. skillID = {0}   fireBulletID = {1}", this.m_id, fireBullets[i]));
				}
				else
				{
					skillFireBulletData.SetTableData(i, elementById);
					this.fireBulletDataArr[i] = skillFireBulletData;
				}
			}
		}

		public bool IsHaveFirebulletData()
		{
			return this.fireBulletDataArr != null && this.fireBulletDataArr.Length != 0;
		}

		public void ChangeFireBullets(int fireBulletID)
		{
			GameSkill_fireBullet elementById = this.m_owner.m_controller.Table.GetGameSkill_fireBulletModelInstance().GetElementById(fireBulletID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("GameSkill_fireBulletModel is Error. fireBulletID = {0}", fireBulletID));
				return;
			}
			for (int i = 0; i < this.fireBulletDataArr.Length; i++)
			{
				this.fireBulletDataArr[i].SetTableData(i, elementById);
			}
		}

		public SkillFireBulletData GetFireBulletData(int index)
		{
			if (!this.IsHaveFirebulletData())
			{
				return null;
			}
			if (index < this.fireBulletDataArr.Length)
			{
				return this.fireBulletDataArr[index];
			}
			return this.fireBulletDataArr[0];
		}

		public List<int> GetSkillStartOwnerAddBuffs()
		{
			return this.GetSkillAddBuffs(this.skillStartOwnerAddBuffs);
		}

		public List<int> GetSkillEndOwnerAddBuffs()
		{
			return this.GetSkillAddBuffs(this.skillEndOwnerAddBuffs);
		}

		public List<int> GetSkillStartTargetAddBuffs()
		{
			return this.GetSkillAddBuffs(this.skillStartTargetAddBuffs);
		}

		public List<int> GetSkillEndTargetAddBuffs()
		{
			return this.GetSkillAddBuffs(this.skillEndTargetAddBuffs);
		}

		public List<int> GetSkillStartFriendAddBuffs()
		{
			return this.GetSkillAddBuffs(this.skillStartFriendAddBuffs);
		}

		public List<int> GetSkillEndFriendAddBuffs()
		{
			return this.GetSkillAddBuffs(this.skillEndFriendAddBuffs);
		}

		private List<int> GetSkillAddBuffs(string json)
		{
			List<int> list = new List<int>();
			if (json.Equals(string.Empty))
			{
				return list;
			}
			SSkillData.TriggerBuffData triggerBuffData = JsonManager.ToObject<SSkillData.TriggerBuffData>(json);
			if (triggerBuffData == null)
			{
				HLog.LogError("Json format is error. json:" + json);
			}
			for (int i = 0; i < triggerBuffData.buffIDs.Count; i++)
			{
				int num = triggerBuffData.buffIDs[i];
				if (this.m_controller.IsMatchProbability(triggerBuffData.probability))
				{
					list.Add(num);
				}
			}
			return list;
		}

		public SMemberBase m_owner;

		public int m_id;

		public string m_parameters = string.Empty;

		public int m_initCD;

		public int m_maxCD;

		public int m_recharge;

		public int m_legacyPower;

		public int m_legacyPowerMax;

		public string m_triggerContitions = string.Empty;

		public int[] m_selectIDs;

		public int skillTypeId;

		public int skillTag;

		public int[] skillTypeDamageAddParam;

		public SkillFreedType m_freedType;

		public SkillRangeType m_rangeType = SkillRangeType.Single;

		public int m_groupSelectMaxCount;

		public List<SSkillData.EventNode> m_animEventNodes;

		private int[] m_fireBullets;

		public SkillFireBulletData[] fireBulletDataArr;

		public string m_baseAttributes = string.Empty;

		public string m_hurtAttributes = string.Empty;

		public List<string> m_hurtAttributeDatas;

		public SkillEffectType m_effectType;

		public int m_legacyAppearFrame;

		private string skillStartOwnerAddBuffs = string.Empty;

		private string skillEndOwnerAddBuffs = string.Empty;

		private string skillStartTargetAddBuffs = string.Empty;

		private string skillEndTargetAddBuffs = string.Empty;

		private string skillStartFriendAddBuffs = string.Empty;

		private string skillEndFriendAddBuffs = string.Empty;

		private BaseBattleServerController m_controller;

		public class EventNode
		{
			public EventNode(int absFrame, string eventName)
			{
				this.absFrame = absFrame;
				this.eventName = eventName;
				this.frame = absFrame;
			}

			public void SetFrame(int lastFrame)
			{
				this.frame = this.absFrame - lastFrame;
			}

			public int absFrame;

			public string eventName = string.Empty;

			public int frame;
		}

		public class TriggerBuffData
		{
			public int probability;

			public List<int> buffIDs = new List<int>();
		}
	}
}
