using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class CostDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CostDto> Parser
		{
			get
			{
				return CostDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Count
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
			if (this.Type != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Type);
			}
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.Count != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.Count);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
			}
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.Count != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Count);
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
							this.Count = input.ReadInt64();
						}
					}
					else
					{
						this.ConfigId = input.ReadUInt32();
					}
				}
				else
				{
					this.Type = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<CostDto> _parser = new MessageParser<CostDto>(() => new CostDto());

		public const int TypeFieldNumber = 1;

		private uint type_;

		public const int ConfigIdFieldNumber = 2;

		private uint configId_;

		public const int CountFieldNumber = 3;

		private long count_;
	}
}
