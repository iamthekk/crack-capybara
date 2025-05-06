using System;
using System.Collections.Generic;
using System.Linq;
using LocalModels.Bean;

namespace Server
{
	public class SBuffFactory
	{
		public SMemberBase m_owner { get; private set; }

		public SBuffFactory(SMemberBase owner)
		{
			this.m_owner = owner;
		}

		public void OnInit()
		{
			this.m_buffsForGUID.Clear();
		}

		public void OnDeInit()
		{
			this.RemoveAllBuffs();
			this.m_owner = null;
		}

		public void AddBuffs(SMemberBase attacker, List<int> buffIds)
		{
			if (attacker == null)
			{
				return;
			}
			if (buffIds == null || buffIds.Count == 0)
			{
				return;
			}
			for (int i = 0; i < buffIds.Count; i++)
			{
				this.AddBuff(attacker, buffIds[i]);
			}
		}

		public void AddBuff(SMemberBase attacker, int buffId)
		{
			if (attacker == null)
			{
				return;
			}
			GameBuff_buff elementById = this.m_owner.m_controller.Table.GetGameBuff_buffModelInstance().GetElementById(buffId);
			if (elementById == null)
			{
				return;
			}
			SBuffData sbuffData = new SBuffData();
			sbuffData.SetController(this.m_owner.m_controller);
			sbuffData.SetBuffTableData(elementById, this.m_owner.m_controller.Table.GetGameBuff_overlayTypeModelInstance());
			if (sbuffData.m_buffType == BuffType.Stun || sbuffData.m_buffType == BuffType.Frozen)
			{
				FP fp = this.m_owner.m_controller.Random01();
				FP controlImmunityRate = this.m_owner.memberData.attribute.ControlImmunityRate;
				FP ignoreControlImmunityRate = attacker.memberData.attribute.IgnoreControlImmunityRate;
				FP fp2 = controlImmunityRate - ignoreControlImmunityRate;
				if (fp <= fp2)
				{
					if (sbuffData.m_buffType == BuffType.Stun)
					{
						this.m_owner.ReportTextTips("battle_text_IgnoreStun");
						return;
					}
					if (sbuffData.m_buffType == BuffType.Frozen)
					{
						this.m_owner.ReportTextTips("battle_text_IgnoreFrozen");
					}
					return;
				}
			}
			BuffResultOverlayType buffResultOverlayType;
			BuffTimeOverlayType buffTimeOverlayType;
			SBuffBase sbuffBase = this.FindBuffByOverlayType(attacker, sbuffData, out buffResultOverlayType, out buffTimeOverlayType);
			bool flag = false;
			switch (buffResultOverlayType)
			{
			case BuffResultOverlayType.Overlay:
				if (sbuffBase != null)
				{
					this.RemoveBuff(sbuffBase, true);
				}
				flag = true;
				break;
			case BuffResultOverlayType.Coexist:
				if (this.IsBuffsCountMax())
				{
					return;
				}
				flag = true;
				break;
			case BuffResultOverlayType.AddLayer:
				if (sbuffBase != null)
				{
					this.RefreshTimeOverlay(sbuffBase, buffTimeOverlayType);
					sbuffBase.ChangeAttacker(attacker);
					sbuffBase.AddOverlayLayer(1);
				}
				else
				{
					flag = true;
				}
				break;
			case BuffResultOverlayType.Invalid:
				break;
			default:
				HLog.LogError("resultOverlayType is Error.");
				break;
			}
			if (flag)
			{
				this.CreateBuff(sbuffData, attacker);
			}
		}

		private void CreateBuff(SBuffData buffData, SMemberBase attacker)
		{
			GameBuff_buffType elementById = this.m_owner.m_controller.Table.GetGameBuff_buffTypeModelInstance().GetElementById((int)buffData.m_buffType);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Table:[GetGameBuff_buffType] is error. buffType = {0}", buffData.m_buffType));
				return;
			}
			SBuffBase sbuffBase = Activator.CreateInstance(Type.GetType(elementById.sClassName)) as SBuffBase;
			if (sbuffBase == null)
			{
				HLog.LogError(string.Format("CreateInstance is error. buffType = {0} buffName = {1}", buffData.m_buffType, elementById.sClassName));
				return;
			}
			bool isPet = attacker.memberData.cardData.IsPet;
			SMemberBase smemberBase = (isPet ? attacker : null);
			SMemberBase smemberBase2 = (isPet ? attacker.memberFactory.GetMainMember(attacker.memberData.Camp) : attacker);
			sbuffBase.Init(this, buffData, this.m_owner, isPet, smemberBase2, smemberBase);
			if (sbuffBase == null)
			{
				return;
			}
			sbuffBase.SetEnabled();
			this.m_buffsForGUID.Add(sbuffBase.m_guid, sbuffBase);
			BattleReportData_BuffAdd battleReportData_BuffAdd = this.m_owner.m_controller.CreateReport<BattleReportData_BuffAdd>();
			MemberAttributeData attribute = this.m_owner.memberData.attribute;
			battleReportData_BuffAdd.SetData(attacker.m_instanceId, this.m_owner.m_instanceId, buffData.m_id, sbuffBase.m_guid, attribute.GetAttack(), attribute.GetHpMax(), attribute.GetDefence(), sbuffBase.OverlayLayer, sbuffBase.GetDuration());
			this.m_owner.m_controller.AddReport(battleReportData_BuffAdd, 0, true);
		}

		private void RefreshTimeOverlay(SBuffBase buff, BuffTimeOverlayType timeOverlayType)
		{
			switch (timeOverlayType)
			{
			case BuffTimeOverlayType.Overlay:
				buff.AddDuration(buff.m_buffData.m_duration);
				buff.OnChange();
				return;
			case BuffTimeOverlayType.Refresh:
				buff.ResetDuration();
				buff.OnChange();
				return;
			case BuffTimeOverlayType.Constant:
				break;
			default:
				HLog.LogError("SwitchTimeOverlay timeOverlayType is Error.");
				break;
			}
		}

		public void RemoveBuff(SBuffBase buff, bool beforeAddNew)
		{
			if (buff == null)
			{
				return;
			}
			BuffType buffType = buff.m_buffData.m_buffType;
			BattleReportData_BuffRemove battleReportData_BuffRemove = this.m_owner.m_controller.CreateReport<BattleReportData_BuffRemove>();
			MemberAttributeData attribute = buff.m_owner.memberData.attribute;
			battleReportData_BuffRemove.SetData(buff.m_attacker.m_instanceId, buff.m_owner.m_instanceId, buff.m_buffData.m_id, buff.m_guid, attribute.GetAttack(), attribute.GetHpMax(), attribute.GetDefence(), buff.OverlayLayer, buff.GetDuration());
			this.m_owner.m_controller.AddReport(battleReportData_BuffRemove, 0, true);
			this.TriggerBuffs(BuffTriggerTags.BuffRemove);
			this.m_buffsForGUID.Remove(buff.m_guid);
			buff.DeInit();
			if (!beforeAddNew)
			{
				this.BuffCheckRemoveAttribute(buffType);
			}
		}

		public void RemoveBuffByTypes(List<int> buffTypes)
		{
			if (buffTypes == null || buffTypes.Count == 0)
			{
				return;
			}
			List<SBuffBase> list = this.m_buffsForGUID.Values.ToList<SBuffBase>();
			for (int i = 0; i < list.Count; i++)
			{
				if (buffTypes.Contains((int)list[i].m_buffData.m_buffType))
				{
					this.RemoveBuff(list[i], false);
				}
			}
		}

		public void RemoveAllBuffs()
		{
			List<SBuffBase> list = this.m_buffsForGUID.Values.ToList<SBuffBase>();
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveBuff(list[i], false);
			}
			this.m_buffsForGUID.Clear();
		}

		public void TriggerBuffs(BuffTriggerTags tag)
		{
			List<SBuffBase> list = this.m_buffsForGUID.Values.ToList<SBuffBase>();
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				SBuffData buffData = list[i].m_buffData;
				if (buffData.IsHaveTriggerAttributes() && buffData.IsHaveTriggerTags(tag))
				{
					flag = true;
					break;
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].TriggerBuff(tag);
			}
			if (flag)
			{
				this.m_owner.m_controller.AddCurFrame(15);
			}
		}

		private SBuffBase FindBuffByOverlayType(SMemberBase attacker, SBuffData buffData, out BuffResultOverlayType resOverlayType, out BuffTimeOverlayType timeOverlayTpye)
		{
			List<KeyValuePair<string, SBuffBase>> list = this.m_buffsForGUID.ToList<KeyValuePair<string, SBuffBase>>();
			SBuffBase sbuffBase = null;
			resOverlayType = BuffResultOverlayType.Coexist;
			timeOverlayTpye = BuffTimeOverlayType.Refresh;
			for (int i = 0; i < list.Count; i++)
			{
				SBuffBase value = list[i].Value;
				if (value.m_buffData.m_overlayType.Equals(buffData.m_overlayType))
				{
					BuffResultOverlayType buffResultOverlayType;
					BuffTimeOverlayType buffTimeOverlayType;
					if (value.m_attacker.m_instanceId.Equals(attacker.m_instanceId))
					{
						buffResultOverlayType = buffData.m_sameResultOverlayType;
						buffTimeOverlayType = buffData.m_sameTimeOverlayType;
					}
					else
					{
						buffResultOverlayType = buffData.m_diffResultOverlayType;
						buffTimeOverlayType = buffData.m_diffTimeOverlayType;
					}
					if (buffResultOverlayType == BuffResultOverlayType.Invalid)
					{
						resOverlayType = buffResultOverlayType;
						timeOverlayTpye = buffTimeOverlayType;
						sbuffBase = null;
						break;
					}
					if (buffResultOverlayType == BuffResultOverlayType.Overlay || buffResultOverlayType == BuffResultOverlayType.AddLayer)
					{
						timeOverlayTpye = buffTimeOverlayType;
						resOverlayType = buffResultOverlayType;
						sbuffBase = value;
						break;
					}
					if (buffResultOverlayType == BuffResultOverlayType.Coexist)
					{
						timeOverlayTpye = buffTimeOverlayType;
					}
				}
			}
			return sbuffBase;
		}

		public List<SBuffBase> GetBuffListByType(BuffType buffType)
		{
			List<SBuffBase> list = new List<SBuffBase>();
			foreach (KeyValuePair<string, SBuffBase> keyValuePair in this.m_buffsForGUID)
			{
				if (keyValuePair.Value.m_buffData.m_buffType == buffType)
				{
					list.Add(keyValuePair.Value);
				}
			}
			return list;
		}

		private bool IsBuffsCountMax()
		{
			return this.GetBuffsCount() >= this.buffCountUpperLimit;
		}

		private int GetBuffsCount()
		{
			return this.m_buffsForGUID.Count;
		}

		public List<SBuffBase> GetBuffs(int[] ids)
		{
			List<SBuffBase> list = new List<SBuffBase>();
			List<int> list2 = ids.ToList<int>();
			List<SBuffBase> list3 = this.m_buffsForGUID.Values.ToList<SBuffBase>();
			for (int i = 0; i < list3.Count; i++)
			{
				SBuffBase sbuffBase = list3[i];
				if (list2.Contains(sbuffBase.m_buffData.m_id))
				{
					list.Add(sbuffBase);
				}
			}
			return list;
		}

		public List<SBuffBase> GetBuffs(int id, int overlayType)
		{
			List<SBuffBase> list = new List<SBuffBase>();
			foreach (SBuffBase sbuffBase in this.m_buffsForGUID.Values)
			{
				if (sbuffBase.m_buffData.m_id == id || sbuffBase.m_buffData.m_overlayType == overlayType)
				{
					list.Add(sbuffBase);
				}
			}
			return list;
		}

		public List<SBuffBase> GetBuffsByOverlayType(int[] overlayTypes)
		{
			List<SBuffBase> list = new List<SBuffBase>();
			List<int> list2 = overlayTypes.ToList<int>();
			List<SBuffBase> list3 = this.m_buffsForGUID.Values.ToList<SBuffBase>();
			for (int i = 0; i < list3.Count; i++)
			{
				SBuffBase sbuffBase = list3[i];
				if (list2.Contains(sbuffBase.m_buffData.m_overlayType))
				{
					list.Add(sbuffBase);
				}
			}
			return list;
		}

		public void RefreshAllRoundCD()
		{
			List<SBuffBase> list = this.m_buffsForGUID.Values.ToList<SBuffBase>();
			for (int i = 0; i < list.Count; i++)
			{
				list[i].RefreshRound();
			}
		}

		public bool IsHasStun
		{
			get
			{
				return this.IsInBuffStateType(BuffStateType.Stun);
			}
		}

		public bool IsHasFreeze
		{
			get
			{
				return this.IsInBuffStateType(BuffStateType.Freeze);
			}
		}

		public bool IsHasBurn
		{
			get
			{
				return this.IsInBuffStateType(BuffStateType.Burn);
			}
		}

		public bool IsHasPoison
		{
			get
			{
				return this.IsInBuffStateType(BuffStateType.Poison);
			}
		}

		public bool IsVerdict
		{
			get
			{
				return this.IsInBuffStateType(BuffStateType.Verdict);
			}
		}

		public bool IsDefenseReduction
		{
			get
			{
				return this.IsInBuffStateType(BuffStateType.DefenseReduction);
			}
		}

		public bool IsVulnerability
		{
			get
			{
				return this.IsInBuffStateType(BuffStateType.Vulnerability);
			}
		}

		private bool IsInBuffStateType(BuffStateType type)
		{
			using (Dictionary<string, SBuffBase>.ValueCollection.Enumerator enumerator = this.m_buffsForGUID.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.m_buffData.m_buffStateType == (int)type)
					{
						return true;
					}
				}
			}
			return false;
		}

		public FP BurnLayerCount
		{
			get
			{
				return this.GetBuffStateTypeLayer(BuffStateType.Burn);
			}
		}

		private FP GetBuffStateTypeLayer(BuffStateType type)
		{
			FP fp = 0;
			foreach (SBuffBase sbuffBase in this.m_buffsForGUID.Values)
			{
				if (sbuffBase.m_buffData.m_buffStateType == (int)type)
				{
					fp = ((sbuffBase.OverlayLayer > fp) ? sbuffBase.OverlayLayer : fp);
				}
			}
			return FP._0;
		}

		private void BuffCheckRemoveAttribute(BuffType buffType)
		{
			if (buffType == BuffType.Shield)
			{
				List<SBuffBase> buffListByType = this.GetBuffListByType(BuffType.Shield);
				if (buffListByType == null || buffListByType.Count <= 0)
				{
					this.m_owner.memberData.ChangeShield(-this.m_owner.memberData.CurShield);
				}
			}
		}

		private readonly int buffCountUpperLimit = 20;

		private Dictionary<string, SBuffBase> m_buffsForGUID = new Dictionary<string, SBuffBase>();
	}
}
