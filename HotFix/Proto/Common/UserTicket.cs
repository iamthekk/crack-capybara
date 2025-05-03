using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UserTicket : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserTicket> Parser
		{
			get
			{
				return UserTicket._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint TicketType
		{
			get
			{
				return this.ticketType_;
			}
			set
			{
				this.ticketType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint NewNum
		{
			get
			{
				return this.newNum_;
			}
			set
			{
				this.newNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BuyTimes
		{
			get
			{
				return this.buyTimes_;
			}
			set
			{
				this.buyTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong TicketTimestamp
		{
			get
			{
				return this.ticketTimestamp_;
			}
			set
			{
				this.ticketTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint RevertLimit
		{
			get
			{
				return this.revertLimit_;
			}
			set
			{
				this.revertLimit_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.TicketType != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.TicketType);
			}
			if (this.NewNum != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.NewNum);
			}
			if (this.BuyTimes != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.BuyTimes);
			}
			if (this.TicketTimestamp != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.TicketTimestamp);
			}
			if (this.RevertLimit != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.RevertLimit);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.TicketType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TicketType);
			}
			if (this.NewNum != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.NewNum);
			}
			if (this.BuyTimes != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyTimes);
			}
			if (this.TicketTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TicketTimestamp);
			}
			if (this.RevertLimit != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RevertLimit);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.TicketType = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.NewNum = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.BuyTimes = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.TicketTimestamp = input.ReadUInt64();
						continue;
					}
					if (num == 40U)
					{
						this.RevertLimit = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserTicket> _parser = new MessageParser<UserTicket>(() => new UserTicket());

		public const int TicketTypeFieldNumber = 1;

		private uint ticketType_;

		public const int NewNumFieldNumber = 2;

		private uint newNum_;

		public const int BuyTimesFieldNumber = 3;

		private uint buyTimes_;

		public const int TicketTimestampFieldNumber = 4;

		private ulong ticketTimestamp_;

		public const int RevertLimitFieldNumber = 5;

		private uint revertLimit_;
	}
}
