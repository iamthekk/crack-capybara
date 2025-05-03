using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgTaskViewReceiveAchievementRewardTask : BaseEventArgs
	{
		public void SetData(int id, int updateID)
		{
			this.m_id = id;
			this.m_updateID = updateID;
		}

		public override void Clear()
		{
		}

		public int m_id;

		public int m_updateID;
	}
}
