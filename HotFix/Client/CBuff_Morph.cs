using System;
using Server;

namespace HotFix.Client
{
	public class CBuff_Morph : CBuffBase
	{
		public override void ReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<CBuff_Morph.Data>(parameters) : new CBuff_Morph.Data());
		}

		public CBuff_Morph.Data m_data;

		public class Data
		{
			public int MorphId;
		}
	}
}
