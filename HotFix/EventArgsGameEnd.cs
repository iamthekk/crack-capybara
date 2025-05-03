using System;
using Framework.EventSystem;
using HotFix.Client;

namespace HotFix
{
	public class EventArgsGameEnd : BaseEventArgs
	{
		public void SetData(GameOverType gameOverType)
		{
			this.m_gameOverType = gameOverType;
		}

		public override void Clear()
		{
			this.m_gameOverType = GameOverType.Win;
		}

		public GameOverType m_gameOverType;
	}
}
