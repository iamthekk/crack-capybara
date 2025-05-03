using System;
using System.Collections.Generic;

namespace Server
{
	public class SBuff_RemoveBuff : SBuffBase
	{
		protected override void OnInit()
		{
			List<int> list = new List<int>(this.m_data.BuffTypes);
			this.m_owner.buffFactory.RemoveBuffByTypes(list);
		}

		protected override void OnDeInit()
		{
		}

		protected override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.LitJson_ToObjectFp<SBuff_RemoveBuff.Data>(parameters) : new SBuff_RemoveBuff.Data());
		}

		public SBuff_RemoveBuff.Data m_data;

		public class Data
		{
			public int[] BuffTypes;
		}
	}
}
