using System;
using Framework.EventSystem;
using Habby.Mail.Data;

namespace HotFix
{
	public class EventArgsReadMail : BaseEventArgs
	{
		public void SetData(MailData mailData)
		{
			this.mailData = mailData;
		}

		public override void Clear()
		{
			this.mailData = null;
		}

		public MailData mailData;
	}
}
