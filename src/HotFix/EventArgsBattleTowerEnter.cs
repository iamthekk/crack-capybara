using System;
using Framework.EventSystem;
using Proto.Tower;

namespace HotFix
{
	public class EventArgsBattleTowerEnter : BaseEventArgs
	{
		public void SetData(TowerChallengeResponse towerChallengeResponse)
		{
			this.m_towerChallengeResponse = towerChallengeResponse;
		}

		public override void Clear()
		{
		}

		public TowerChallengeResponse m_towerChallengeResponse;
	}
}
