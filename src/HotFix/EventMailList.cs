using System;
using System.Collections.Generic;
using Framework.EventSystem;
using Habby.Mail.Data;
using Proto.Mail;

namespace HotFix
{
	public class EventMailList : BaseEventArgs
	{
		public void SetData(MailGetListResponse MailGetListRequest, Action Callback)
		{
			this.mailGetListResponse = MailGetListRequest;
			this.callback = Callback;
		}

		public void SetData(List<MailData> mailDatas)
		{
			this.m_mailDatas = mailDatas;
		}

		public override void Clear()
		{
			this.mailGetListResponse = null;
			this.callback = null;
		}

		public MailGetListResponse mailGetListResponse;

		public Action callback;

		public List<MailData> m_mailDatas = new List<MailData>();
	}
}
