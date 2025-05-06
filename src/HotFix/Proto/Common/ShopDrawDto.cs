using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class ShopDrawDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopDrawDto> Parser
		{
			get
			{
				return ShopDrawDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> MiniDrawCount
		{
			get
			{
				return this.miniDrawCount_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> HardDrawCount
		{
			get
			{
				return this.hardDrawCount_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> FreeCostTimes
		{
			get
			{
				return this.freeCostTimes_;
			}
		}

		[DebuggerNonUserCode]
		public ulong DayResetTimeStamp
		{
			get
			{
				return this.dayResetTimeStamp_;
			}
			set
			{
				this.dayResetTimeStamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> DayTotalTimes
		{
			get
			{
				return this.dayTotalTimes_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.miniDrawCount_.WriteTo(output, ShopDrawDto._map_miniDrawCount_codec);
			this.hardDrawCount_.WriteTo(output, ShopDrawDto._map_hardDrawCount_codec);
			this.freeCostTimes_.WriteTo(output, ShopDrawDto._map_freeCostTimes_codec);
			if (this.DayResetTimeStamp != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.DayResetTimeStamp);
			}
			this.dayTotalTimes_.WriteTo(output, ShopDrawDto._map_dayTotalTimes_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.miniDrawCount_.CalculateSize(ShopDrawDto._map_miniDrawCount_codec);
			num += this.hardDrawCount_.CalculateSize(ShopDrawDto._map_hardDrawCount_codec);
			num += this.freeCostTimes_.CalculateSize(ShopDrawDto._map_freeCostTimes_codec);
			if (this.DayResetTimeStamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.DayResetTimeStamp);
			}
			return num + this.dayTotalTimes_.CalculateSize(ShopDrawDto._map_dayTotalTimes_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 10U)
					{
						this.miniDrawCount_.AddEntriesFrom(input, ShopDrawDto._map_miniDrawCount_codec);
						continue;
					}
					if (num == 18U)
					{
						this.hardDrawCount_.AddEntriesFrom(input, ShopDrawDto._map_hardDrawCount_codec);
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.freeCostTimes_.AddEntriesFrom(input, ShopDrawDto._map_freeCostTimes_codec);
						continue;
					}
					if (num == 32U)
					{
						this.DayResetTimeStamp = input.ReadUInt64();
						continue;
					}
					if (num == 42U)
					{
						this.dayTotalTimes_.AddEntriesFrom(input, ShopDrawDto._map_dayTotalTimes_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopDrawDto> _parser = new MessageParser<ShopDrawDto>(() => new ShopDrawDto());

		public const int MiniDrawCountFieldNumber = 1;

		private static readonly MapField<uint, uint>.Codec _map_miniDrawCount_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 10U);

		private readonly MapField<uint, uint> miniDrawCount_ = new MapField<uint, uint>();

		public const int HardDrawCountFieldNumber = 2;

		private static readonly MapField<uint, uint>.Codec _map_hardDrawCount_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 18U);

		private readonly MapField<uint, uint> hardDrawCount_ = new MapField<uint, uint>();

		public const int FreeCostTimesFieldNumber = 3;

		private static readonly MapField<uint, uint>.Codec _map_freeCostTimes_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 26U);

		private readonly MapField<uint, uint> freeCostTimes_ = new MapField<uint, uint>();

		public const int DayResetTimeStampFieldNumber = 4;

		private ulong dayResetTimeStamp_;

		public const int DayTotalTimesFieldNumber = 5;

		private static readonly MapField<uint, uint>.Codec _map_dayTotalTimes_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 42U);

		private readonly MapField<uint, uint> dayTotalTimes_ = new MapField<uint, uint>();
	}
}
