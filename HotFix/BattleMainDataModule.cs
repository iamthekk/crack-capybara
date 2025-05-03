using System;
using System.Collections.Generic;
using Framework.DataModule;
using Framework.EventSystem;
using HotFix.Logic.BattleMain;

namespace HotFix
{
	public class BattleMainDataModule : IDataModule
	{
		public int GetName()
		{
			return 111;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_Data_EnterBattle, new HandlerEvent(this.OnEventEnterBattle));
			manager.RegisterEvent(LocalMessageName.CC_Data_CurrentRoundFinish, new HandlerEvent(this.OnEventCurrentRoundFinish));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_Data_EnterBattle, new HandlerEvent(this.OnEventEnterBattle));
			manager.UnRegisterEvent(LocalMessageName.CC_Data_CurrentRoundFinish, new HandlerEvent(this.OnEventCurrentRoundFinish));
		}

		public void Reset()
		{
		}

		public int GetWaveCount()
		{
			return this.m_waveManager.GetWaveCount();
		}

		public MainWaveData GetWaveData(int index)
		{
			return this.m_waveManager.GetWaveData(index);
		}

		public List<MainWaveData> GetWaveList()
		{
			return this.m_waveManager.GetList();
		}

		public int GetCurrentWaveIndex()
		{
			return this.m_currentWaveIndex;
		}

		public bool IsLastRound()
		{
			return this.m_currentWaveIndex >= this.GetWaveCount();
		}

		private void OnEventEnterBattle(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.m_waveManager = new BattleMainWaveManager();
			this.m_waveManager.OnInit();
			this.m_currentWaveIndex = 0;
		}

		private void OnEventCurrentRoundFinish(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.m_currentWaveIndex++;
		}

		private BattleMainWaveManager m_waveManager;

		private int m_currentWaveIndex;

		private int m_waveMaxIndex;

		public bool m_isCloseServer;
	}
}
