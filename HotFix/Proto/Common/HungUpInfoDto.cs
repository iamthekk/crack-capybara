using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class HungUpInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HungUpInfoDto> Parser
		{
			get
			{
				return HungUpInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, HungUpDetailDto> HungUpRewards
		{
			get
			{
				return this.hungUpRewards_;
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
		public int TotalSettleTime
		{
			get
			{
				return this.totalSettleTime_;
			}
			set
			{
				this.totalSettleTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.hungUpRewards_.WriteTo(output, HungUpInfoDto._map_hungUpRewards_codec);
			if (this.LastSettlementTime != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.LastSettlementTime);
			}
			if (this.TotalSettleTime != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.TotalSettleTime);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.hungUpRewards_.CalculateSize(HungUpInfoDto._map_hungUpRewards_codec);
			if (this.LastSettlementTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.LastSettlementTime);
			}
			if (this.TotalSettleTime != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TotalSettleTime);
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
							this.TotalSettleTime = input.ReadInt32();
						}
					}
					else
					{
						this.LastSettlementTime = input.ReadInt64();
					}
				}
				else
				{
					this.hungUpRewards_.AddEntriesFrom(input, HungUpInfoDto._map_hungUpRewards_codec);
				}
			}
		}

		private static readonly MessageParser<HungUpInfoDto> _parser = new MessageParser<HungUpInfoDto>(() => new HungUpInfoDto());

		public const int HungUpRewardsFieldNumber = 1;

		private static readonly MapField<uint, HungUpDetailDto>.Codec _map_hungUpRewards_codec = new MapField<uint, HungUpDetailDto>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<HungUpDetailDto>(18U, HungUpDetailDto.Parser), 10U);

		private readonly MapField<uint, HungUpDetailDto> hungUpRewards_ = new MapField<uint, HungUpDetailDto>();

		public const int LastSettlementTimeFieldNumber = 2;

		private long lastSettlementTime_;

		public const int TotalSettleTimeFieldNumber = 3;

		private int totalSettleTime_;
	}
}
