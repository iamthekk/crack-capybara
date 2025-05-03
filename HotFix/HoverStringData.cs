using System;
using Server;

namespace HotFix
{
	public class HoverStringData
	{
		public string strParam { get; private set; }

		public int ownerId { get; private set; }

		public MemberCamp memberCamp { get; private set; }

		public void SetData(string param, int id, MemberCamp camp)
		{
			this.strParam = param;
			this.ownerId = id;
			this.memberCamp = camp;
		}
	}
}
