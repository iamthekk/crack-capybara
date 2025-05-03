using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chat
{
	public sealed class ChatGetMessageRecordsRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChatGetMessageRecordsRequest> Parser
		{
			get
			{
				return ChatGetMessageRecordsRequest._parser;
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
		public uint GroupType
		{
			get
			{
				return this.groupType_;
			}
			set
			{
				this.groupType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GroupId
		{
			get
			{
				return this.groupId_;
			}
			set
			{
				this.groupId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.GroupType != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.GroupType);
			}
			if (this.GroupId.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.GroupId);
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
			if (this.GroupType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GroupType);
			}
			if (this.GroupId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GroupId);
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
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.MsgId = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.PageIndex = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.GroupType = input.ReadUInt32();
						continue;
					}
					if (num == 42U)
					{
						this.GroupId = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChatGetMessageRecordsRequest> _parser = new MessageParser<ChatGetMessageRecordsRequest>(() => new ChatGetMessageRecordsRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int MsgIdFieldNumber = 2;

		private ulong msgId_;

		public const int PageIndexFieldNumber = 3;

		private uint pageIndex_;

		public const int GroupTypeFieldNumber = 4;

		private uint groupType_;

		public const int GroupIdFieldNumber = 5;

		private string groupId_ = "";
	}
}
