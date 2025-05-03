using System;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.ActTime;

namespace HotFix
{
	public class ActTimeRankDataModule : IDataModule
	{
		public ActTimeRankResponse Data
		{
			get
			{
				return this.m_response;
			}
			set
			{
				this.m_response = value;
			}
		}

		public long LastLoadRankDataTime { get; private set; }

		public int GetName()
		{
			return 138;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_ActTimeRank, new HandlerEvent(this.OnEventSetData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_ActTimeRank, new HandlerEvent(this.OnEventSetData));
		}

		public void Reset()
		{
			this.m_response = null;
			this.ResetLoadRankDataTime();
		}

		public void ResetLoadRankDataTime()
		{
			this.LastLoadRankDataTime = 0L;
		}

		private void OnEventSetData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsActTimeRankData eventArgsActTimeRankData = eventargs as EventArgsActTimeRankData;
			if (eventArgsActTimeRankData != null)
			{
				this.m_response = eventArgsActTimeRankData.Response;
				this.LastLoadRankDataTime = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC;
			}
		}

		private ActTimeRankResponse m_response;
	}
}
