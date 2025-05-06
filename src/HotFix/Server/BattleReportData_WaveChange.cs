using System;
using System.Collections.Generic;

namespace Server
{
	public class BattleReportData_WaveChange : BaseBattleReportData
	{
		public int CurWave { get; private set; }

		public int MaxWave { get; private set; }

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.WaveChange;
			}
		}

		public void SetData(BaseBattleServerController controller, int curWave, int maxWave)
		{
			this.CurWave = curWave;
			this.MaxWave = maxWave;
			this.m_members.Clear();
			List<CardData> waveData = controller.InData.GetWaveData(this.CurWave);
			for (int i = 0; i < waveData.Count; i++)
			{
				int instanceID = waveData[i].m_instanceID;
				SMemberBase member = controller.memberFactory.GetMember(instanceID);
				if (member != null)
				{
					BattleReportData_WaveChange.WaveChangeMemberData waveChangeMemberData = new BattleReportData_WaveChange.WaveChangeMemberData();
					waveChangeMemberData.m_instanceId = member.m_instanceId;
					waveChangeMemberData.m_curHp = member.memberData.CurHP;
					waveChangeMemberData.m_maxHp = member.memberData.attribute.GetHpMax();
					waveChangeMemberData.m_curRecharge = member.memberData.CurRecharge;
					waveChangeMemberData.m_maxRecharge = member.memberData.attribute.RechargeMax;
					waveChangeMemberData.m_curLegacyPower.Clear();
					foreach (KeyValuePair<int, FP> keyValuePair in member.memberData.CurLegacyPowerDict)
					{
						waveChangeMemberData.m_curLegacyPower.Add(keyValuePair.Key, keyValuePair.Value);
					}
					waveChangeMemberData.m_maxLegacyPower.Clear();
					foreach (KeyValuePair<int, FP> keyValuePair2 in member.memberData.MaxLegacyPowerDict)
					{
						waveChangeMemberData.m_maxLegacyPower.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					waveChangeMemberData.m_attack = member.memberData.attribute.GetAttack();
					waveChangeMemberData.m_defense = member.memberData.attribute.GetDefence();
					waveChangeMemberData.m_isUsedRevive = member.memberData.IsReviveUsed;
					this.m_members.Add(waveChangeMemberData.m_instanceId, waveChangeMemberData);
				}
			}
		}

		public override string ToString()
		{
			return string.Format("CurWave:{0}, MaxWave:{1}\n", this.CurWave, this.MaxWave);
		}

		public Dictionary<int, BattleReportData_WaveChange.WaveChangeMemberData> m_members = new Dictionary<int, BattleReportData_WaveChange.WaveChangeMemberData>();

		public class WaveChangeMemberData
		{
			public override string ToString()
			{
				return string.Format("m_instanceId:{0}, m_curHp:{1}, m_curHp:{2}, m_curRecharge:{3}, m_maxRecharge:{4}, m_curLegacyPower:{5}, m_maxLegacyPower:{6}, m_attack:{7}, m_defense:{8}, \n", new object[] { this.m_instanceId, this.m_curHp, this.m_maxHp, this.m_curRecharge, this.m_maxRecharge, this.m_curLegacyPower, this.m_maxLegacyPower, this.m_attack, this.m_defense });
			}

			public int m_instanceId;

			public FP m_curHp;

			public FP m_maxHp;

			public FP m_curRecharge;

			public FP m_maxRecharge;

			public Dictionary<int, FP> m_curLegacyPower = new Dictionary<int, FP>();

			public Dictionary<int, FP> m_maxLegacyPower = new Dictionary<int, FP>();

			public FP m_attack;

			public FP m_defense;

			public bool m_isUsedRevive;
		}
	}
}
