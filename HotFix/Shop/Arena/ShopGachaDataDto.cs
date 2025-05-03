using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Shop.Arena
{
	public sealed class ShopGachaDataDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopGachaDataDto> Parser
		{
			get
			{
				return ShopGachaDataDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Id
		{
			get
			{
				return this.id_;
			}
			set
			{
				this.id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint OpenType
		{
			get
			{
				return this.openType_;
			}
			set
			{
				this.openType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong OpenTimestamp
		{
			get
			{
				return this.openTimestamp_;
			}
			set
			{
				this.openTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong EndTimestamp
		{
			get
			{
				return this.endTimestamp_;
			}
			set
			{
				this.endTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint GachaCount
		{
			get
			{
				return this.gachaCount_;
			}
			set
			{
				this.gachaCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Id != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Id);
			}
			if (this.OpenType != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.OpenType);
			}
			if (this.OpenTimestamp != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.OpenTimestamp);
			}
			if (this.EndTimestamp != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.EndTimestamp);
			}
			if (this.GachaCount != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.GachaCount);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Id != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Id);
			}
			if (this.OpenType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.OpenType);
			}
			if (this.OpenTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.OpenTimestamp);
			}
			if (this.EndTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.EndTimestamp);
			}
			if (this.GachaCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GachaCount);
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
						this.Id = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.OpenType = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.OpenTimestamp = input.ReadUInt64();
						continue;
					}
					if (num == 32U)
					{
						this.EndTimestamp = input.ReadUInt64();
						continue;
					}
					if (num == 40U)
					{
						this.GachaCount = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopGachaDataDto> _parser = new MessageParser<ShopGachaDataDto>(() => new ShopGachaDataDto());

		public const int IdFieldNumber = 1;

		private uint id_;

		public const int OpenTypeFieldNumber = 2;

		private uint openType_;

		public const int OpenTimestampFieldNumber = 3;

		private ulong openTimestamp_;

		public const int EndTimestampFieldNumber = 4;

		private ulong endTimestamp_;

		public const int GachaCountFieldNumber = 5;

		private uint gachaCount_;
	}
}
