using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix.Logic.BattleMain
{
	public class BattleMainWaveManager
	{
		public void OnInit()
		{
			int num = 1;
			BattleMain_chapter elementById = GameApp.Table.GetManager().GetBattleMain_chapterModelInstance().GetElementById(num);
			int waveStartId = elementById.waveStartId;
			this.m_list = new List<MainWaveData>();
			for (int i = 0; i < elementById.waveCount; i++)
			{
				MainWaveData mainWaveData = new MainWaveData();
				mainWaveData.m_id = i;
				int num2 = waveStartId + i;
				BattleMain_wave elementById2 = GameApp.Table.GetManager().GetBattleMain_waveModelInstance().GetElementById(num2);
				mainWaveData.m_waveType = (MainWaveType)elementById2.roomType;
				mainWaveData.m_memberList = new List<int>();
				mainWaveData.m_memberList.Add(elementById2.position0);
				mainWaveData.m_memberList.Add(elementById2.position1);
				mainWaveData.m_memberList.Add(elementById2.position2);
				mainWaveData.m_attackUpgrade = elementById2.attackUpgrade;
				mainWaveData.m_hpUpgrade = elementById2.hpUpgrade;
				mainWaveData.m_attributes = new List<string>();
				for (int j = 0; j < elementById2.attributes.Length; j++)
				{
					mainWaveData.m_attributes.Add(elementById2.attributes[j]);
				}
				this.m_list.Add(mainWaveData);
			}
		}

		public MainWaveData GetWaveData(int index)
		{
			if (index < 0 || index >= this.m_list.Count)
			{
				HLog.LogError(string.Format("BattleMainRoomManager.GetWave index error: {0}", index));
				return null;
			}
			return this.m_list[index];
		}

		public int GetWaveCount()
		{
			return this.m_list.Count;
		}

		public List<MainWaveData> GetList()
		{
			return this.m_list;
		}

		private List<MainWaveData> m_list;
	}
}
