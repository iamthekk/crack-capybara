using System;
using Framework.EventSystem;
using Habby.Mail.Data;

namespace HotFix
{
	public class EventMailReceiveAwards : BaseEventArgs
	{
		public void SetData(MailRewardObject MailReceiveAwardsResponse)
		{
			this.mailReceiveAwardsResponse = MailReceiveAwardsResponse;
		}

		public override void Clear()
		{
			this.mailReceiveAwardsResponse = null;
		}

		public MailRewardObject mailReceiveAwardsResponse;
	}
}
