using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsTicketUpdate : BaseEventArgs
	{
		public override void Clear()
		{
			this.TicketKind = UserTicketKind.None;
			this.OldTicketValue = null;
			this.NewTicketValue = null;
		}

		public void SetData(UserTicketKind kind, UserTicket oldvalue, UserTicket newvalue)
		{
			this.TicketKind = kind;
			this.OldTicketValue = oldvalue;
			this.NewTicketValue = newvalue;
		}

		public UserTicketKind TicketKind;

		public UserTicket OldTicketValue;

		public UserTicket NewTicketValue;
	}
}
