using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildDonationReceiveRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildDonationReceiveRequest> Parser
		{
			get
			{
				return GuildDonationReceiveRequest._parser;
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
		public ulong MsgId
		{
			get
			{
				return this.msgId_;
			}
			set
			{
				this.msgId_ = value;
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
			if (this.MsgId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.MsgId);
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
			if (this.MsgId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.MsgId);
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
						input.SkipLastField();
					}
					else
					{
						this.MsgId = input.ReadUInt64();
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

		private static readonly MessageParser<GuildDonationReceiveRequest> _parser = new MessageParser<GuildDonationReceiveRequest>(() => new GuildDonationReceiveRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int MsgIdFieldNumber = 2;

		private ulong msgId_;
	}
}
