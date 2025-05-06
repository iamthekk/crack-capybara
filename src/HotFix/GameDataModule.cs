using System;
using Framework.DataModule;
using Framework.EventSystem;

namespace HotFix
{
	public class GameDataModule : IDataModule
	{
		public GameModel Model
		{
			get
			{
				return this.m_gameModel;
			}
		}

		public int GetName()
		{
			return 110;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameData_GameEnter, new HandlerEvent(this.OnEventGameEnter));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		private void OnEventGameEnter(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsGameDataEnter eventArgsGameDataEnter = eventargs as EventArgsGameDataEnter;
			if (eventArgsGameDataEnter == null)
			{
				return;
			}
			this.m_gameModel = eventArgsGameDataEnter.m_gameModel;
		}

		private GameModel m_gameModel;
	}
}
