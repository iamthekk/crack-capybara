using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.SevenDayTask
{
	public sealed class DropItemDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DropItemDto> Parser
		{
			get
			{
				return DropItemDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int ItemId
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
		public int ItemNum
		{
			get
			{
				return this.itemNum_;
			}
			set
			{
				this.itemNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Index
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
			if (this.ItemId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ItemId);
			}
			if (this.ItemNum != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ItemNum);
			}
			if (this.Index != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Index);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ItemId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ItemId);
			}
			if (this.ItemNum != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ItemNum);
			}
			if (this.Index != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Index);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 16U)
				{
					if (num != 24U)
					{
						if (num != 32U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Index = input.ReadInt32();
						}
					}
					else
					{
						this.ItemNum = input.ReadInt32();
					}
				}
				else
				{
					this.ItemId = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<DropItemDto> _parser = new MessageParser<DropItemDto>(() => new DropItemDto());

		public const int ItemIdFieldNumber = 2;

		private int itemId_;

		public const int ItemNumFieldNumber = 3;

		private int itemNum_;

		public const int IndexFieldNumber = 4;

		private int index_;
	}
}
