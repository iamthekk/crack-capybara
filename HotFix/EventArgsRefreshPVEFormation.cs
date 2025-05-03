using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshPVEFormation : BaseEventArgs
	{
		public void SetData(List<int> formation)
		{
			this.m_formation.Clear();
			this.m_formation.AddRange(formation);
		}

		public override void Clear()
		{
			this.m_formation.Clear();
		}

		public List<int> m_formation = new List<int>();
	}
}
