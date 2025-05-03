using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildDonationGetOperationRecordsRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildDonationGetOperationRecordsRequest> Parser
		{
			get
			{
				return GuildDonationGetOperationRecordsRequest._parser;
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
		public uint PageIndex
		{
			get
			{
				return this.pageIndex_;
			}
			set
			{
				this.pageIndex_ = value;
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
			if (this.PageIndex != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.PageIndex);
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
			if (this.PageIndex != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PageIndex);
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
							this.PageIndex = input.ReadUInt32();
						}
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

		private static readonly MessageParser<GuildDonationGetOperationRecordsRequest> _parser = new MessageParser<GuildDonationGetOperationRecordsRequest>(() => new GuildDonationGetOperationRecordsRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int MsgIdFieldNumber = 2;

		private ulong msgId_;

		public const int PageIndexFieldNumber = 3;

		private uint pageIndex_;
	}
}
