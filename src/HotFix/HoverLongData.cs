using System;
using Server;

namespace HotFix
{
	public class HoverLongData
	{
		public long param { get; private set; }

		public int ownerId { get; private set; }

		public MemberCamp memberCamp { get; private set; }

		public void SetData(long param)
		{
			this.param = param;
		}

		public void SetData(long param, int ownerId, MemberCamp camp)
		{
			this.param = param;
			this.ownerId = ownerId;
			this.memberCamp = camp;
		}
	}
}
