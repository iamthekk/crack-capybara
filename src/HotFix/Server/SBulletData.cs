using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class SBulletData
	{
		public void SetController(BaseBattleServerController controller)
		{
			this.m_controller = controller;
		}

		public void SetTableData(GameBullet_bullet data)
		{
			this.m_bulletId = data.id;
			this.m_bulletType = data.bulletType;
			this.m_parameters = data.parameters;
			this.frame = data.frame;
			this.m_hitAddBuffs = data.hitAddBuffs;
		}

		public void SetFireBulletID(int id)
		{
			this.m_fireBulletID = id;
		}

		public List<int> GetHitAddBuffs()
		{
			return this.GetHitAddBuffs(this.m_hitAddBuffs);
		}

		private List<int> GetHitAddBuffs(string json)
		{
			List<int> list = new List<int>();
			if (json.Equals(string.Empty))
			{
				return list;
			}
			SBulletData.TriggerBuffData triggerBuffData = JsonManager.ToObject<SBulletData.TriggerBuffData>(json);
			if (triggerBuffData == null)
			{
				HLog.LogError("json format is error. json:" + json);
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

		public void OnDispose()
		{
			this.m_parameters = null;
			this.m_hitAddBuffs = null;
			this.m_controller = null;
		}

		public int m_bulletId;

		public int m_bulletType;

		public int m_delayFrame;

		public string m_parameters;

		public string m_hitAddBuffs = string.Empty;

		public int frame;

		public int m_fireBulletID;

		private BaseBattleServerController m_controller;

		[Serializable]
		public class TriggerBuffData
		{
			public int probability;

			public List<int> buffIDs = new List<int>();
		}
	}
}
