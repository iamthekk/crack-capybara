using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopBuyTicketsRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopBuyTicketsRequest> Parser
		{
			get
			{
				return ShopBuyTicketsRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
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
		public uint TicketNum
		{
			get
			{
				return this.ticketNum_;
			}
			set
			{
				this.ticketNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.TicketType != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.TicketType);
			}
			if (this.TicketNum != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.TicketNum);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.TicketType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TicketType);
			}
			if (this.TicketNum != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TicketNum);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.TicketNum = input.ReadUInt32();
						}
					}
					else
					{
						this.TicketType = input.ReadUInt32();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<ShopBuyTicketsRequest> _parser = new MessageParser<ShopBuyTicketsRequest>(() => new ShopBuyTicketsRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int TicketTypeFieldNumber = 2;

		private uint ticketType_;

		public const int TicketNumFieldNumber = 3;

		private uint ticketNum_;
	}
}
