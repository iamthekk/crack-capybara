using System;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class BattleConquerDataModule : IDataModule
	{
		public PVPRecordDto Record
		{
			get
			{
				return this.m_record;
			}
		}

		public int Duration
		{
			get
			{
				return this.m_duration;
			}
		}

		public int GetName()
		{
			return 113;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_BattleConquer_BattleConquerEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_BattleConquer_BattleConquerEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void Reset()
		{
			this.m_record = null;
			this.m_isRecord = false;
		}

		private void OnBattleEnter(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBattleConquerEnter eventArgsBattleConquerEnter = eventargs as EventArgsBattleConquerEnter;
			if (eventArgsBattleConquerEnter == null)
			{
				return;
			}
			this.m_record = eventArgsBattleConquerEnter.m_record;
			this.m_isRecord = eventArgsBattleConquerEnter.m_isRecord;
		}

		public bool IsWin()
		{
			if (this.Record == null)
			{
				return false;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			bool flag = this.Record.Result != 0;
			if (dataModule.userId != this.Record.OwnerUser.UserId)
			{
				return !flag;
			}
			return flag;
		}

		[SerializeField]
		private PVPRecordDto m_record;

		public bool m_isRecord;

		[SerializeField]
		private int m_duration = 15;
	}
}
