using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chat
{
	public sealed class ChatGuildShowItemRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChatGuildShowItemRequest> Parser
		{
			get
			{
				return ChatGuildShowItemRequest._parser;
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
		public uint ItemType
		{
			get
			{
				return this.itemType_;
			}
			set
			{
				this.itemType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
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
			if (this.ItemType != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ItemType);
			}
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.RowId);
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
			if (this.ItemType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ItemType);
			}
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
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
							this.RowId = input.ReadUInt64();
						}
					}
					else
					{
						this.ItemType = input.ReadUInt32();
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

		private static readonly MessageParser<ChatGuildShowItemRequest> _parser = new MessageParser<ChatGuildShowItemRequest>(() => new ChatGuildShowItemRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ItemTypeFieldNumber = 2;

		private uint itemType_;

		public const int RowIdFieldNumber = 3;

		private ulong rowId_;
	}
}
