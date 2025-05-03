using System;
using Framework.EventSystem;
using Server;

namespace HotFix
{
	public class EventArgsBattleStart : BaseEventArgs
	{
		public void SetData(BattleReportData_GameStart data)
		{
			this.data = data;
		}

		public override void Clear()
		{
		}

		public BattleReportData_GameStart data;
	}
}
