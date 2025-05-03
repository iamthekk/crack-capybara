using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Item
{
	public sealed class ItemUseRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ItemUseRequest> Parser
		{
			get
			{
				return ItemUseRequest._parser;
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
		public uint Count
		{
			get
			{
				return this.count_;
			}
			set
			{
				this.count_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Index
		{
			get
			{
				return this.index_;
			}
			set
			{
				this.index_ = value;
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
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.RowId);
			}
			if (this.Count != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Count);
			}
			if (this.Index != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Index);
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
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			if (this.Count != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Count);
			}
			if (this.Index != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Index);
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
						this.RowId = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Count = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.Index = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ItemUseRequest> _parser = new MessageParser<ItemUseRequest>(() => new ItemUseRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RowIdFieldNumber = 2;

		private ulong rowId_;

		public const int CountFieldNumber = 3;

		private uint count_;

		public const int IndexFieldNumber = 4;

		private uint index_;
	}
}
