using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
	public abstract class SBuffBase
	{
		public int OverlayLayer
		{
			get
			{
				return this.m_overlayLayer;
			}
			protected set
			{
				this.m_overlayLayer = value;
			}
		}

		public void Init(SBuffFactory factory, SBuffData buffData, SMemberBase owner, bool isPet, SMemberBase attacker, SMemberBase attackerPet)
		{
			this.m_guid = Guid.NewGuid().ToString();
			this.m_buffData = buffData;
			this.m_buffFactory = factory;
			this.m_owner = owner;
			this.m_isPet = isPet;
			this.m_attacker = attacker;
			this.m_attackerPet = attackerPet;
			this.m_curRound = buffData.m_duration;
			this.ReadParameters(buffData.m_parameters);
			this.OnInitBefore();
			this.OnTriggerAddAttributesOnce();
			this.m_buffData.RefreshTriggerAttributeData(this.m_owner, this.m_attacker, this.m_buffData.m_overlayCreate);
			this.isAdd = true;
			this.OnInit();
		}

		public void DeInit()
		{
			this.m_buffData.RemoveBuffAddAttributes(this.m_owner, this.m_attacker);
			this.RemoveAddBuffs();
			this.OnDeInit();
			this.m_buffFactory = null;
			this.m_owner = null;
			this.m_attacker = null;
			this.m_attackerPet = null;
			this.m_guid = null;
			SBuffData buffData = this.m_buffData;
			if (buffData != null)
			{
				buffData.Dispose();
			}
			this.m_buffData = null;
		}

		protected virtual void OnInitBefore()
		{
		}

		protected abstract void OnInit();

		protected abstract void ReadParameters(string parameters);

		protected abstract void OnDeInit();

		public void ChangeAttacker(SMemberBase attacker)
		{
			this.m_attacker = attacker;
		}

		public void SetEnabled()
		{
			this.SetOverlayLayer(this.m_buffData.m_overlayCreate);
		}

		public int GetDuration()
		{
			return this.m_curRound;
		}

		public void SetDuration(int duration)
		{
			if (duration > 0)
			{
				this.m_curRound = duration;
				return;
			}
			SBuffFactory buffFactory = this.m_buffFactory;
			if (buffFactory == null)
			{
				return;
			}
			buffFactory.RemoveBuff(this, false);
		}

		public void ResetDuration()
		{
			this.m_curRound = this.m_buffData.m_duration;
			this.isAdd = true;
		}

		public void AddDuration(int duration)
		{
			this.SetDuration(this.m_curRound + duration);
		}

		public virtual void OnChange()
		{
			SBuffData buffData = this.m_buffData;
			if (buffData == null)
			{
				return;
			}
			buffData.RefreshTriggerAttributeData(this.m_owner, this.m_attacker, this.m_overlayLayer);
		}

		public void SetOverlayLayer(int count)
		{
			if (count <= 0)
			{
				this.m_buffFactory.RemoveBuff(this, false);
				return;
			}
			if (this.OverlayLayer != count)
			{
				this.OverlayLayer = MathTools.Clamp(count, 1, this.m_buffData.m_overlayMax);
				if (count <= this.m_buffData.m_overlayMax)
				{
					this.m_buffData.AddBuffAddAttributes(this.m_owner, this.m_attacker);
				}
				this.OnChange();
				this.TriggerBuff(BuffTriggerTags.Default);
			}
			BattleReportData_BuffUpdate battleReportData_BuffUpdate = this.m_owner.m_controller.CreateReport<BattleReportData_BuffUpdate>();
			MemberAttributeData attribute = this.m_owner.memberData.attribute;
			battleReportData_BuffUpdate.SetData(this.m_attacker.m_instanceId, this.m_owner.m_instanceId, this.m_buffData.m_id, this.m_guid, attribute.GetAttack(), attribute.GetHpMax(), attribute.GetDefence(), this.OverlayLayer, this.GetDuration());
			this.m_owner.m_controller.AddReport(battleReportData_BuffUpdate, 0, true);
		}

		public void AddOverlayLayer(int addCount)
		{
			int num = this.OverlayLayer + addCount;
			this.SetOverlayLayer(num);
		}

		public void RefreshRound()
		{
			if (this.isAdd)
			{
				this.isAdd = false;
				if (this.m_buffData.isAddCountRound)
				{
					this.m_curRound--;
				}
			}
			else
			{
				this.m_curRound--;
			}
			if (this.m_curRound <= 0)
			{
				this.m_buffFactory.RemoveBuff(this, false);
				return;
			}
			BattleReportData_BuffUpdate battleReportData_BuffUpdate = this.m_owner.m_controller.CreateReport<BattleReportData_BuffUpdate>();
			MemberAttributeData attribute = this.m_owner.memberData.attribute;
			battleReportData_BuffUpdate.SetData(this.m_attacker.m_instanceId, this.m_owner.m_instanceId, this.m_buffData.m_id, this.m_guid, attribute.GetAttack(), attribute.GetHpMax(), attribute.GetDefence(), this.OverlayLayer, this.GetDuration());
			this.m_owner.m_controller.AddReport(battleReportData_BuffUpdate, 0, true);
		}

		protected void RemoveAddBuffs()
		{
			List<int> removeAddBuffs = this.m_buffData.m_removeAddBuffs;
			if (removeAddBuffs != null && removeAddBuffs.Count > 0)
			{
				this.m_buffFactory.AddBuffs(this.m_owner, removeAddBuffs);
			}
		}

		private void OnTriggerAddAttributesOnce()
		{
			this.m_buffData.AddBuffAddAttributesOnce(this.m_owner, this.m_attacker);
		}

		private void OnTriggerAttributes()
		{
			this.OnTriggerAttributesImpl(this.m_attacker, this.m_owner);
		}

		private void OnTriggerBuffs()
		{
			this.OnTriggerBuffsImpl(this.m_attacker, this.m_owner);
		}

		protected void OnTriggerAttributesImpl(SMemberBase attacker, SMemberBase target)
		{
			if (attacker == null || target == null || target.IsDeath)
			{
				return;
			}
			if (this.m_buffData.m_buffTriggerAttributeData.hurtDatas.Count > 0)
			{
				bool isPet = attacker.memberData.cardData.IsPet;
				SMemberBase smemberBase = (isPet ? attacker : null);
				SMemberBase smemberBase2 = (isPet ? this.m_owner.memberFactory.GetMainMember(attacker.memberData.Camp) : attacker);
				long num;
				target.ToHittedByBuff(smemberBase2, this, smemberBase, isPet, this.m_buffData.m_buffTriggerAttributeData.hurtDatas, this.m_buffData.m_buffTriggerAttributeData.energy.AsLong(), out num);
			}
		}

		protected void OnTriggerBuffsImpl(SMemberBase attacker, SMemberBase defender)
		{
			if (attacker == null || defender == null || defender.IsDeath)
			{
				return;
			}
			if (this.m_buffData.m_triggerBuffs.Length != 0)
			{
				defender.buffFactory.AddBuffs(attacker, this.m_buffData.m_triggerBuffs.ToList<int>());
			}
		}

		public virtual void TriggerBuff(BuffTriggerTags tag)
		{
			if (this.m_buffData == null || this.m_buffData.m_triggerTags == null)
			{
				return;
			}
			if (this.m_buffData.m_triggerTags.Contains(tag))
			{
				this.OnTrigger();
			}
		}

		protected virtual void OnTrigger()
		{
			this.OnTriggerAttributes();
			this.OnTriggerBuffs();
			this.m_triggerCounter += 1f;
		}

		public void OnForceTrigger()
		{
			this.OnTriggerAttributes();
			this.OnTriggerBuffs();
		}

		public SBuffFactory m_buffFactory;

		public SMemberBase m_owner;

		public bool m_isPet;

		public SMemberBase m_attacker;

		public SMemberBase m_attackerPet;

		public SBuffData m_buffData;

		public string m_guid = string.Empty;

		protected float m_triggerCounter;

		protected int m_curRound;

		protected int m_overlayLayer;

		private bool isAdd;
	}
}
