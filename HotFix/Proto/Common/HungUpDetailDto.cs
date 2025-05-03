using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class HungUpDetailDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HungUpDetailDto> Parser
		{
			get
			{
				return HungUpDetailDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int GoldDrop
		{
			get
			{
				return this.goldDrop_;
			}
			set
			{
				this.goldDrop_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int GemDrop
		{
			get
			{
				return this.gemDrop_;
			}
			set
			{
				this.gemDrop_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Drops
		{
			get
			{
				return this.drops_;
			}
		}

		[DebuggerNonUserCode]
		public long LastSettlementTime
		{
			get
			{
				return this.lastSettlementTime_;
			}
			set
			{
				this.lastSettlementTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.GoldDrop != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.GoldDrop);
			}
			if (this.GemDrop != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.GemDrop);
			}
			this.drops_.WriteTo(output, HungUpDetailDto._repeated_drops_codec);
			if (this.LastSettlementTime != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.LastSettlementTime);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.GoldDrop != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.GoldDrop);
			}
			if (this.GemDrop != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.GemDrop);
			}
			num += this.drops_.CalculateSize(HungUpDetailDto._repeated_drops_codec);
			if (this.LastSettlementTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.LastSettlementTime);
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
						this.GoldDrop = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.GemDrop = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.drops_.AddEntriesFrom(input, HungUpDetailDto._repeated_drops_codec);
						continue;
					}
					if (num == 32U)
					{
						this.LastSettlementTime = input.ReadInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<HungUpDetailDto> _parser = new MessageParser<HungUpDetailDto>(() => new HungUpDetailDto());

		public const int GoldDropFieldNumber = 1;

		private int goldDrop_;

		public const int GemDropFieldNumber = 2;

		private int gemDrop_;

		public const int DropsFieldNumber = 3;

		private static readonly FieldCodec<RewardDto> _repeated_drops_codec = FieldCodec.ForMessage<RewardDto>(26U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> drops_ = new RepeatedField<RewardDto>();

		public const int LastSettlementTimeFieldNumber = 4;

		private long lastSettlementTime_;
	}
}
