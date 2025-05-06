using System;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Tower;
using UnityEngine;

namespace HotFix
{
	public class BattleTowerDataModule : IDataModule
	{
		public int Duration
		{
			get
			{
				return this.m_duration;
			}
		}

		public int GetName()
		{
			return 116;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_BattleTower_BattleTowerEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_BattleTower_BattleTowerEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void Reset()
		{
		}

		private void OnBattleEnter(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBattleTowerEnter eventArgsBattleTowerEnter = eventargs as EventArgsBattleTowerEnter;
			if (eventArgsBattleTowerEnter == null)
			{
				return;
			}
			this.m_towerChallengeResponse = eventArgsBattleTowerEnter.m_towerChallengeResponse;
		}

		public TowerChallengeResponse m_towerChallengeResponse;

		[SerializeField]
		private int m_duration = 15;
	}
}
