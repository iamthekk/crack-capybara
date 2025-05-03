using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;

namespace HotFix.Client
{
	public class CBuffFactory
	{
		public CBuffFactory(CMemberBase member)
		{
			this.m_owner = member;
		}

		public async Task OnInit()
		{
			await Task.CompletedTask;
		}

		public void OnDeInit()
		{
			this.m_owner = null;
			List<KeyValuePair<int, List<CBuffBase>>> list = this.m_buffContainer.ToList<KeyValuePair<int, List<CBuffBase>>>();
			for (int i = 0; i < list.Count; i++)
			{
				List<CBuffBase> list2 = list[i].Value.ToList<CBuffBase>();
				for (int j = 0; j < list2.Count; j++)
				{
					list2[j].DeInit(false);
				}
				list2.Clear();
			}
			this.m_buffContainer.Clear();
			this.m_buffContainer = null;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = this.m_buffContainer.Count - 1; i >= 0; i--)
			{
				List<CBuffBase> value = this.m_buffContainer[i].Value;
				if (value != null)
				{
					for (int j = value.Count - 1; j >= 0; j--)
					{
						CBuffBase cbuffBase = value[j];
						if (cbuffBase != null)
						{
							cbuffBase.Update(deltaTime, unscaledDeltaTime);
						}
					}
				}
			}
		}

		public async Task AddBuff(string guid, int buffId, CMemberBase attacker)
		{
			GameBuff_buff elementById = GameApp.Table.GetManager().GetGameBuff_buffModelInstance().GetElementById(buffId);
			if (elementById == null)
			{
				HLog.LogError(HLog.ToColor(string.Format("Not found [GameBuff] table buffid = {0}", buffId), 3));
			}
			else
			{
				CBuffData cbuffData = new CBuffData();
				cbuffData.SetTableData(elementById);
				this.CreateBuff(guid, cbuffData, attacker);
			}
		}

		private void CreateBuff(string guid, CBuffData buffData, CMemberBase attacker)
		{
			GameBuff_buffType elementById = GameApp.Table.GetManager().GetGameBuff_buffTypeModelInstance().GetElementById((int)buffData.m_buffType);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Table:[GetGameBuff_buffType] is error. buffType = {0}", buffData.m_buffType));
				return;
			}
			CBuffBase cbuffBase = Activator.CreateInstance(Type.GetType(elementById.cClassName)) as CBuffBase;
			if (cbuffBase == null)
			{
				HLog.LogError(string.Format("CreateInstance is error. buffType = {0} buffName = {1}", buffData.m_buffType, elementById.cClassName));
				return;
			}
			cbuffBase.SetGuid(guid);
			cbuffBase.Init(this, buffData, this.m_owner, attacker, buffData.m_bodyDirection);
			if (!this.BuffContainerContains(buffData.m_id))
			{
				this.m_buffContainer.Add(new KeyValuePair<int, List<CBuffBase>>(buffData.m_id, new List<CBuffBase>()));
			}
			List<CBuffBase> list;
			if (this.BuffContainerTryGet(buffData.m_id, out list))
			{
				list.Add(cbuffBase);
			}
			else
			{
				HLog.LogError(string.Format("TryGetBuffs is error. buffId = {0}", buffData.m_id));
			}
			if (buffData.m_soundID > 0)
			{
				GameApp.Sound.PlayClip(buffData.m_soundID, 1f);
			}
		}

		public async Task RemoveBuff(string guid, int buffId, CMemberBase attacker)
		{
			List<CBuffBase> list;
			if (this.BuffContainerTryGet(buffId, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					CBuffBase cbuffBase = list[i];
					if (cbuffBase.m_guid.Equals(guid))
					{
						cbuffBase.DeInit(false);
						list.RemoveAt(i);
						break;
					}
				}
				if (list.Count <= 0)
				{
					this.BuffContainerTryRemoveKey(buffId);
				}
			}
			await Task.CompletedTask;
		}

		public void RemoveAllBuff(bool removeEffectImmediate)
		{
			for (int i = this.m_buffContainer.Count - 1; i >= 0; i--)
			{
				KeyValuePair<int, List<CBuffBase>> keyValuePair = this.m_buffContainer[i];
				for (int j = keyValuePair.Value.Count - 1; j >= 0; j--)
				{
					CBuffBase cbuffBase = keyValuePair.Value[j];
					if (cbuffBase != null)
					{
						cbuffBase.DeInit(removeEffectImmediate);
					}
				}
				List<CBuffBase> value = keyValuePair.Value;
				if (value != null)
				{
					value.Clear();
				}
			}
			this.m_buffContainer.Clear();
		}

		private bool BuffContainerTryGet(int buffId, out List<CBuffBase> buffs)
		{
			buffs = null;
			for (int i = 0; i < this.m_buffContainer.Count; i++)
			{
				if (this.m_buffContainer[i].Key == buffId)
				{
					buffs = this.m_buffContainer[i].Value;
					return true;
				}
			}
			return false;
		}

		private bool BuffContainerTryRemoveKey(int buffId)
		{
			for (int i = this.m_buffContainer.Count - 1; i >= 0; i--)
			{
				if (this.m_buffContainer[i].Key == buffId && (this.m_buffContainer[i].Value == null || this.m_buffContainer[i].Value.Count <= 0))
				{
					this.m_buffContainer.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		private bool BuffContainerContains(int buffId)
		{
			for (int i = 0; i < this.m_buffContainer.Count; i++)
			{
				if (this.m_buffContainer[i].Key == buffId)
				{
					return true;
				}
			}
			return false;
		}

		public void OnTrigger(int buffID, int layer)
		{
			List<CBuffBase> list;
			if (this.BuffContainerTryGet(buffID, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i].OnTrigger(layer);
				}
			}
		}

		public CBuffBase GetCBuff(string guid, int buffId)
		{
			for (int i = this.m_buffContainer.Count - 1; i >= 0; i--)
			{
				if (this.m_buffContainer[i].Key == buffId)
				{
					List<CBuffBase> value = this.m_buffContainer[i].Value;
					for (int j = value.Count - 1; j >= 0; j--)
					{
						if (value[j].m_guid.Equals(guid))
						{
							return value[j];
						}
					}
					break;
				}
			}
			return null;
		}

		public CMemberBase m_owner;

		public List<KeyValuePair<int, List<CBuffBase>>> m_buffContainer = new List<KeyValuePair<int, List<CBuffBase>>>();
	}
}
