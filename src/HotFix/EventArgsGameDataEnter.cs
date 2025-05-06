using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsGameDataEnter : BaseEventArgs
	{
		public void SetData(GameModel model, object data = null)
		{
			this.m_gameModel = model;
			this.m_data = data;
		}

		public override void Clear()
		{
		}

		public GameModel m_gameModel;

		public object m_data;
	}
}
