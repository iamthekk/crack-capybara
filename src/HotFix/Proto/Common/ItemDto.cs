using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ItemDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ItemDto> Parser
		{
			get
			{
				return ItemDto._parser;
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
		public uint ItemId
		{
			get
			{
				return this.itemId_;
			}
			set
			{
				this.itemId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong Count
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.RowId);
			}
			if (this.ItemId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ItemId);
			}
			if (this.Count != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.Count);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			if (this.ItemId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ItemId);
			}
			if (this.Count != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Count);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Count = input.ReadUInt64();
						}
					}
					else
					{
						this.ItemId = input.ReadUInt32();
					}
				}
				else
				{
					this.RowId = input.ReadUInt64();
				}
			}
		}

		private static readonly MessageParser<ItemDto> _parser = new MessageParser<ItemDto>(() => new ItemDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int ItemIdFieldNumber = 2;

		private uint itemId_;

		public const int CountFieldNumber = 3;

		private ulong count_;
	}
}
