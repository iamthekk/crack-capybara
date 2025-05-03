using System;
using System.Collections.Generic;
using System.Linq;
using LocalModels.Bean;

namespace Server
{
	public class SMemberFactory
	{
		public Dictionary<int, SMemberBase> allMember
		{
			get
			{
				return this.m_members;
			}
		}

		public int RevivedCount
		{
			get
			{
				return this.m_revivedCount;
			}
			set
			{
				this.m_revivedCount = value;
			}
		}

		public FP MainTotalDamage { get; private set; } = FP._0;

		public SMemberFactory(BaseBattleServerController controller)
		{
			this.m_controller = controller;
		}

		public void OnInit()
		{
			List<CardData> cardDatas = this.m_controller.InData.m_cardDatas;
			for (int i = 0; i < cardDatas.Count; i++)
			{
				CardData cardData = cardDatas[i];
				if (cardData != null)
				{
					this.CreateMember(cardData);
				}
			}
		}

		public void OnDeInit()
		{
			this.m_friendlyMembers.Clear();
			this.m_enemyMembers.Clear();
			List<SMemberBase> list = this.m_members.Values.ToList<SMemberBase>();
			for (int i = 0; i < list.Count; i++)
			{
				SMemberBase smemberBase = list[i];
				smemberBase.DeInit();
				list.Remove(smemberBase);
			}
		}

		public List<SMemberBase> CreateWaveMember(List<CardData> cardDatas)
		{
			List<SMemberBase> list = new List<SMemberBase>();
			for (int i = 0; i < cardDatas.Count; i++)
			{
				SMemberBase smemberBase = this.CreateMember(cardDatas[i]);
				if (smemberBase != null)
				{
					list.Add(smemberBase);
				}
			}
			return list;
		}

		public SMemberBase CreateMember(CardData cardData)
		{
			if (cardData == null)
			{
				return null;
			}
			GameMember_member elementById = this.m_controller.Table.GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
			if (elementById == null)
			{
				return null;
			}
			GameMember_aiType elementById2 = this.m_controller.Table.GetGameMember_aiTypeModelInstance().GetElementById(elementById.aiTypeID);
			if (elementById2 == null)
			{
				return null;
			}
			SMemberBase smemberBase = Activator.CreateInstance(Type.GetType(elementById2.sClassName)) as SMemberBase;
			if (smemberBase == null)
			{
				HLog.LogError(string.Format("CreateMember is null cardData.m_memberID = {0}", cardData.m_memberID));
				return null;
			}
			smemberBase.SetController(this.m_controller);
			smemberBase.SetMemberFactory(this);
			smemberBase.SetMemberData(cardData, elementById);
			smemberBase.SetParameters(elementById.parameters);
			smemberBase.Init();
			this.m_members[smemberBase.m_instanceId] = smemberBase;
			if (!cardData.IsPet)
			{
				if (cardData.m_camp == MemberCamp.Friendly)
				{
					this.m_friendlyMembers[smemberBase.m_instanceId] = smemberBase;
				}
				else if (cardData.m_camp == MemberCamp.Enemy)
				{
					this.m_enemyMembers[smemberBase.m_instanceId] = smemberBase;
				}
			}
			return smemberBase;
		}

		public void RemoveMember(SMemberBase member)
		{
			member.DeInit();
			this.m_members.Remove(member.m_instanceId);
		}

		public Dictionary<int, SMemberBase> GetAllMember()
		{
			return this.m_members;
		}

		public SMemberBase GetMember(int instanceID)
		{
			if (this.m_members.ContainsKey(instanceID))
			{
				return this.m_members[instanceID];
			}
			return null;
		}

		public List<SMemberBase> GetAllMemberList()
		{
			return this.m_members.Values.ToList<SMemberBase>();
		}

		public List<SMemberBase> GetMembersByCamp(MemberCamp camp)
		{
			List<SMemberBase> list = new List<SMemberBase>();
			List<SMemberBase> allMemberList = this.GetAllMemberList();
			for (int i = 0; i < allMemberList.Count; i++)
			{
				SMemberBase smemberBase = allMemberList[i];
				if (smemberBase.memberData.Camp == camp)
				{
					list.Add(smemberBase);
				}
			}
			return list;
		}

		public SMemberBase GetMainMember(MemberCamp camp)
		{
			List<SMemberBase> membersByCamp = this.GetMembersByCamp(camp);
			for (int i = 0; i < membersByCamp.Count; i++)
			{
				SMemberBase smemberBase = membersByCamp[i];
				if (smemberBase.memberData.cardData.m_isMainMember)
				{
					return smemberBase;
				}
			}
			return null;
		}

		public bool OnGameOver()
		{
			bool flag = true;
			foreach (KeyValuePair<int, SMemberBase> keyValuePair in this.m_friendlyMembers)
			{
				if (!keyValuePair.Value.IsDeath)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.m_controller.OnGameEnd(false);
				return true;
			}
			bool flag2 = true;
			foreach (KeyValuePair<int, SMemberBase> keyValuePair2 in this.m_enemyMembers)
			{
				if (!keyValuePair2.Value.IsDeath)
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				bool flag3;
				this.m_controller.TryCreateNewWave(true, out flag3);
				return flag3;
			}
			return false;
		}

		public void OnEnemyFreezeAfter(SBuffBase buff)
		{
			List<SMemberBase> list = ((buff.m_owner.memberData.Camp == MemberCamp.Friendly) ? this.GetMembersByCamp(MemberCamp.Enemy) : this.GetMembersByCamp(MemberCamp.Friendly));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].skillFactory.CheckPlay(SkillTriggerType.EnemyFreezeAfter, buff);
			}
		}

		public void MainRoleDamage(FP damage)
		{
			if (damage <= 0)
			{
				return;
			}
			this.MainTotalDamage += damage;
			string text = "|" + damage.ToString();
			this.sbMainTotalDamage += text;
		}

		private BaseBattleServerController m_controller;

		private Dictionary<int, SMemberBase> m_members = new Dictionary<int, SMemberBase>();

		private Dictionary<int, SMemberBase> m_friendlyMembers = new Dictionary<int, SMemberBase>();

		private Dictionary<int, SMemberBase> m_enemyMembers = new Dictionary<int, SMemberBase>();

		private int m_revivedCount;

		public string sbMainTotalDamage = string.Empty;
	}
}
