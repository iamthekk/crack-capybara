using System;
using System.Collections.Generic;
using System.Linq;
using LocalModels.Bean;
using LocalModels.Model;

namespace Server
{
	public class SBuffData : IDisposable
	{
		public GameBuff_buff m_buffTableData { get; private set; }

		public void SetController(BaseBattleServerController controller)
		{
			this.m_controller = controller;
		}

		public void SetBuffTableData(GameBuff_buff data, GameBuff_overlayTypeModel overlayTypeModel)
		{
			this.m_buffTableData = data;
			this.m_id = data.id;
			this.m_buffType = (BuffType)data.buffType;
			this.m_freedType = (BuffFreedType)data.freedType;
			this.m_triggerBuffs = data.triggerBuffs;
			this.m_overlayType = data.overlayType;
			this.m_buffStateType = data.buffStateType;
			this.m_duration = data.duration;
			this.isAddCountRound = data.addCountRound > 0;
			GameBuff_overlayType elementById = overlayTypeModel.GetElementById(this.m_overlayType);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Buff overlayTypeTable is null, id = {0}", this.m_overlayType));
				return;
			}
			this.m_sameResultOverlayType = (BuffResultOverlayType)elementById.sameResultOverlayType;
			this.m_diffResultOverlayType = (BuffResultOverlayType)elementById.diffResultOverlayType;
			this.m_sameTimeOverlayType = (BuffTimeOverlayType)elementById.sameTimeOverlayType;
			this.m_diffTimeOverlayType = (BuffTimeOverlayType)elementById.diffTimeOverlayType;
			this.m_triggerTags = new List<BuffTriggerTags>();
			for (int i = 0; i < data.triggerTags.Length; i++)
			{
				this.m_triggerTags.Add((BuffTriggerTags)data.triggerTags[i]);
			}
			this.m_overlayMax = data.overlayMax;
			this.m_parameters = data.parameters;
			this.m_overlayCreate = 1;
			this.m_buffAddAttributesData = new BuffAddAttributesData();
			this.m_buffTriggerAttributeData = new BuffTriggerAttributeData();
			this.m_removeAddBuffs = data.removeAddBuffs.ToList<int>();
		}

		public void RefreshTriggerAttributeData(SMemberBase owner, SMemberBase attacker, int layer)
		{
			if (this.m_buffTriggerAttributeData == null)
			{
				return;
			}
			if (this.m_buffTableData == null)
			{
				return;
			}
			this.m_buffTriggerAttributeData.RefreshAttributes(this.m_buffTableData, owner, attacker, layer);
		}

		public void AddBuffAddAttributes(SMemberBase owner, SMemberBase attacker)
		{
			if (this.m_buffAddAttributesData == null)
			{
				return;
			}
			if (this.m_buffTableData == null)
			{
				return;
			}
			this.m_buffAddAttributesData.AddAttributes(this.m_buffTableData, owner, attacker);
		}

		public void RemoveBuffAddAttributes(SMemberBase owner, SMemberBase attacker)
		{
			if (this.m_buffAddAttributesData == null)
			{
				return;
			}
			if (this.m_buffTableData == null)
			{
				return;
			}
			this.m_buffAddAttributesData.RemoveAttributes(this.m_buffTableData, owner, attacker);
		}

		public void AddBuffAddAttributesOnce(SMemberBase owner, SMemberBase attacker)
		{
			if (this.m_buffAddAttributesData == null)
			{
				return;
			}
			if (this.m_buffTableData == null)
			{
				return;
			}
			this.m_buffAddAttributesData.AddAttributesOnce(this.m_buffTableData, owner, attacker);
		}

		public bool IsHaveTriggerAttributes()
		{
			return !this.m_buffTableData.triggerAttributes.Equals(string.Empty);
		}

		public bool IsHaveTriggerTags(BuffTriggerTags tag)
		{
			return this.m_triggerTags.Contains(tag);
		}

		public void Dispose()
		{
			this.m_triggerTags = null;
			this.m_triggerBuffs = null;
			this.m_parameters = null;
			this.m_buffTableData = null;
			this.m_buffAddAttributesData = null;
			this.m_buffTriggerAttributeData = null;
			this.m_controller = null;
		}

		public int m_id;

		public BuffType m_buffType = BuffType.Default;

		public BuffFreedType m_freedType = BuffFreedType.Normal;

		public int m_duration;

		public int m_overlayType;

		public int m_buffStateType;

		public BuffResultOverlayType m_sameResultOverlayType;

		public BuffResultOverlayType m_diffResultOverlayType;

		public BuffTimeOverlayType m_sameTimeOverlayType;

		public BuffTimeOverlayType m_diffTimeOverlayType;

		public List<BuffTriggerTags> m_triggerTags;

		public int[] m_triggerBuffs;

		public int m_overlayMax = 1;

		public int m_overlayCreate = 1;

		public bool isAddCountRound;

		public string m_parameters;

		public List<int> m_removeAddBuffs;

		public BuffAddAttributesData m_buffAddAttributesData;

		public BuffTriggerAttributeData m_buffTriggerAttributeData;

		private BaseBattleServerController m_controller;
	}
}
